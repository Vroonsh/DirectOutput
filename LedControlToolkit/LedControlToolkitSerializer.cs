﻿using DirectOutput;
using DirectOutput.FX;
using DirectOutput.LedControl.Loader;
using DirectOutput.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LedControlToolkit
{
    public class LedControlToolkitTableDescriptor
    {
        public class EffectDescriptor
        {
            public string ToyOutput { get; set; }
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
        public List<TableElementDescriptor> TableElements = new List<TableElementDescriptor>();

        public void FromTableNode(EditionTableTreeNode node, Dictionary<string, string> ToyOutputMappings)
        {
            TableName = node.EditionTable.TableName;
            RomName = node.EditionTable.RomName;
            foreach (var TENode in node.Nodes) {
                var TE = (TENode as TableElementTreeNode).TE;
                var newTE = new TableElementDescriptor() { Type = TE.TableElementType, Name = TE.Name, Number = TE.Number };

                foreach(var effNode in (TENode as TreeNode).Nodes) {
                    var effect = (effNode as EffectTreeNode).Effect;
                    var ToyName = effect.GetAssignedToy()?.Name;

                    var neweffDesc = new EffectDescriptor();
                    neweffDesc.ToyOutput = ToyOutputMappings.FirstOrDefault(kv => kv.Key.Equals(ToyName, StringComparison.InvariantCultureIgnoreCase)).Value;
                    neweffDesc.TCS = (effNode as EffectTreeNode).TCS.ToConfigToolCommand();

                    newTE.Effects.Add(neweffDesc);
                }

                TableElements.Add(newTE);
            }
        }
    }

    public class LedControlToolkitSerializer
    {
        public bool Serialize(EditionTableTreeNode TableNode, string FilePath, Dictionary<string, string> ToyOutputMappings)
        {
            using (MemoryStream ms = new MemoryStream()) {
                var serializer = new XmlSerializer(typeof(LedControlToolkitTableDescriptor));
                var tableDesc = new LedControlToolkitTableDescriptor();
                tableDesc.FromTableNode(TableNode, ToyOutputMappings);
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

        public bool Deserialize(EditionTableTreeNode TableNode, string FilePath, LedControlToolkitHandler Handler, Dictionary<string, string> ToyOutputMappings)
        {
            string Xml;
            try {
                Xml = DirectOutput.General.FileReader.ReadFileToString(FilePath);
            } catch (Exception E) {
                Log.Exception("Could not load LedControl Toolkit Table Descriptor from {0}.".Build(FilePath), E);
                throw new Exception("Could not read LedControl Toolkit Table Descriptor file {0}.".Build(FilePath), E);
            }

            byte[] xmlBytes = Encoding.Default.GetBytes(Xml);
            LedControlToolkitTableDescriptor tableDescriptor = null;
            using (MemoryStream ms = new MemoryStream(xmlBytes)) {
                try {
                    tableDescriptor = (LedControlToolkitTableDescriptor)new XmlSerializer(typeof(LedControlToolkitTableDescriptor)).Deserialize(ms);
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
                TableNode.Refresh();

                var TCCNumber = 0;
                foreach(var te in tableDescriptor.TableElements) {
                    var newTE = new TableElement() { TableElementType = te.Type, Name = te.Name, Number = te.Number };
                    newTE.AssignedEffects = new AssignedEffectList();
                    TableNode.EditionTable.TableElements.Add(newTE);
                    var newTENode = new TableElementTreeNode(newTE, LedControlToolkitHandler.ETableType.EditionTable);
                    TableNode.Nodes.Add(newTENode);

                    var SettingNumber = 0;
                    foreach (var eff in te.Effects) {
                        TableConfigSetting TCS = new TableConfigSetting();
                        TCS.ParseSettingData(eff.TCS);

                        var ToyName = ToyOutputMappings.FirstOrDefault(kv => kv.Value.Equals(eff.ToyOutput, StringComparison.InvariantCultureIgnoreCase)).Key;
                        var Toy = Handler.Pinball.Cabinet.Toys.FirstOrDefault(T=>T.Name.Equals(ToyName, StringComparison.InvariantCultureIgnoreCase));
                        var newEffect = Handler.RebuildConfigurator.CreateEffect(TCS, TCCNumber, SettingNumber, TableNode.EditionTable
                                                                                , Toy
                                                                                , Handler.LedControlConfigData.LedWizNumber
                                                                                , Handler.LedControlConfigData.LedControlIniFile.DirectoryName, TableNode.EditionTable.RomName);
                        var newEffectNode = new EffectTreeNode(newTE, LedControlToolkitHandler.ETableType.EditionTable, newEffect, Handler.LedControlConfigData);
                        SettingNumber++;
                    }

                    foreach (var eff in TableNode.EditionTable.Effects) {
                        eff.Init(TableNode.EditionTable);
                    }
                    TCCNumber++;
                    newTE.AssignedEffects.Init(TableNode.EditionTable);
                    newTENode.Rebuild(Handler);
                }

                TableNode.Refresh();

                return true;
            }

            return false;
        }
    }
}