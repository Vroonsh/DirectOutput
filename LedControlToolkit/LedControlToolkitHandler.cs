using DirectOutput;
using DirectOutput.Cab.Toys;
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
            public List<TableElement> RunnigTableElements = new List<TableElement>();
        }

        public enum ETableType
        {
            EditionTable,
            ReferenceTable
        }

        protected Dictionary<ETableType, TableDescriptor> TableDescriptors { get; } = new Dictionary<ETableType, TableDescriptor>() {
                {ETableType.EditionTable, new TableDescriptor()
                                            {
                                                Table = new Table() {TableName = "Edition Table", RomName = "romname"},
                                            }
                },
                {ETableType.ReferenceTable, new TableDescriptor()
                                            {
                                                Table = null,
                                            }
                }
            };

        internal string[] GetTableNames() => TableDescriptors.Select(TD => $"{TD.Value.Table.TableName}").ToArray();
        internal Table GetTable(ETableType tableType) => TableDescriptors[tableType].Table;
        internal Table GetTableByName(string text) => TableDescriptors.Select(TD => TD.Value.Table).FirstOrDefault(T => T.TableName.Equals(text, StringComparison.InvariantCultureIgnoreCase));

        public LedControlConfig LedControlConfigData { get; private set; }

        public Configurator RebuildConfigurator { get; private set; } = new Configurator();

        internal LedControlToolkitHandler(Settings settings, LedMatrixPreviewControl previewControl)
        {
            Settings = settings;
            RebuildConfigurator.EffectMinDurationMs = Settings.EffectMinDurationMs;
            RebuildConfigurator.EffectRGBMinDurationMs = Settings.EffectRGBMinDurationMs;
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

        internal void ResetPinball()
        {
            if (Pinball != null) {
                foreach(var tdesc in TableDescriptors) {
                    foreach( var TE in tdesc.Value.Table.TableElements) {
                        TE.Value = 0;
                    }
                    tdesc.Value.Table.Finish();
                    tdesc.Value.Table.TableElements.RemoveAll(TE => TE.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase));
                    tdesc.Value.RunnigTableElements.Clear();
                }
                Pinball.Finish();
                foreach (var tdesc in TableDescriptors) {
                    tdesc.Value.Table.Init(Pinball);
                }
                Pinball.Init();
            }
        }

        internal void ResetEditionTable()
        {
            Finish();
            TableDescriptors[ETableType.EditionTable].Table = new Table() { TableName = "Edition Table", RomName = "romname" };
            if (Pinball != null) {
                TableDescriptors[ETableType.ReferenceTable].Table.Init(Pinball);
                TableDescriptors[ETableType.EditionTable].Table.TableElements.RemoveAll(TE => TE.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase));
                TableDescriptors[ETableType.EditionTable].Table.Init(Pinball);
                Pinball.Init();
            }
        }

        internal IEffect CreateEffect(TableConfigSetting tCS, int tCCNumber, int settingNumber, ETableType tableType, IToy toy, int ledWizNumber)
        {
            var Table = TableDescriptors[tableType].Table;
            var newEffect = RebuildConfigurator.CreateEffect(tCS, tCCNumber, settingNumber, Table, toy, ledWizNumber, Pinball.GlobalConfig.IniFilesPath, Table.RomName);

            //Reorder Assigned effects as they should be from the ini file & resolve effect from effectname
            foreach (var TE in Table.TableElements) {
                TE.AssignedEffects.Sort((E1, E2) => E1.EffectName.CompareTo(E2.EffectName));
                foreach (var assignEff in TE.AssignedEffects) {
                    assignEff.Init(Table);
                }
            }

            //cascade call Init on all effects (ahad to init one after each other because of the TargetEffect resolution)
            var curEffect = newEffect;
            while (curEffect != null) {
                curEffect.Init(Table);
                if (curEffect is EffectEffectBase effectWithTarget) {
                    curEffect = effectWithTarget.TargetEffect;
                } else {
                    curEffect = null;
                }
            }

            return newEffect;
        }

        internal void RemoveEffects(List<IEffect> allEffects, TableElement parentTE, ETableType tableType)
        {
            ResetRunnigTableElement(parentTE);
            var Table = TableDescriptors[tableType].Table;
            foreach (var eff in allEffects) {
                Table.Effects.Remove(eff);
                foreach (var TE in Table.TableElements) {
                    if (TE.AssignedEffects.RemoveAll(AE => AE.Effect == eff) > 0) {
                        ResetRunnigTableElement(TE);
                    }
                }
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

        internal void AddTableElement(TableElement TE, ETableType tableType)
        {
            TableDescriptors[tableType].Table.TableElements.Add(TE);
        }

        internal void RemoveTableElement(TableElement tableElement, ETableType tableType)
        {
            var Table = GetTable(tableType);
            ResetRunnigTableElement(tableElement);
            Table.TableElements.RemoveAll(TE => TE == tableElement);
        }

        internal void ResetRunnigTableElement(TableElement TE)
        {
            if (TE == null) return;
            var effects = TE.AssignedEffects.Select(AE => AE.Effect).ToArray();
            foreach(var tdesc in TableDescriptors) {
                if (tdesc.Value.RunnigTableElements.Contains(TE) || tdesc.Value.RunnigTableElements.Any(E=>E.AssignedEffects.Any(AE=>effects.Contains(AE.Effect)))) {
                    ResetPinball();
                    break;
                }
            }
        }

        internal string[] GetShapeNames() => TableDescriptors[ETableType.ReferenceTable].Table.ShapeDefinitions.Shapes.Select(S => S.Name).ToArray();

        internal void InitEffect(AssignedEffect eff, ETableType tableType)
        {
            eff.Init(TableDescriptors[tableType].Table);
        }

        internal bool CanSwitchTableElement(TreeNode TreeNode) => (TreeNode is ITableElementTreeNode);

        internal int SwitchTableElement(TreeNode TreeNode)
        {
            var tableElementTreeNode = (TreeNode as ITableElementTreeNode);
            if (tableElementTreeNode == null) return 0;

            var TableDesc = TableDescriptors[tableElementTreeNode.GetTableType()];
            var Table = TableDesc.Table;
            var tableElement = tableElementTreeNode.GetTableElement();
            var TEData = tableElement.GetTableElementData();

            if (TreeNode is EffectTreeNode effectTreeNode) {
                //Inject the one from the EffectNode
                if (!Table.TableElements.Contains(effectTreeNode.GetTableElement())) {
                    Table.TableElements.Add(effectTreeNode.GetTableElement());
                }
            }

            TableElementData D = tableElement.GetTableElementData();
            D.Value = TEData.Value > 0 ? 0 : 255;
            if (D.Value == 0) {
                TableDesc.RunnigTableElements.RemoveAll(TE => TE == tableElement);
            } else {
                if (!TableDesc.RunnigTableElements.Contains(tableElement)) {
                    TableDesc.RunnigTableElements.Add(tableElement);
                }
            }
            Pinball.Table = Table;
            Pinball.ReceiveData(D);
            return D.Value;
        }

        internal LedStrip[] GetLedstrips()
        {
            return Pinball.Cabinet.Toys.Where(T => T is LedStrip).Select(T => T as LedStrip).ToArray();
        }

    }
}
