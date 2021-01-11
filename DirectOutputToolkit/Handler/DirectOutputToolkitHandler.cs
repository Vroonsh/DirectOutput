using DirectOutput;
using DirectOutput.Cab.Toys;
using DirectOutput.FX;
using DirectOutput.LedControl.Loader;
using DirectOutput.LedControl.Setup;
using DirectOutput.Table;
using DirectOutputControls;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputToolkit
{
    public class DirectOutputToolkitHandler
    {
        private Settings Settings;

        public Pinball Pinball { get; private set; }

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

        internal string[] GetTableNames() => TableDescriptors.Select(TD => $"{TD.Value.Table.TableName}").ToArray();
        internal Table GetTable(ETableType tableType) => TableDescriptors[tableType].Table;
        internal Table GetTableByName(string text) => TableDescriptors.Select(TD => TD.Value.Table).FirstOrDefault(T => T.TableName.Equals(text, StringComparison.InvariantCultureIgnoreCase));

        private DofConfigToolFilesHandler DofFilesHandler = new DofConfigToolFilesHandler() { RootDirectory = "DofToolkit\\setups" };

        public LedControlConfigList LedControlConfigList { get; private set; } = new LedControlConfigList();
        public LedControlConfig LedControlConfigData { get; private set; } = new LedControlConfig();

        public Configurator RebuildConfigurator { get; private set; } = new Configurator();

        public DirectOutputToolkitHandler(Settings settings)
        {
            TableDescriptors[ETableType.ReferenceTable].Table = new Table();
            TableDescriptors[ETableType.EditionTable].Table = new Table();
            Settings = settings;
            RebuildConfigurator.EffectMinDurationMs = Settings.EffectMinDurationMs;
            RebuildConfigurator.EffectRGBMinDurationMs = Settings.EffectRGBMinDurationMs;
        }

        #region Pinball
        internal void SetupPinball()
        {
            FinishPinball();

            Pinball = new Pinball();
            Pinball.Setup();

            TableDescriptors[ETableType.ReferenceTable].Table.Init(Pinball);
            TableDescriptors[ETableType.EditionTable].Table.Init(Pinball);

            PreviewController.DofSetup = DofConfigToolSetup;
            PreviewController.DofViewSetup = DofViewSetup;
            PreviewController.Setup(Pinball);

            Pinball.Init();

            DofFilesHandler.DofSetup = DofConfigToolSetup;
            DofFilesHandler.UpdateConfigFiles();
        }

        internal void FinishPinball()
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
                foreach (var tdesc in TableDescriptors) {
                    foreach (var TE in tdesc.Value.Table.TableElements) {
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
                    ResetPinball();
                    break;
                }
            }
        }
        #endregion

        #region Effects 
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
        #endregion

        #region Edition Table
        #endregion

    }
}
