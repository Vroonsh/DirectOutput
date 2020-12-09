using DirectOutput;
using DirectOutput.Cab.Toys.Hardware;
using DirectOutput.FX;
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

        public class TableDescriptor
        {
            public Table Table;
            public TableElement EffectTE;
        }

        public enum ETableType
        {
            EditionTable,
            ReferenceTable
        }

        public Dictionary<ETableType, TableDescriptor> TableDescriptors { get; } = new Dictionary<ETableType, TableDescriptor>() {
                {ETableType.EditionTable, new TableDescriptor()
                                            {
                                                Table = new Table() {TableName = "Edition Table", RomName = "romname"},
                                                EffectTE = null
                                            }
                },
                {ETableType.ReferenceTable, new TableDescriptor()
                                            {
                                                Table = null,
                                                EffectTE = null
                                            }
                }
            };

        public Table GetTable(ETableType type) => TableDescriptors[type].Table;
        public TableElement GetEffectTE(ETableType type) => TableDescriptors[type].EffectTE;

        public LedControlConfig LedControlConfigData { get; private set; }

        public Configurator RebuildConfigurator { get; private set; } = new Configurator();

        internal LedControlToolkitHandler(Settings settings, LedMatrixPreviewControl previewControl)
        {
            Settings = settings;
            PreviewControl = previewControl;
            ResetEditionTable();
        }

        internal void Start()
        {
        }

        internal void Finish()
        {
            if (Pinball != null) {
                TableDescriptors[ETableType.EditionTable].Table.Finish();
                Pinball.Table = TableDescriptors[ETableType.ReferenceTable].Table;
                Pinball.Finish();
            }
        }

        internal void ResetEditionTable()
        {
            Finish();
            TableDescriptors[ETableType.EditionTable].Table = new Table() { TableName = "Edition Table", RomName = "romname" };
            if (Pinball != null) {
                TableDescriptors[ETableType.ReferenceTable].Table.Init(Pinball);
                TableDescriptors[ETableType.EditionTable].Table.Init(Pinball);
                Pinball.Init();
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

            TableDescriptors[ETableType.ReferenceTable].Table = Pinball.Table;
            TableDescriptors[ETableType.EditionTable].Table.Init(Pinball);

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

        internal bool CanSwitchTableElement(TreeNode TreeNode) => (TreeNode is ITableElementTreeNode);

        internal int SwitchTableElement(TreeNode TreeNode)
        {
            var tableElementTreeNode = (TreeNode as ITableElementTreeNode);
            if (tableElementTreeNode == null) return 0;

            var Table = TableDescriptors[tableElementTreeNode.GetTableType()].Table;

            if (TreeNode is EffectTreeNode effectTreeNode) {
                //Remove any present Test TableElement
                Table.TableElements.RemoveAll(TE => TE.Name.Equals(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase));
                //Inject the one from the EffectNode
                Table.TableElements.Add(effectTreeNode.GetTableElement());
            }

            TableElementData D = tableElementTreeNode.GetTableElement().GetTableElementData();
            D.Value = D.Value > 0 ? 0 : 255;
            Pinball.Table = TableDescriptors[tableElementTreeNode.GetTableType()].Table;
            Pinball.ReceiveData(D);
            return D.Value;
        }

        internal LedStrip[] GetLedstrips()
        {
            return Pinball.Cabinet.Toys.Where(T => T is LedStrip).Select(T => T as LedStrip).ToArray();
        }
    }
}
