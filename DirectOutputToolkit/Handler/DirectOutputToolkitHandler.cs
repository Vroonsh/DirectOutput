using DirectOutput;
using DirectOutput.Cab.Out;
using DirectOutput.Cab.Toys;
using DirectOutput.FX;
using DirectOutput.FX.AnalogToyFX;
using DirectOutput.FX.MatrixFX;
using DirectOutput.FX.RGBAFX;
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
            public List<TableElement> RunningTableElements = new List<TableElement>();
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

        internal TableConfigSetting TCSFromEffect(IEffect eff)
        {
            var TCS = new TableConfigSetting();
            TCS.FromEffect(eff);
            var toy = eff.GetAssignedToy();
            if (toy is IAnalogAlphaToy && TCS.MinDurationMs == Settings.EffectMinDurationMs) { 
                TCS.MinDurationMs = 0;
            } else if (toy is IRGBAToy && TCS.MinDurationMs == Settings.EffectRGBMinDurationMs) {
                TCS.MinDurationMs = 0;
            }
            return TCS;
        }

        internal string ToConfigToolCommand(TableConfigSetting TCS, IEffect eff, bool exportTE, bool fullRangeIntensity)
        {
            var dofCommand = TCS.ToConfigToolCommand(ColorConfigurations.GetCabinetColorList(), exportTE, fullRangeIntensity);

            var splitCommands = dofCommand.ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            splitCommands.Distinct();

            var variables = GetEffectVariableOverrides(eff).ToArray();
            if (variables.Length > 0) {
                foreach (var variable in variables) {
                    if (GlobalVariables.Keys.Contains(variable)) {
                        var varCommands = GlobalVariables[variable].ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        splitCommands = splitCommands.Except(varCommands).ToList();
                        splitCommands.Add($"@{variable}@");
                    }
                }
            }

            if (eff?.GetAssignedToy() is IAnalogAlphaToy) {
                splitCommands.Remove($"M{Settings.EffectMinDurationMs}");
            } else {
                splitCommands.Remove($"M{Settings.EffectRGBMinDurationMs}");
            }

            return string.Join(" ", splitCommands);
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

        public DofConfigToolFilesHandler.EDofConfigToolConnectMethod DofConfigToolConnectMethod { get; set; } = DofConfigToolFilesHandler.EDofConfigToolConnectMethod.PullVBScript;

        public bool ForceDofConfigToolUpdate { get; set; } = false;

        public LedControlConfigList LedControlConfigList => DofFilesHandler?.ConfigFiles;

        public ColorConfigList ColorConfigurations => LedControlConfigList.Count == 0 ? null : LedControlConfigList[0].ColorConfigurations;

        public VariablesDictionary GlobalVariables => LedControlConfigList.Count == 0 ? null : LedControlConfigList[0].GlobalVariables;

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
        internal async Task<bool> SetupPinballAsync()
        {
            FinishPinball();

            var dir = Path.GetDirectoryName(Settings.LastDofConfigSetup);
            DofFilesHandler.RootDirectory = dir;
            DofFilesHandler.DofSetup = DofConfigToolSetup;
            DofFilesHandler.ForceDofConfigToolUpdate = ForceDofConfigToolUpdate;
            DofFilesHandler.DofConfigToolConnectMethod = DofConfigToolConnectMethod;
            await DofFilesHandler.UpdateConfigFilesAsync();
            if (DofFilesHandler.ConfigFiles.Count == 0) {
                MessageBox.Show("DofSetup was not initialized correctly, DirectOutout Toolkit cannot start.\nExiting...", "DofSetup init failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            PopulateCategorizedVariables();

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
                    tdesc.Value.RunningTableElements.Clear();
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
            EffectVariableOverrides.Clear();
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
                if (tdesc.Value.RunningTableElements.Contains(TE) || tdesc.Value.RunningTableElements.Any(E => E.AssignedEffects.Any(AE => effects.Contains(AE.Effect)))) {
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
                TableDesc.RunningTableElements.RemoveAll(TE => TE == tableElement);
            } else {
                if (!TableDesc.RunningTableElements.Contains(tableElement)) {
                    TableDesc.RunningTableElements.Add(tableElement);
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
                TableDesc.RunningTableElements.RemoveAll(TE => TE == tableElement);
            } else {
                if (!TableDesc.RunningTableElements.Contains(tableElement)) {
                    TableDesc.RunningTableElements.Add(tableElement);
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
            var TCS = TCSFromEffect(refEffect);

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
                ClearEffectVariableOverrides(eff);
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
                    !PreviewController.IsDummyToy(T) &&
                    ((TCS.IsArea && (T is IMatrixToy<RGBAColor> || T is IMatrixToy<AnalogAlpha>)) ||
                    (!TCS.IsArea &&
                        ((TCS.OutputType == OutputTypeEnum.AnalogOutput && T is IAnalogAlphaToy) ||
                        (TCS.OutputType == OutputTypeEnum.RGBOutput && T is IRGBAToy))))
                ));

            return compatibleToys.ToArray();
        }

        #endregion

        #region Edition Table
        #endregion

        #region Variables
        private Dictionary<string, List<string>> CategorizedVariables = new Dictionary<string, List<string>>();

        static string[] MXCommands = {
            "AT","AL", "AW", "AH",
            "SHP",
            "APS", "APD", "APC",
            "ABT", "ABL", "ABW", "ABH", "ABF",
            "AAS", "AAC", "AAF", "AAD", "AAB",
            "AFDEN", "AFMIN", "AFMAX", "AFFADE",
            "ASA", "ASS", "ASD"
        };

        private void PopulateCategorizedVariables()
        {
            CategorizedVariables.Clear();
            CategorizedVariables["MX"] = new List<string>();
            CategorizedVariables["RGB"] = new List<string>();
            CategorizedVariables["ANALOG"] = new List<string>();
            CategorizedVariables["ANY"] = new List<string>();

            foreach (var variable in GlobalVariables) {
                var splitCommands = variable.Value.ToUpper().Split(' ').ToList();
                if (splitCommands.Any(C => MXCommands.Any(MXC => C.StartsWith(MXC)))) {
                    CategorizedVariables["MX"].Add(variable.Key);
                } else if (splitCommands.Any(C => C.StartsWith("#") || ColorConfigurations.Any(CC => CC.Name.Equals(C, StringComparison.InvariantCultureIgnoreCase)))) {
                    CategorizedVariables["RGB"].Add(variable.Key);
                } else if (splitCommands.Any(C => C.StartsWith("I"))) {
                    CategorizedVariables["ANALOG"].Add(variable.Key);
                } else {
                    CategorizedVariables["ANY"].Add(variable.Key);
                }
            }

        }

        internal List<string> GetCategorizedVariables(IEffect effect)
        {
            var effects = effect.GetAllEffects();
            if (effects.Any(E => E is IMatrixEffect)) {
                return CategorizedVariables["MX"];
            } else if (effects.Any(E => E is RGBAEffectBase)) {
                return CategorizedVariables["RGB"];
            } else if (effects.Any(E => E is AnalogToyEffectBase)){
                return CategorizedVariables["ANALOG"];
            }
            return CategorizedVariables["ANY"];
        }

        internal void OverrideVariables(TableConfigSetting tCS, IEffect eff)
        {
            List<string> variables = null;
            if (EffectVariableOverrides.TryGetValue(eff, out variables)) {
                foreach (var variable in variables) {
                    tCS.ParseCommands(GlobalVariables[variable].ToUpper());
                }
            }
        }

        private Dictionary<IEffect, List<string>> EffectVariableOverrides = new Dictionary<IEffect, List<string>>();

        public IEnumerable<string> GetEffectVariableOverrides(IEffect eff)
        {
            List<string> variables = null;
            if (EffectVariableOverrides.TryGetValue(eff, out variables)) {
                return variables;
            } else {
                return new string[0];
            }
        }

        public void AddEffectVariableOverride(IEffect eff, string variable)
        {
            if (!EffectVariableOverrides.Keys.Contains(eff)) {
                EffectVariableOverrides[eff] = new List<string>();
            }
            if (!EffectVariableOverrides[eff].Contains(variable)) {
                EffectVariableOverrides[eff].Add(variable);
            }
        }

        public void SetEffectVariableOverrides(IEffect eff, List<string> variables)
        {
            if (!EffectVariableOverrides.Keys.Contains(eff)) {
                EffectVariableOverrides[eff] = new List<string>();
            }
            EffectVariableOverrides[eff].Clear();
            EffectVariableOverrides[eff].AddRange(variables);
        }

        public void RemoveEffectVariableOverride(IEffect eff, string variable)
        {
            List<string> variables = null;
            if (EffectVariableOverrides.TryGetValue(eff, out variables)) {
                variables.RemoveAll(V => V.Equals(variable));
            }
        }

        internal void SwitchEffectVariableOverride(IEffect effect, string variable)
        {
            List<string> variables = null;
            if (EffectVariableOverrides.TryGetValue(effect, out variables)) {
                if (variables.Contains(variable)) {
                    variables.Remove(variable);
                } else {
                    variables.Add(variable);
                }
            } else {
                EffectVariableOverrides[effect] = new List<string>();
                EffectVariableOverrides[effect].Add(variable);
            }
        }

        internal void ClearEffectVariableOverrides(IEffect effect)
        {
            EffectVariableOverrides.Remove(effect);
        }
        #endregion
    }
}
