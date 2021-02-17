using DirectOutput;
using DirectOutput.FX;
using DirectOutput.FX.MatrixFX;
using DirectOutput.LedControl.Loader;
using DirectOutput.Table;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputToolkit
{
    public class EditionTableTreeNode : TreeNode
    {
        public EditionTableTreeNode(DirectOutputToolkitHandler handler, Table Table) : base()
        {
            Handler = handler;
            EditionTable = Table;
            Text = ToString();
        }

        public override string ToString()
        {
            return $"{EditionTable?.TableName} [{EditionTable?.RomName}] [{EditionTable?.AssignedStaticEffects.Count} static effects, {Nodes.OfType<TableElementTreeNode>().Count()} table elements]";
        }

        public DirectOutputToolkitHandler Handler { get; set; }
        public Table EditionTable { get; set; }
        public StaticEffectsTreeNode StaticEffectsNode { get; private set; }

        public Image Image { get; set; } = null;

        internal void Refresh()
        {
            Text = ToString();
        }

        internal void Rebuild(DirectOutputToolkitHandler Handler)
        {
            Nodes.Clear();

            StaticEffectsNode = new StaticEffectsTreeNode(EditionTable, DirectOutputToolkitHandler.ETableType.EditionTable);
            StaticEffectsNode.Rebuild(Handler);
            Nodes.Add(StaticEffectsNode);

            foreach (var te in EditionTable.TableElements) {
                if (!te.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase)) {
                    var teNode = new TableElementTreeNode(te, DirectOutputToolkitHandler.ETableType.EditionTable);
                    teNode.Rebuild(Handler);
                    Nodes.Add(teNode);
                }
            }
            Refresh();
        }

        private bool FilesAreEquals(string srcFile, string destFile)
        {
            if (!File.Exists(destFile)) {
                return false;
            }

            if (!File.ReadAllBytes(srcFile).SequenceEqual(File.ReadAllBytes(destFile))) {
                return false;
            }

            return true;
        }

        internal void OnImageChanged(Image image)
        {
            if (image == null) {
                Image = image;
            } else {
                var forceWrite = (Image == null || !(Image.Tag as string).Equals(image.Tag as string, StringComparison.InvariantCultureIgnoreCase));
                if (Image != image) {
                    Image?.Dispose();
                    Image = image;
                }
                var srcFile = Image.Tag as string;
                var ext = Path.GetExtension(srcFile);
                var destFile = Path.Combine(Handler.DofFilesHandler.UserLocalPath, $"{EditionTable.RomName}{ext}");
                if (forceWrite || !FilesAreEquals(srcFile, destFile)) { 
                    var dir = new DirectoryInfo(Handler.DofFilesHandler.UserLocalPath);
                    foreach (var file in dir.EnumerateFiles($"{EditionTable.RomName}.*")) {
                        file.Delete();
                    }
                    Image.Save(destFile);
                    Handler.ReinitEditionTable();
                }
            }
        }
    }
}
