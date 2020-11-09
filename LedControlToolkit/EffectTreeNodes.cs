using DirectOutput.FX;
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
    public class EffectTreeNode : TreeNode
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
            TCS.OutputControl = (TableTE != null) ? OutputControlEnum.Controlled : OutputControlEnum.FixedOff;
            TCS.TableElement = (TableTE != null) ? $"{(char)TableTE.TableElementType}{((TableTE.TableElementType == DirectOutput.TableElementTypeEnum.NamedElement) ? TableTE.Name : TableTE.Number.ToString())}" : string.Empty;
            LCC = ledControl;
            DofConfigCommand = TCS.ToConfigToolCommand(LCC.ColorConfigurations.GetCabinetColorList());
            Text = DofConfigCommand;
        }

        public override string ToString() => DofConfigCommand;

        public IEffect Effect { get; set; }
        public TableConfigSetting TCS = new TableConfigSetting();
        public LedControlConfig LCC;
        public string DofConfigCommand;
        public TableElement TableTE = null;
        public TableElement TestTE = new TableElement(TableElementTestName, 0);
    }

    public class TableElementTreeNode : TreeNode
    {
        public TableElementTreeNode(TableElement tableElement, IEffect[] effects) : base()
        {
            Effects = effects;
            TE = tableElement;
            ImageIndex = TE.GetTableElementData().Value > 0 ? 1 : 0;
            SelectedImageIndex = ImageIndex;
            Text = ToString();
        }

        public override string ToString()
        {
            if (TE.TableElementType == DirectOutput.TableElementTypeEnum.NamedElement) {
                return $"{(char)TE.TableElementType}{TE.Name} : {Effects.Length} effects";
            }
            return $"{TE.TableElementType} {(char)TE.TableElementType}{TE.Number} : {Effects.Length} effects";
        }

        public IEffect[] Effects { get; set; }
        public TableElement TE = null;
    }
}
