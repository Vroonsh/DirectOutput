using DirectOutput.LedControl.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DofConfigToolWrapper
{
    public class DofConfigToolFilesHandler
    {
        public string RootDirectory { get; set; } = string.Empty;
        private DofConfigToolSetup _DofSetup = null;
        public DofConfigToolSetup DofSetup
        {   get { return _DofSetup; }
            set {
                _DofSetup = value;
                ParseConfigFiles();
            }
        }

        public LedControlConfigList ConfigFiles { get; private set; } = new LedControlConfigList();
        public string UserLocalPath { get; private set; } = string.Empty;

        private int DofConfigToolVersion = 0;
        private List<string> MissingIniFiles = new List<string>();

        public void ParseConfigFiles()
        {
            try {
                ConfigFiles.Clear();
                MissingIniFiles.Clear();

                //Create directory if not already created
                var setupsPath = Path.Combine(RootDirectory, "setups");
                new DirectoryInfo(RootDirectory).CreateDirectoryPath();

                var userDir = string.Join("-", new string[] { DofSetup?.UserName, DofSetup?.APIKey });
                if (!userDir.IsNullOrEmpty()) {
                    UserLocalPath = Path.Combine(setupsPath, userDir);
                    new DirectoryInfo(UserLocalPath).CreateDirectoryPath();

                    foreach(var controller in DofSetup.ControllerSetups) {
                        var filePath = Path.Combine(UserLocalPath, $"directoutputconfig{controller.Number}.ini");
                        if (File.Exists(filePath)) {
                            ConfigFiles.Add(new LedControlConfig(filePath, controller.Number));
                        } else {
                            MissingIniFiles.Add(filePath);
                        }
                    }
                }

            } catch (Exception e) {
                throw new Exception("DofConfigToolFilesHandler cannot parse config files", e);
            }
        }

        private void ClearUserLocalPath()
        {
            if (!UserLocalPath.IsNullOrEmpty()) {
                Directory.Delete(UserLocalPath, true);
                Directory.CreateDirectory(UserLocalPath);
            }
        }

        private void RetrieveDofConfigToolVersion()
        {
            WebRequest request = WebRequest.Create("http://configtool.vpuniverse.com/api.php?query=version");
            request.Credentials = CredentialCache.DefaultCredentials;

            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream()) {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                DofConfigToolVersion = Int32.Parse(responseFromServer);
            }
            response.Close();
        }

        private void GatherAndExtractConfigFiles(WaitForm waitForm)
        {
            var url = "http://configtool.vpuniverse.com/api.php?query=getconfig&apikey=" + DofSetup.APIKey;
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;

            waitForm.Invoke((Action)(()=>waitForm.UpdateMessage($"Retrieving config files for user {DofSetup.UserName}...")));

            WebResponse response = request.GetResponse();

            waitForm.Invoke((Action)(() => waitForm.UpdateMessage("Extracting config files...")));

            ClearUserLocalPath();
            var outputZipFileName = Path.Combine(UserLocalPath, "directoutputconfig.zip");

            using (Stream dataStream = response.GetResponseStream()) {
                using (FileStream decompressedFileStream = File.Create(outputZipFileName)) {
                    dataStream.CopyTo(decompressedFileStream);
                }
            }

            response.Close();

            ZipFile.ExtractToDirectory(outputZipFileName, UserLocalPath);
            ParseConfigFiles();
        }

        private void RetrieveDofConfigToolFiles()
        {
            var task = Task.Factory.StartNew(() => { var f = new WaitForm(); f.ShowDialog(); f.Update(); });

            while (Application.OpenForms["WaitForm"] == null) {
                Application.DoEvents();
            }
            var waitForm = (WaitForm)Application.OpenForms["WaitForm"];
            GatherAndExtractConfigFiles(waitForm);

            waitForm.Invoke((Action)(() => waitForm.Close()));
        }

        public void UpdateConfigFiles(bool forceUpdate = false)
        {
            if (DofSetup.APIKey.IsNullOrEmpty()) {
                MessageBox.Show("Cannot update local config files. APIKey was not set in the current DofConfigToolSetup.", "Update config files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (UserLocalPath.IsNullOrEmpty()) {
                MessageBox.Show("Cannot update local config files. Local extract path was not correctly set using UserName & APIKey from the DofConfigToolSetup.", "Update config files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RetrieveDofConfigToolVersion();

            if (forceUpdate) {
                RetrieveDofConfigToolFiles();
            } else {
                if (ConfigFiles.Count == 0) {
                    ParseConfigFiles();
                }

                int newerLocalVersion = 0;
                foreach (var configfile in ConfigFiles) {
                    newerLocalVersion = Math.Max(newerLocalVersion, configfile.Version);
                }

                if (MissingIniFiles.Count > 0) {
                    if (MessageBox.Show($"There are missing local config files:\n" +
                                        $"{string.Join("\n", MissingIniFiles)}" +
                                        $"\nDo you want to update from DofConfigTool ?", "DofConfigTool files update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        RetrieveDofConfigToolFiles();
                    }
                } else if (newerLocalVersion < DofConfigToolVersion) {
                    if (MessageBox.Show($"Local config files are older than the DofConfigTool online version [{newerLocalVersion} => {DofConfigToolVersion}].\n" +
                                        $"Do you want to update them ?", "DofConfigTool files update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        RetrieveDofConfigToolFiles();
                    }
                }
            }
        }
    }
}
