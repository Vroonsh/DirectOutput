using DirectOutput;
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

        public LedControlConfig LedControlConfigData { get; private set; }

        public Configurator RebuildConfigurator { get; private set; } = new Configurator();

        public DirectOutputToolkitHandler()
        {
            TableDescriptors[ETableType.ReferenceTable].Table = new Table();
            TableDescriptors[ETableType.EditionTable].Table = new Table();
        }

        internal void FinishPinball()
        {
            if (Pinball != null) {
                TableDescriptors[ETableType.EditionTable].Table.Finish();
                Pinball.Table = TableDescriptors[ETableType.ReferenceTable].Table;
                Pinball.Finish();
            }
        }

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
        }

    }
}
