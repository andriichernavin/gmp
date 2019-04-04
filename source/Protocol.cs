using System;
using System.Collections.Generic;
using System.Xml;

namespace gmp
{
    public class Protocol
    {
        public static List<StructBlock> BlockList = new List<StructBlock>();
        
        public static List<StructReceipt> ReceiptList = new List<StructReceipt>();

        public class StructBlock
        {
            public ushort Company;
            public ushort Code;
            public ushort Size;
            public string Name;
            public string Table;

            public List<StructValue> ValueList = new List<StructValue>();
        }

        public class StructReceipt
        {
            public ushort Company;
            public ushort Code;
            public ushort Size;
            public string Name;
            public string Table;

            public List<StructValue> ValueList = new List<StructValue>();
        }

        public struct StructValue
        {
            public ushort Offset;
            public ushort Size;
            public string Type;
            public byte Base;
            public string Parameter;
            public string Field;
            public string Name;
            
            private List<StructFlag> FlagList;

            public void AddFlag(string Parameter)
            {
                if (Parameter == null) return;

                string[] Parameters = Parameter.Split(new char[] { ';' });
                
                for (int i = 0; i < Parameters.Length; i++ )
                {
                    string FlagParameter = Parameters[i].Trim();

                    if (FlagParameter.Length > 0)
                    {
                        if (FlagList == null) FlagList = new List<StructFlag>();

                        FlagList.Add(new StructFlag(FlagParameter, Name));
                    }
                }
            }

            public bool CheckFlag(byte[] Buffer, ushort Size)
            {
                if (FlagList == null) return true;
                
                foreach (StructFlag Flag in FlagList)
                {
                    if (!((Protocol.StructFlag)Flag).Check(Buffer, Size)) return false;
                }

                return true;
            }
        }

        public struct StructFlag
        {
            private ushort Offset;
            private byte Mask;
            private byte Value;
            private string Name;

            public StructFlag(string Parameter, string ValueName)
            {
                string[] Parameters = Parameter.Split(new char[] { ',' });

                if (Parameters.Length != 3) throw new Exception("Flag format error");

                Offset = Convert.ToUInt16(Parameters[0]);
                Mask = Convert.ToByte(Parameters[1]);
                Value = Convert.ToByte(Parameters[2]);

                Name = ValueName + " (" + Parameter + ")";
            }

            public bool Check(byte[] Buffer, ushort Size)
            {
                if (Offset >= Size) throw new Exception("Flag is out of block limits: " + Name);

                return ((Buffer[Offset] ^ Mask) == Value);
            }
        }

        static Protocol()
        {
            ushort Company = 0;

            StructBlock Block = new StructBlock();
            StructReceipt Receipt = new StructReceipt();
            StructValue Value;

            string GroupElement = "";

            XmlReader xmlReader = XmlReader.Create(Properties.Settings.Default.Protocol);
            
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Whitespace)
                { 
                    // пропускаем
                }
                else if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "Company") && xmlReader.HasAttributes)
                {
                    Company = Convert.ToUInt16(xmlReader.GetAttribute("Code"));
                }
                else if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "Block") && xmlReader.HasAttributes)
                {
                    Block = new StructBlock();

                    try { Block.Name = xmlReader.GetAttribute("Name").Trim(); }
                    catch { Block.Name = "";  }

                    try { Block.Table = xmlReader.GetAttribute("Table").Trim(); }
                    catch { Block.Table = ""; }

                    Block.Code = Convert.ToUInt16(xmlReader.GetAttribute("Code"));
                    Block.Size = Convert.ToUInt16(xmlReader.GetAttribute("Size"));

                    Block.Company = Company;

                    BlockList.Add(Block);

                    GroupElement = "Block";
                }
                else if ((xmlReader.NodeType == XmlNodeType.EndElement) && (xmlReader.Name == "Block"))
                {
                    GroupElement = "";
                }
                else if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "Receipt") && xmlReader.HasAttributes)
                {
                    Receipt = new StructReceipt();

                    try { Receipt.Name = xmlReader.GetAttribute("Name").Trim(); }
                    catch { Receipt.Name = ""; }

                    try { Receipt.Table = xmlReader.GetAttribute("Table").Trim(); }
                    catch { Receipt.Table = ""; }

                    Receipt.Code = Convert.ToUInt16(xmlReader.GetAttribute("Code"));
                    Receipt.Size = Convert.ToUInt16(xmlReader.GetAttribute("Size"));

                    Receipt.Company = Company;

                    ReceiptList.Add(Receipt);

                    GroupElement = "Receipt";
                }
                else if ((xmlReader.NodeType == XmlNodeType.EndElement) && (xmlReader.Name == "Receipt"))
                {
                    GroupElement = "";
                }
                else if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "Value") && xmlReader.HasAttributes)
                {
                    Value = new StructValue();

                    try { Value.Name = xmlReader.GetAttribute("Name").Trim(); }
                    catch { Value.Name = ""; }

                    try { Value.Type = xmlReader.GetAttribute("Type").Trim(); }
                    catch { Value.Type = ""; }

                    try { Value.Base = Convert.ToByte(xmlReader.GetAttribute("Base")); }
                    catch { Value.Base = 10; }

                    try { Value.Field = xmlReader.GetAttribute("Field").Trim(); }
                    catch { Value.Field = ""; }

                    try { Value.Parameter = xmlReader.GetAttribute("Parameter").Trim(); }
                    catch { Value.Parameter = ""; }

                    Value.Offset = Convert.ToUInt16(xmlReader.GetAttribute("Offset"));
                    Value.Size = Convert.ToUInt16(xmlReader.GetAttribute("Size"));

                    Value.AddFlag(xmlReader.GetAttribute("Flag"));
                    
                    if (GroupElement == "Block")
                    {
                        Block.ValueList.Add(Value);
                    }
                    else if (GroupElement == "Receipt")
                    {
                        Receipt.ValueList.Add(Value);
                    }
                    else
                    {
                        throw new Exception("Error in protocol file");
                    }
                }
            }
        }
    }
}
