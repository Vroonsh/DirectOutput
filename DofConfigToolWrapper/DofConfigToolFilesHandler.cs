using DirectOutput.LedControl.Loader;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                var userDir = string.Join("-", new string[] { DofSetup?.UserName, DofSetup?.APIKey });
                if (!userDir.IsNullOrEmpty()) {
                    var setupsPath = Path.Combine(RootDirectory, "setups");
                    new DirectoryInfo(RootDirectory).CreateDirectoryPath();
                    UserLocalPath = Path.Combine(setupsPath, userDir);
                    new DirectoryInfo(UserLocalPath).CreateDirectoryPath();
                }
            }
        }
        public enum EDofConfigToolConnectMethod
        {
            [Description("Internal Http requests")]
            InternalHttpRequest,
            [Description("Dofconfigtool Pull VBS")]
            PullVBScript,
            [Description("No Connection")]
            Offline
        };
        public EDofConfigToolConnectMethod DofConfigToolConnectMethod { get; set; } = EDofConfigToolConnectMethod.PullVBScript;
        public bool ForceDofConfigToolUpdate { get; set; } = false;

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

                if (!UserLocalPath.IsNullOrEmpty()) {
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

        private HttpClient _httpClient = new HttpClient() {
            Timeout = TimeSpan.FromMinutes(2)
        };

        private async Task RetrieveDofConfigToolVersionAsync()
        {
            try {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://configtool.vpuniverse.com/api.php?query=version");
                request.Headers.Add("user-agent", "Other");
                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                DofConfigToolVersion = Int32.Parse(responseBody);
            }catch (Exception e) {
                MessageBox.Show($"DofConfigTool site returned an error while fetching te last Dof version.\nCould be caused by too much requests from your IP.\nPlease wait a few minutes before retrying.\n\nException Message :\n{e.Message}"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<Stream> GatherConfigFilesAsync()
        {
            Stream dataStream = null;
            try {
                    var url = "https://configtool.vpuniverse.com/api.php?query=getconfig&apikey=" + DofSetup.APIKey;
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("user-agent", "Other");
                    HttpResponseMessage response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    dataStream = await response.Content.ReadAsStreamAsync();

                    if (dataStream != null) {
                        ClearUserLocalPath();
                        var outputZipFileName = Path.Combine(UserLocalPath, "directoutputconfig.zip");

                        using (FileStream decompressedFileStream = File.Create(outputZipFileName)) {
                            dataStream.CopyTo(decompressedFileStream);
                        }

                        ZipFile.ExtractToDirectory(outputZipFileName, UserLocalPath);
                    }
            } catch (Exception e) {
                MessageBox.Show($"DofConfigTool site returned an error while retrieving your last Dof config files.\nCould be caused by too much requests from your IP.\nPlease wait a few minutes before retrying.\n\nException Message :\n{e.Message}"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dataStream;
        }

        private void GatherConfigFilesVBAsync()
        {
            try {
                ClearUserLocalPath();
                Process scriptProc = new Process();
                scriptProc.StartInfo.FileName = $"{RootDirectory}\\VB\\ledcontrol_pull.vbs";
                scriptProc.StartInfo.WorkingDirectory = UserLocalPath;
                scriptProc.StartInfo.Arguments = $"/A={DofSetup.APIKey} /F=directoutputconfig.zip /T=\"{UserLocalPath}\" /-V /Y /L /F";
                scriptProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                scriptProc.Start();
                scriptProc.WaitForExit();
                scriptProc.Close();
            } catch (Exception ex) {
                MessageBox.Show($"DofConfigTool site returned an error while retrieving your last Dof config files usinf pull vbs.\nCheck logs in {UserLocalPath} for info.\n\nException Message :\n{ex.Message}"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RetrieveDofConfigToolFilesAsync()
        {
            try {
                WaitForm waitForm = null;

                var task = Task.Factory.StartNew(() => { var f = new WaitForm(); f.ShowDialog(); f.Update(); });

                while (Application.OpenForms["WaitForm"] == null) {
                    Application.DoEvents();
                }
                waitForm = (WaitForm)Application.OpenForms["WaitForm"];

                Stream configFilesStream = null;


                switch (DofConfigToolConnectMethod) {
                    case EDofConfigToolConnectMethod.InternalHttpRequest: {
                        waitForm?.Invoke((Action)(() => waitForm.UpdateMessage($"Retrieving config files for user {DofSetup.UserName}...")));
                        if (ForceDofConfigToolUpdate) {
                            configFilesStream = await GatherConfigFilesAsync();
                        } else {
                            await RetrieveDofConfigToolVersionAsync();

                            int newerLocalVersion = 0;
                            if (ConfigFiles.Count == 0) {
                                ParseConfigFiles();
                            }

                            foreach (var configfile in ConfigFiles) {
                                newerLocalVersion = Math.Max(newerLocalVersion, configfile.Version);
                            }

                            if (MissingIniFiles.Count > 0) {
                                if (MessageBox.Show($"There are missing local config files:\n" +
                                                    $"{string.Join("\n", MissingIniFiles)}" +
                                                    $"\nDo you want to update from DofConfigTool ?", "DofConfigTool files update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                                    configFilesStream = await GatherConfigFilesAsync();
                                }
                            } else if (newerLocalVersion < DofConfigToolVersion) {
                                if (MessageBox.Show($"Local config files are older than the DofConfigTool online version [{newerLocalVersion} => {DofConfigToolVersion}].\n" +
                                                    $"Do you want to update them ?", "DofConfigTool files update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                                    configFilesStream = await GatherConfigFilesAsync();
                                }
                            }
                        }
                        break;
                    }

                    case EDofConfigToolConnectMethod.PullVBScript: {
                        waitForm?.Invoke((Action)(() => waitForm.UpdateMessage($"Retrieving config files for user {DofSetup.UserName}...")));
                        GatherConfigFilesVBAsync();
                        break;
                    }

                    case EDofConfigToolConnectMethod.Offline:
                    default: {
                        break;
                    }
                }

                waitForm?.Invoke((Action)(() => waitForm.UpdateMessage($"Parse config files for user {DofSetup.UserName}...")));
                ParseConfigFiles();
                waitForm?.Invoke((Action)(() => waitForm.Close()));

            } catch (Exception ex) {
                MessageBox.Show($"DofConfigTool site returned an error while updating Dof config files.\n\nException Message :\n{ex.Message}"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task UpdateConfigFilesAsync()
        {
            if (DofSetup.APIKey.IsNullOrEmpty()) {
                MessageBox.Show("Cannot update local config files. APIKey was not set in the current DofConfigToolSetup.", "Update config files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (UserLocalPath.IsNullOrEmpty()) {
                MessageBox.Show("Cannot update local config files. Local extract path was not correctly set using UserName & APIKey from the DofConfigToolSetup.", "Update config files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await RetrieveDofConfigToolFilesAsync();
        }
    }
}
