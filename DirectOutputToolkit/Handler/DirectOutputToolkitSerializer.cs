using DirectOutput;
using DirectOutput.Cab.Toys;
using DirectOutput.FX;
using DirectOutput.LedControl.Loader;
using DirectOutput.Table;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DirectOutputToolkit
{
    public class DirectOutputToolkitTableDescriptor
    {
        public class EffectDescriptor
        {
            public DofConfigToolOutputEnum ToyOutput { get; set; }
            public string TCS { get; set; }
        }

        public class TableElementDescriptor
        {
            public TableElementTypeEnum Type { get; set; }
            public string Name { get; set; }
            public int Number { get; set; }
            public List<EffectDescriptor> Effects = new List<EffectDescriptor>();
        }

        public string TableName { get; set; } = string.Empty;
        public string RomName { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public List<TableElementDescriptor> TableElements = new List<TableElementDescriptor>();
        public List<EffectDescriptor> StaticEffects = new List<EffectDescriptor>();

        public void FromTableNode(EditionTableTreeNode node, DirectOutputToolkitHandler Handler)
        {
            TableName = node.EditionTable.TableName;
            RomName = node.EditionTable.RomName;
            if (node.Image != null) {
                ImageName = node.Image.Tag as string;
            }
            foreach (var se in node.EditionTable.AssignedStaticEffects) {
                if (se.Effect != null) {
                    var neweffDesc = new EffectDescriptor();
                    var ToyName = se.Effect.GetAssignedToy()?.Name;
                    neweffDesc.ToyOutput = Handler.GetToyOutput(ToyName);
                    var TCS = Handler.TCSFromEffect(se.Effect);
                    TCS.OutputControl = TCS.Invert ? OutputControlEnum.FixedOff : OutputControlEnum.FixedOn;
                    neweffDesc.TCS = TCS.ToConfigToolCommand();
                    StaticEffects.Add(neweffDesc);
                }
            }

            foreach(var TE in node.EditionTable.TableElements) {
                if (TE.Name.StartsWith(EffectTreeNode.TableElementTestName, StringComparison.InvariantCultureIgnoreCase)) {
                    continue;
                }

                var newTE = new TableElementDescriptor() { Type = TE.TableElementType, Name = TE.Name, Number = TE.Number };

                foreach(var se in TE.AssignedEffects) {
                    if (se.Effect != null) {
                        var neweffDesc = new EffectDescriptor();
                        var ToyName = se.Effect.GetAssignedToy()?.Name;
                        neweffDesc.ToyOutput = Handler.GetToyOutput(ToyName);
                        var TCS = Handler.TCSFromEffect(se.Effect);
                        TCS.OutputControl = OutputControlEnum.Controlled;
                        TCS.TableElement = $"{(char)TE.TableElementType}{((TE.TableElementType == DirectOutput.TableElementTypeEnum.NamedElement) ? TE.Name : TE.Number.ToString())}";
                        neweffDesc.TCS = TCS.ToConfigToolCommand();
                        newTE.Effects.Add(neweffDesc);
                    }
                }

                TableElements.Add(newTE);
            }
        }
    }

    public class DirectOutputToolkitSerializer
    {
        public bool Serialize(EditionTableTreeNode TableNode, string FilePath, DirectOutputToolkitHandler Handler)
        {
            using (MemoryStream ms = new MemoryStream()) {
                var serializer = new XmlSerializer(typeof(DirectOutputToolkitTableDescriptor));
                var tableDesc = new DirectOutputToolkitTableDescriptor();
                tableDesc.FromTableNode(TableNode, Handler);
                serializer.Serialize(ms, tableDesc);
                ms.Position = 0;
                string Xml = string.Empty;
                using (StreamReader sr = new StreamReader(ms, Encoding.Default)) {
                    Xml = sr.ReadToEnd();
                    sr.Dispose();
                }
                File.WriteAllText(FilePath, Xml);
            }
            return true;
        }

        public bool Deserialize(EditionTableTreeNode TableNode, string FilePath, DirectOutputToolkitHandler Handler)
        {
            string Xml;
            try {
                Xml = DirectOutput.General.FileReader.ReadFileToString(FilePath);
            } catch (Exception E) {
                Log.Exception("Could not load LedControl Toolkit Table Descriptor from {0}.".Build(FilePath), E);
                throw new Exception("Could not read LedControl Toolkit Table Descriptor file {0}.".Build(FilePath), E);
            }

            byte[] xmlBytes = Encoding.Default.GetBytes(Xml);
            DirectOutputToolkitTableDescriptor tableDescriptor = null;
            using (MemoryStream ms = new MemoryStream(xmlBytes)) {
                try {
                    tableDescriptor = (DirectOutputToolkitTableDescriptor)new XmlSerializer(typeof(DirectOutputToolkitTableDescriptor)).Deserialize(ms);
                } catch (Exception E) {
                    Exception Ex = new Exception("Could not deserialize the LedControl Toolkit Table Descriptor from XML data.", E);
                    Ex.Data.Add("XML Data", Xml);
                    Log.Exception("Could not load LedControl Toolkit Table Descriptor from XML data.", Ex);
                    throw Ex;
                }
            }

            if (tableDescriptor != null) {
                TableNode.EditionTable.TableName = tableDescriptor.TableName;
                TableNode.EditionTable.RomName = tableDescriptor.RomName;
                if (tableDescriptor.ImageName != string.Empty) {
                    TableNode.Image = Image.FromFile(tableDescriptor.ImageName);
                    TableNode.Image.Tag = tableDescriptor.ImageName;
                    TableNode.OnImageChanged(TableNode.Image);
                }
                TableNode.Refresh();

                var TCCNumber = 0;

                foreach(var eff in tableDescriptor.StaticEffects) {
                    var SettingNumber = 0;

                    TableConfigSetting TCS = new TableConfigSetting();
                    TCS.ParseSettingData(eff.TCS);
                    TCS.ResolveColorConfigs(Handler.ColorConfigurations);

                    var Toys = Handler.GetToysFromOutput(eff.ToyOutput);

                    foreach (var Toy in Toys) {
                        var lastEffectsCount = TableNode.EditionTable.Effects.Count;
                        var newEffect = Handler.RebuildConfigurator.CreateEffect(TCS, TCCNumber, SettingNumber, TableNode.EditionTable
                                                                                , Toy
                                                                                , Handler.GetToyLedwizNum(Toy.Name)
                                                                                , Handler.DofFilesHandler.UserLocalPath, TableNode.EditionTable.RomName);
                        for (var num = lastEffectsCount; num < TableNode.EditionTable.Effects.Count; ++num) {
                            TableNode.EditionTable.Effects[num].Init(TableNode.EditionTable);
                        }
                        var newEffectNode = new EffectTreeNode(null, DirectOutputToolkitHandler.ETableType.EditionTable, newEffect, Handler);
                        TableNode.StaticEffectsNode.Nodes.Add(newEffectNode);
                        SettingNumber++;
                    }
                }

                TableNode.EditionTable.AssignedStaticEffects.Init(TableNode.EditionTable);
                TableNode.StaticEffectsNode.Rebuild(Handler);
                TCCNumber++;

                foreach (var te in tableDescriptor.TableElements) {
                    var newTENode = TableNode.Nodes.OfType<TableElementTreeNode>().Cast<TableElementTreeNode>().FirstOrDefault(N => N.TE.TableElementType == te.Type &&
                                                                                                (te.Type == TableElementTypeEnum.NamedElement ? N.TE.Name.Equals(te.Name, StringComparison.InvariantCultureIgnoreCase) : N.TE.Number == te.Number));
                    if (newTENode == null) {
                        var newTE = new TableElement() { TableElementType = te.Type, Name = te.Name, Number = te.Number };
                        newTE.AssignedEffects = new AssignedEffectList();
                        TableNode.EditionTable.TableElements.Add(newTE);
                        newTENode = new TableElementTreeNode(newTE, DirectOutputToolkitHandler.ETableType.EditionTable);
                        TableNode.Nodes.Add(newTENode);
                    }

                    var SettingNumber = 0;
                    foreach (var eff in te.Effects) {
                        TableConfigSetting TCS = new TableConfigSetting();
                        TCS.ParseSettingData(eff.TCS);
                        TCS.ResolveColorConfigs(Handler.ColorConfigurations);

                        var Toys = Handler.GetToysFromOutput(eff.ToyOutput);
                        foreach (var Toy in Toys) {
                            var lastEffectsCount = TableNode.EditionTable.Effects.Count;
                            var newEffect = Handler.RebuildConfigurator.CreateEffect(TCS, TCCNumber, SettingNumber, TableNode.EditionTable
                                                                                    , Toy
                                                                                    , Handler.GetToyLedwizNum(Toy.Name)
                                                                                    , Handler.DofFilesHandler.UserLocalPath, TableNode.EditionTable.RomName);
                            for (var num = lastEffectsCount; num < TableNode.EditionTable.Effects.Count; ++num) {
                                TableNode.EditionTable.Effects[num].Init(TableNode.EditionTable);
                            }
                            var newEffectNode = new EffectTreeNode(newTENode.TE, DirectOutputToolkitHandler.ETableType.EditionTable, newEffect, Handler);
                            SettingNumber++;
                        }
                    }

                    TCCNumber++;
                    newTENode.TE.AssignedEffects.Init(TableNode.EditionTable);
                    newTENode.Rebuild(Handler);
                }


                TableNode.Refresh();

                return true;
            }

            return false;
        }
    }
}
