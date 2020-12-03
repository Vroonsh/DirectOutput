using DirectOutput;
using DirectOutput.FX;
using DirectOutput.FX.MatrixFX;
using DirectOutput.LedControl.Loader;
using DirectOutput.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedControlToolkit
{
    public class EffectTreeNode : TreeNode, ITableElementTreeNode
    {
        public static readonly string TableElementTestName = "LedControlToolkit_EffectTreeNode_Test";

        public EffectTreeNode(TableElement tableElement, IEffect effect, LedControlConfig ledControl) : base()
        {
            Effect = effect;
            TableTE = tableElement;
            TestTE.AssignedEffects.Clear();
            TestTE.AssignedEffects.Add(new AssignedEffect("TestEffect") { Effect = Effect });
            ImageIndex = TestTE.GetTableElementData().Value > 0 ? 1 : 0;
            SelectedImageIndex = ImageIndex;
            TCS.FromEffect(Effect);
            TCS.ResolveColorConfig(ledControl.ColorConfigurations);
            TCS.OutputControl = (TableTE != null) ? OutputControlEnum.Controlled : OutputControlEnum.FixedOff;
            LCC = ledControl;
            UpdateFromTableElement(TableTE);
        }

        public void UpdateFromTableElement(TableElement TE)
        {
            if (TCS.OutputControl == OutputControlEnum.Controlled) {
                TCS.TableElement = (TE != null) ? $"{(char)TE.TableElementType}{((TE.TableElementType == DirectOutput.TableElementTypeEnum.NamedElement) ? TE.Name : TE.Number.ToString())}" : string.Empty;
                DofConfigCommand = TCS.ToConfigToolCommand(LCC.ColorConfigurations.GetCabinetColorList());
                Text = $"[{Effect.GetAssignedToy()?.Name}] {DofConfigCommand}";
            }
        }

        public void Rebuild(LedControlToolkitHandler Handler)
        {
            //Retrieve Toy 
            var Toy = Effect.GetAssignedToy();

            //rebuild Effect
            List<IEffect> allEffects = new List<IEffect>();
            Effect.GetAllEffects(allEffects);

            //Retrieve necessary data for the Configurator directly from the effect name
            var nameData = Effect.Name.Split(' ');
            int LedWizNumber = 0;
            int TCCNumber = 0;
            int SettingNumber = 0;
            for (var n = 0; n < nameData.Length - 1; ++n) {
                if (nameData[n].Equals("LedWiz", StringComparison.InvariantCultureIgnoreCase)) {
                    LedWizNumber = Int32.Parse(nameData[n + 1]);
                } else if (nameData[n].Equals("Column", StringComparison.InvariantCultureIgnoreCase)) {
                    TCCNumber = Int32.Parse(nameData[n + 1]);
                } else if (nameData[n].Equals("Setting", StringComparison.InvariantCultureIgnoreCase)) {
                    SettingNumber = Int32.Parse(nameData[n + 1]);
                }
            }

            //Remove all effects from Table & AssignedEffects before rebuilding
            foreach (var eff in allEffects) {
                Handler.Pinball.Table.Effects.Remove(eff);
                foreach (var TE in Handler.Pinball.Table.TableElements) {
                    TE.AssignedEffects.RemoveAll(AE => AE.Effect == eff);
                }
            }

            // The create effect will add the effects to the provided Table & TebleElements' assigned effects
            var newEffect = Handler.RebuildConfigurator.CreateEffect(TCS, TCCNumber, SettingNumber, Handler.Pinball.Table, Toy, LedWizNumber, Handler.Pinball.GlobalConfig.IniFilesPath, Handler.Pinball.Table.RomName);

            //Reorder Assigned effects as they should be from the ini file & resolve effect from effectname
            foreach (var TE in Handler.Pinball.Table.TableElements) {
                TE.AssignedEffects.Sort((E1, E2) => E1.EffectName.CompareTo(E2.EffectName));
                foreach (var assignEff in TE.AssignedEffects) {
                    assignEff.Init(Handler.Pinball.Table);
                }
            }

            //cascade call Init on all effects (ahad to init one after each other because of the TargetEffect resolution)
            var curEffect = newEffect;
            while (curEffect != null) {
                curEffect.Init(Handler.Pinball.Table);
                if (curEffect is EffectEffectBase effectWithTarget) {
                    curEffect = effectWithTarget.TargetEffect;
                } else {
                    curEffect = null;
                }
            }

            //Reassign new effect to the TreeNode
            TestTE.AssignedEffects.Clear();
            TestTE.AssignedEffects.Add(new AssignedEffect("TestEffect") { Effect = newEffect });
            Effect = newEffect;

            //Update Name
            DofConfigCommand = TCS.ToConfigToolCommand(LCC.ColorConfigurations.GetCabinetColorList());
            Text = $"[{Toy?.Name}] {DofConfigCommand}";
        }

        public virtual TableElement GetTableElement() => TestTE;

        public override string ToString() => DofConfigCommand;

        public IEffect Effect { get; set; }
        public TableConfigSetting TCS = new TableConfigSetting();
        public LedControlConfig LCC;
        public string DofConfigCommand;
        public TableElement TableTE = null;
        public TableElement TestTE = new TableElement(TableElementTestName, 0);
    }
}
