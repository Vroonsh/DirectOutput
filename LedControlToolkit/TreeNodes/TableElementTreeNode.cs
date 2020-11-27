﻿using DirectOutput;
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
    public class TableElementTreeNode : TreeNode, ITableElementTreeNode
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

        public virtual TableElement GetTableElement() => TE;

        public IEffect[] Effects { get; set; }
        public TableElement TE = null;
    }
}
