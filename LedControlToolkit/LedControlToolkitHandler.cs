using DirectOutput;
using DirectOutput.Cab.Toys.Hardware;
using DirectOutput.GlobalConfiguration;
using DirectOutput.LedControl.Loader;
using DirectOutput.LedControl.Setup;
using DirectOutput.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedControlToolkit
{
    public class LedControlToolkitHandler
    {
        private Settings Settings;
        private LedMatrixPreviewControl PreviewControl;

        public Pinball Pinball { get; private set; }
        private Table PinballTable = null;
        public Table EditionTable { get; private set; } = new Table() { TableName = "Edition Table", RomName = "romname" };
        public LedControlConfig LedControlConfigData { get; private set; }

        public Configurator RebuildConfigurator { get; private set; } = new Configurator();

        private TableElement CurrentTableElement = null;

        internal LedControlToolkitHandler(Settings settings, LedMatrixPreviewControl previewControl)
        {
            Settings = settings;
            PreviewControl = previewControl;
        }

        internal void Start()
        {
        }

        internal void Finish()
        {
            if (Pinball != null) {
                Pinball.Finish();
            }
        }

        internal void SetupPinball()
        {
            Finish();

            LedControlToolkitControllerAutoConfigurator.LastLedControlIniFilename = Settings.LastLedControlIniFilename;

            Pinball = new Pinball();
            Pinball.Setup(GlobalConfigFilename: Settings.LastGlobalConfigFilename, RomName: Settings.LastRomName);

            var controllers = Pinball.Cabinet.OutputControllers.Where(c => c is LedControlToolkitController).ToArray();
            if (controllers.Length > 0) {
                (controllers[0] as LedControlToolkitController).OutputControl = PreviewControl;
            } else {
                var ledControlFilename = Path.GetFileNameWithoutExtension(Settings.LastLedControlIniFilename);
                var ledWizNumber = 30;
                if (ledControlFilename.Contains("directoutputconfig")) {
                    ledWizNumber = Int32.Parse(ledControlFilename.Replace("directoutputconfig", ""));
                }
                var previewController = new LedControlToolkitController() { Name = "LedControlToolkitController", LedWizNumber = ledWizNumber };
                previewController.Init(Pinball.Cabinet);
                Pinball.Cabinet.OutputControllers.Add(previewController);
            }

            PinballTable = Pinball.Table;

            EditionTable.Init(Pinball);

            PreviewControl.SetupPreviewParts(Pinball.Cabinet, Settings);

            Pinball.Init();
        }

        internal void LoadLedControlConfigData()
        {
            FileInfo GlobalConfigFile = new FileInfo(Settings.LastGlobalConfigFilename);
            var GlobalConfig = DirectOutput.GlobalConfiguration.GlobalConfig.GetGlobalConfigFromConfigXmlFile(GlobalConfigFile.FullName);
            if (GlobalConfig == null) {
                Log.Write("No global config file loaded");
                //set new global config object if it config could not be loaded from the file.
                GlobalConfig = new GlobalConfig();
            }
            GlobalConfig.GlobalConfigFilename = GlobalConfigFile.FullName;
            Dictionary<int, FileInfo> LedControlIniFiles = GlobalConfig.GetIniFilesDictionary();
            LedControlConfigList L = new LedControlConfigList();

            LedControlIniFiles = LedControlIniFiles.Where(INI => INI.Value.FullName == Settings.LastLedControlIniFilename).ToDictionary(INI => INI.Key, INI => INI.Value);

            LedControlConfigData = null;
            if (LedControlIniFiles.Count > 0) {
                L.LoadLedControlFiles(LedControlIniFiles, false);
                Log.Write("{0} directoutputconfig.ini or ledcontrol.ini files loaded.".Build(LedControlIniFiles.Count));
                LedControlConfigData = L[0];
            } else {
                Log.Write("No directoutputconfig.ini or ledcontrol.ini files found.");
            }
        }

        internal void SetCurrentTableElement(TreeNode TreeNode, bool isEditionTable)
        {
            CurrentTableElement = null;
            Pinball.Table = isEditionTable ? EditionTable : PinballTable;
            if (TreeNode is ITableElementTreeNode tableElementTreeNode) {
                CurrentTableElement = tableElementTreeNode.GetTableElement();

                //Remove old TableElementEffectTest from the Table if there is one
                var te = Pinball.Table.TableElements.FirstOrDefault(T => T.Name.Equals(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase));
                if (te != null) {
                    Pinball.Table.TableElements.Remove(te);
                }

                if (TreeNode is EffectTreeNode effectNode) {
                    //Put the new table element into the current table
                    if (CurrentTableElement != null) {
                        Pinball.Table.TableElements.Add(CurrentTableElement);
                    }
                }
            }
        }

        internal int SwitchTableElement(TreeNode TreeNode)
        {
            if (TreeNode is ITableElementTreeNode tableElementTreeNode) {
                if (CurrentTableElement == tableElementTreeNode.GetTableElement()) {
                    TableElementData D = CurrentTableElement.GetTableElementData();
                    D.Value = D.Value > 0 ? 0 : 255;
                    Pinball.ReceiveData(D);
                    return D.Value;
                }
            }

            return 0;
        }

        internal int GetCurrentTableElementValue()
        {
            if (CurrentTableElement != null) {
                return CurrentTableElement.GetTableElementData().Value;
            }
            return 0;
        }

        internal LedStrip[] GetLedstrips()
        {
            return Pinball.Cabinet.Toys.Where(T => T is LedStrip).Select(T => T as LedStrip).ToArray();
        }
    }
}
