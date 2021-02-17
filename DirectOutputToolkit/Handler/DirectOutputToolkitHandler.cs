using DirectOutput;
using DirectOutput.Cab.Out;
using DirectOutput.Cab.Toys;
using DirectOutput.FX;
using DirectOutput.General;
using DirectOutput.General.Analog;
using DirectOutput.General.Color;
using DirectOutput.GlobalConfiguration;
using DirectOutput.LedControl.Loader;
using DirectOutput.LedControl.Setup;
using DirectOutput.Table;
using DirectOutputControls;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputToolkit
{
    public class DirectOutputToolkitHandler
    {
        public Settings Settings { get; private set; }

        private Pinball Pinball { get; set; }

        public DofConfigToolSetup DofConfigToolSetup { get; set; } = null;
        public DirectOutputViewSetup DofViewSetup { get; set; } = null;

        public DirectOutputPreviewControl PreviewControl { get; set; } = null;
        private DirectOutputPreviewController PreviewController = new DirectOutputPreviewController();

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

        internal IEnumerable<Image> GetBitmapList(TableConfigSetting wrappedTCS)
        {
            FilePattern BitmapFilePattern = new FilePattern($"{DofFilesHandler.UserLocalPath}\\{TableDescriptors[ETableType.EditionTable].Table.RomName}.*");
            var Files = BitmapFilePattern.GetMatchingFiles();
            List<Image> images = new List<Image>();
            foreach(var file in Files) {
                var image = Image.FromFile(file.FullName);
                image.Tag = file.Name;
                images.Add(image);
            }
            return images;
        }

        internal DofConfigToolOutputEnum GetToyOutput(string ToyName)
        {
            var remap = PreviewController.GetToyOutputRemaps(M => M.Toy.Name.Equals(ToyName, StringComparison.InvariantCultureIgnoreCase));
            return (remap != null && remap.Length > 0 ? remap.First().DofOutput : DofConfigToolOutputEnum.Invalid);
        }

        internal IToy[] GetToysFromOutput(DofConfigToolOutputEnum ToyOutput)
        {
            var toys = PreviewController.GetToyOutputRemaps(M => M.DofOutput == ToyOutput).Select(M => M.Toy).ToList();
            return toys.ToArray();
        }

        internal int GetToyLedwizNum(string ToyName)
        {
            var remap = PreviewController.GetToyOutputRemaps(M => M.Toy.Name.Equals(ToyName, StringComparison.InvariantCultureIgnoreCase));
            return (remap != null && remap.Length > 0 ? remap.First().LedWizNum : 0);
        }

        internal void ReinitEditionTable()
        {
            var table = TableDescriptors[ETableType.EditionTable].Table;
            table.Bitmaps.Clear();
            foreach(var eff in table.Effects) {
                eff.Init(table);
            }
        }

        internal string[] GetTableNames() => TableDescriptors.Select(TD => $"{TD.Value.Table.TableName}").ToArray();
        internal Table GetTable(ETableType tableType) => TableDescriptors[tableType].Table;
        internal Table GetTableByName(string text) => TableDescriptors.Select(TD => TD.Value.Table).FirstOrDefault(T => T.TableName.Equals(text, StringComparison.InvariantCultureIgnoreCase));

        public DofConfigToolFilesHandler DofFilesHandler { get; private set; } = new DofConfigToolFilesHandler() { };
        public bool ForceDofConfigToolUpdate { get; set; } = false;

        public LedControlConfigList LedControlConfigList => DofFilesHandler?.ConfigFiles;

        public ColorConfigList ColorConfigurations => LedControlConfigList.Count == 0 ? null : LedControlConfigList[0].ColorConfigurations;

        public string[] ShapeNames => TableDescriptors[ETableType.ReferenceTable].Table.ShapeDefinitions.Shapes.Select(S => S.Name).ToArray();

        public ToyList Toys => Pinball?.Cabinet.Toys;

        public Configurator RebuildConfigurator { get; private set; } = new Configurator();

        public DirectOutputToolkitHandler(Settings settings)
        {
            TableDescriptors[ETableType.ReferenceTable].Table = new Table();
            TableDescriptors[ETableType.EditionTable].Table = new Table();
            Settings = settings;
            RebuildConfigurator.EffectMinDurationMs = Settings.EffectMinDurationMs;
            RebuildConfigurator.EffectRGBMinDurationMs = Settings.EffectRGBMinDurationMs;
            ResetEditionTable(null);
        }

        #region Pinball
        internal bool SetupPinball()
        {
            FinishPinball();

            var dir = Path.GetDirectoryName(Settings.LastDofConfigSetup);
            DofFilesHandler.RootDirectory = dir;
            DofFilesHandler.DofSetup = DofConfigToolSetup;
            DofFilesHandler.UpdateConfigFiles(ForceDofConfigToolUpdate);
            if (DofFilesHandler.ConfigFiles.Count == 0) {
                MessageBox.Show("DofSetup was not initialized correctly, DirectOutout Toolkit cannot start.\nExiting...", "DofSetup init failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            Pinball = new Pinball();
            var GlobalConfig = new GlobalConfig(){
                IniFilesPath = DofFilesHandler.UserLocalPath,
                EnableLogging = true,
                ClearLogOnSessionStart = true,
                LedWizDefaultMinCommandIntervalMs = 10
            };
            var fileName = Path.Combine(DofFilesHandler.UserLocalPath, "GlobalConfig.xml");
            GlobalConfig.SaveGlobalConfig(fileName, false);
            Pinball.Setup(GlobalConfigFilename: fileName);

            TableDescriptors[ETableType.ReferenceTable].Table.Init(Pinball);
            TableDescriptors[ETableType.EditionTable].Table.Init(Pinball);

            PreviewController.DofSetup = DofConfigToolSetup;
            PreviewController.DofViewSetup = DofViewSetup;
            PreviewController.Refresh += PreviewControl.OnControllerRefresh;
            PreviewController.Setup(Pinball);

            Pinball.Init();
            GC.Collect(6, GCCollectionMode.Forced);

            return true;
        }

        internal void FinishPinball()
        {
            if (Pinball != null) {
                TableDescriptors[ETableType.EditionTable].Table.Finish();
                Pinball.Table = TableDescriptors[ETableType.ReferenceTable].Table;
                Pinball.Finish();
            }
        }

        internal void ResetPinball(bool ReInit)
        {
            if (Pinball != null) {
                foreach (var tdesc in TableDescriptors) {
                    foreach (var TE in tdesc.Value.Table.TableElements) {
                        TE.Value = 0;
                    }
                    if (ReInit) {
                        tdesc.Value.Table.Finish();
                        tdesc.Value.Table.TableElements.RemoveAll(TE => TE.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase));
                    }
                    tdesc.Value.RunnigTableElements.Clear();
                }
                if (ReInit) {
                    Pinball.Finish();
                    Pinball.Init();
                }
                foreach (var tdesc in TableDescriptors) {
                    tdesc.Value.Table.Init(Pinball);
                }
            }
            GC.Collect(6, GCCollectionMode.Forced);
        }
        #endregion

        #region Tables
        internal void ResetEditionTable(string RefRomName)
        {
            ResetPinball(false);
            TableDescriptors[ETableType.EditionTable].Table = new Table() { TableName = RefRomName.IsNullOrEmpty() ? "Edition Table" : $"Table copied from {RefRomName}", RomName = RefRomName.IsNullOrEmpty() ? "romname" : RefRomName };
            if (Pinball != null) {
                Pinball.Init();
                TableDescriptors[ETableType.ReferenceTable].Table.Init(Pinball);
                TableDescriptors[ETableType.EditionTable].Table.TableElements.RemoveAll(TE => TE.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase));
                TableDescriptors[ETableType.EditionTable].Table.Init(Pinball);
            }
            GC.Collect(6, GCCollectionMode.Forced);
        }

        internal void SetupTable(ETableType TableType, string RomName)
        {
            ResetPinball(false);
            TableDescriptors[TableType].Table = null;
            TableDescriptors[TableType].Table = new Table();
            var table = TableDescriptors[TableType].Table;
            RebuildConfigurator.Setup(LedControlConfigList, table, Pinball.Cabinet, RomName);
            table.TableElements.Sort((TE1, TE2) => (TE1.TableElementType == TE2.TableElementType ? TE1.Number.CompareTo(TE2.Number) : TE1.TableElementType.CompareTo(TE2.TableElementType)));
            table.Init(Pinball);
        }

        internal void LaunchTable(ETableType TableType)
        {
            TableDescriptors[TableType].Table.TriggerStaticEffects();
            Pinball.Cabinet.Update();
        }
        #endregion

        #region Table Elements
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
            foreach (var tdesc in TableDescriptors) {
                if (tdesc.Value.RunnigTableElements.Contains(TE) || tdesc.Value.RunnigTableElements.Any(E => E.AssignedEffects.Any(AE => effects.Contains(AE.Effect)))) {
                    ResetPinball(false);
                    break;
                }
            }
        }

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
            D.Value = TEData.Value > 0 ? 0 : tableElementTreeNode.HasNoBoolEffects() ? 255 : 1;
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

        internal int SetTableElementValue(TreeNode TreeNode, int value)
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
            D.Value = value == 1 ? (tableElementTreeNode.HasNoBoolEffects() ? 255 : 1) : value;
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

        internal void TriggerStaticEffects(ETableType tableType)
        {
            Pinball.Table = GetTable(tableType);
            Pinball.Table.TriggerStaticEffects();
            Pinball.Cabinet.Update();
        }

        #endregion

        #region Effects 
        internal IEffect CreateEffect(IEffect refEffect, TableElement TE, ETableType tableType)
        {
            TableConfigSetting TCS = new TableConfigSetting();
            TCS.FromEffect(refEffect);

            if (TE != null) {
                TCS.OutputControl = OutputControlEnum.Controlled;
                TCS.TableElement = $"{(char)TE.TableElementType}{((TE.TableElementType == DirectOutput.TableElementTypeEnum.NamedElement) ? TE.Name : TE.Number.ToString())}";
            } else {
                TCS.OutputControl = TCS.Invert ? OutputControlEnum.FixedOff : OutputControlEnum.FixedOn;
            }

            //Retrieve necessary data for the Configurator directly from the effect name
            int LedWizNumber, TCCNumber, SettingNumber;
            string Suffix = string.Empty;
            Configurator.RetrieveEffectSettings(refEffect.Name, out LedWizNumber, out TCCNumber, out SettingNumber, out Suffix);

            //Retrieve Toy & TCCNumber from Chosen Output
            var Toy = refEffect.GetAssignedToy();

            // The create effect will add the effects to the provided Table & TebleElements' assigned effects
            return CreateEffect(TCS, TCCNumber, SettingNumber, DirectOutputToolkitHandler.ETableType.EditionTable, Toy, LedWizNumber);
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

        internal void InitEffect(AssignedEffect eff, ETableType tableType)
        {
            eff.Init(TableDescriptors[tableType].Table);
        }

        internal void RemoveEffects(IEffect[] effects, TableElement parentTE, ETableType tableType)
        {
            ResetRunnigTableElement(parentTE);
            var Table = TableDescriptors[tableType].Table;
            foreach (var eff in effects) {
                Table.Effects.RemoveAll(E=> E == eff);
                Table.AssignedStaticEffects.RemoveAll(AE => AE.Effect == eff);
                foreach (var TE in Table.TableElements) {
                    if (TE.AssignedEffects.RemoveAll(AE => AE.Effect == eff) > 0) {
                        ResetRunnigTableElement(TE);
                    }
                }
            }
        }

        internal IEnumerable<IToy> GetCompatibleToys(TableConfigSetting TCS)
        {
            if (TCS == null) return new IToy[0];

            List<IToy> compatibleToys = new List<IToy>();

            compatibleToys.AddRange(Toys.Where(T=>
                    (TCS.IsArea && (T is IMatrixToy<RGBAColor> || T is IMatrixToy<AnalogAlpha>)) ||
                    (!TCS.IsArea &&
                        ((TCS.OutputType == OutputTypeEnum.AnalogOutput && T is IAnalogAlphaToy) ||
                        (TCS.OutputType == OutputTypeEnum.RGBOutput && T is IRGBAToy)))
                ));

            return compatibleToys.ToArray();
        }

        #endregion

        #region Edition Table
        #endregion

    }
}
