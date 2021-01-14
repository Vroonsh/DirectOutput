using DirectOutput;
using DirectOutput.FX;
using DirectOutput.FX.MatrixFX;
using DirectOutput.LedControl.Loader;
using DirectOutput.LedControl.Setup;
using DirectOutput.Table;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputToolkit
{
    public class EffectTreeNode : TreeNode, ITableElementTreeNode
    {
        public static readonly string TableElementTestName = "LCTK Effect Test";

        public EffectTreeNode(TableElement tableElement, DirectOutputToolkitHandler.ETableType TableType, IEffect effect, DirectOutputToolkitHandler handler) : base()
        {
            Effect = effect;
            _TableType = TableType;
            TableTE = tableElement;
            TestTE.Name = $"{TableElementTestName} [{Effect?.Name}]";
            TestTE.AssignedEffects.Clear();
            TestTE.AssignedEffects.Add(new AssignedEffect("TestEffect") { Effect = Effect });
            TestTE.ValueChanged += TestTE_ValueChanged;
            ImageIndex = TestTE.Value > 0 ? 1 : 0;
            SelectedImageIndex = ImageIndex;
            Handler = handler;
            TCS.FromEffect(Effect);
            TCS.ResolveColorConfigs(Handler.ColorConfigurations);
            TCS.OutputControl = (TableTE != null) ? OutputControlEnum.Controlled : OutputControlEnum.FixedOn;
            if (Effect != null) {
                UpdateFromTableElement(TableTE);
            }
        }

        ~EffectTreeNode()
        {
            TestTE.ValueChanged -= TestTE_ValueChanged;
        }

        private void TestTE_ValueChanged(object sender, TableElementValueChangedEventArgs e)
        {
            TreeView?.Invoke((Action)(() => this.Refresh()));
        }

        public void UpdateFromTableElement(TableElement TE)
        {
            if (TCS.OutputControl == OutputControlEnum.Controlled) {
                TCS.TableElement = (TE != null) ? $"{(char)TE.TableElementType}{((TE.TableElementType == DirectOutput.TableElementTypeEnum.NamedElement) ? TE.Name : TE.Number.ToString())}" : string.Empty;
            }
            DofConfigCommand = TCS.ToConfigToolCommand(Handler.ColorConfigurations.GetCabinetColorList());
            Text = $"[{Effect.GetAssignedToy()?.Name}] {DofConfigCommand}";
        }

        public void Rebuild(DirectOutputToolkitHandler Handler, IEffect SrcEffect)
        {
            var RefEffect = SrcEffect != null ? SrcEffect : Effect;

            //rebuild Effect
            List<IEffect> allEffects = new List<IEffect>();
            RefEffect.GetAllEffects(allEffects);

            //Retrieve necessary data for the Configurator directly from the effect name
            int LedWizNumber, TCCNumber, SettingNumber;
            string Suffix = string.Empty;
            Configurator.RetrieveEffectSettings(RefEffect.Name, out LedWizNumber, out TCCNumber, out SettingNumber, out Suffix);

            //Retrieve Toy & TCCNumber from Chosen Output
            //TODO
            var Toy = Effect.GetAssignedToy();


            //Remove all effects from Table & AssignedEffects before rebuilding
            Handler.RemoveEffects(allEffects, (Parent as TableElementTreeNode)?.TE, _TableType);

            // The create effect will add the effects to the provided Table & TebleElements' assigned effects
            var newEffect = Handler.CreateEffect(TCS, TCCNumber, SettingNumber, _TableType, Toy, LedWizNumber);

            //Reassign new effect to the TreeNode
            TestTE.Name = $"{TableElementTestName} [{newEffect.Name}]";
            TestTE.AssignedEffects.Clear();
            TestTE.AssignedEffects.Add(new AssignedEffect("TestEffect") { Effect = newEffect });
            Effect = newEffect;

            //Update Name
            DofConfigCommand = TCS.ToConfigToolCommand(Handler.ColorConfigurations.GetCabinetColorList());

            Refresh();
        }

        public void Refresh()
        {
            Text = $"[{Effect.GetAssignedToy()?.Name}] {DofConfigCommand}";
            ImageIndex = TestTE.Value > 0 ? 1 : 0;
        }

        public virtual TableElement GetTableElement() => TestTE;
        private DirectOutputToolkitHandler.ETableType _TableType = DirectOutputToolkitHandler.ETableType.EditionTable;
        public DirectOutputToolkitHandler.ETableType GetTableType() => _TableType;

        public override string ToString() => DofConfigCommand;

        public IEffect Effect { get; private set; }
        public TableConfigSetting TCS { get; private set; } = new TableConfigSetting();
        public DofConfigToolOutputEnum Output = DofConfigToolOutputEnum.Invalid;
        public DirectOutputToolkitHandler Handler { get; private set; }
        public string DofConfigCommand { get; private set; }
        public TableElement TableTE { get; private set; } = null;
        public TableElement TestTE { get; private set; } = new TableElement(TableElementTestName, 0);
    }
}
