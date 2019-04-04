using System;
using System.Collections.Generic;
using System.Data.Common;

namespace gmp
{
    public class IncomingPacket
    {
        private byte Id;
        private string TextId;

        public ushort Size;

        public byte Channel;
        public uint Serial;
        public byte Company;
        public byte Device;
        public byte SoftVersion;
        public byte HardVersion;
        public ulong Imei;
        public uint Phone;

        private byte Operation;
        private List<Block> BlockList = new List<Block>();
        private ushort Crc;
        private bool CorrectCrc;

        public void DecodePrefix(byte[] Buffer)
        {
            Id = Buffer[0];
            if (Id != 0x96) throw new Exception("Prefix format error");

            TextId = System.Text.Encoding.ASCII.GetString(Buffer, 1, 3);
            if (TextId != "RTV") throw new Exception("Prefix format error");
            
            Size = System.BitConverter.ToUInt16(Buffer, 4);
            if (Size <= Constant.PrefixSize + 2) throw new Exception("Message is too short");
            if (Size > Constant.BufferSize) throw new Exception("Message is too long");

            SoftVersion = Buffer[6];
            HardVersion = Buffer[7];
            Channel = Buffer[8];
            Serial = System.BitConverter.ToUInt32(Buffer, 9);
            Company = Buffer[13];
            Device = Buffer[14];
            Imei = System.BitConverter.ToUInt64(Buffer, 15);
            Phone = System.BitConverter.ToUInt32(Buffer, 23);
            Operation = Buffer[31];
        }
        public void WritePrefix(DbConnect DbConnect)
        {
            DbConnect.ClearParameters();

            DbConnect.Query = "INSERT INTO prefix_reports (`session_time`,`prefix0`, `prefix1_3`, `prefix4_5`, `prefix8`, `prefix9_12`, `prefix13`, `prefix14`, `prefix15_22`, `prefix23_26`, `prefix31`, `crc_packet`, `crc_packet_ok`)"
                + " VALUES (now(), @Id, @TextId, @Size, @Channel, @Serial, @Company, @Device, @Imei, @Phone, @Operation, @Crc, @CorrectCrc)";

            DbConnect.AddParameter("Id", Id);
            DbConnect.AddParameter("TextId", TextId);
            DbConnect.AddParameter("Size", Size);
            DbConnect.AddParameter("Channel", Channel);
            DbConnect.AddParameter("Serial", Serial);
            DbConnect.AddParameter("Company", Company);
            DbConnect.AddParameter("Device", Device);
            DbConnect.AddParameter("Imei", Imei);
            DbConnect.AddParameter("Phone", Phone);
            DbConnect.AddParameter("Operation", Operation);
            DbConnect.AddParameter("Crc", Crc.ToString("X4"));
            DbConnect.AddParameter("CorrectCrc", (CorrectCrc ? 1 : 0));

            DbConnect.ExecuteNonQuery();
        }
        
        public void DecodeData(byte[] Buffer)
        {
            Crc = System.BitConverter.ToUInt16(Buffer, Size - 2);
            CorrectCrc = (Crc == CRC16.Compute(Buffer, 0, Size - 2));

            if (!CorrectCrc)
            {
                if (Properties.Settings.Default.IgnoreCRC)
                    Console.WriteLine("CRC error in packet");
                else
                    throw new Exception("CRC error in packet").SetCode(Constant.ExceptionCRC);
            }

            ushort Position = Constant.PrefixSize;

            while (Position < Size - 2)
            {
                Block CurrentBlock = new Block(Buffer, Company, Position, (ushort)(Size - 2 - Position));
                BlockList.Add(CurrentBlock);
                Position = (ushort)(Position + CurrentBlock.Size);
            }
        }
        public void WriteData(DbConnect DbConnect)
        {
            foreach (Block CurrentBlock in BlockList)
            {
                DbConnect.ClearParameters();

                DbConnect.AddParameter("Channel", Channel);
                DbConnect.AddParameter("Serial", Serial);
                DbConnect.AddParameter("Company", Company);
                DbConnect.AddParameter("Device", Device);
                DbConnect.AddParameter("SoftVersion", SoftVersion);
                DbConnect.AddParameter("HardVersion", HardVersion);
                DbConnect.AddParameter("Imei", Imei);
                DbConnect.AddParameter("Phone", Phone);
                
                CurrentBlock.Write(DbConnect);
            }
        }
    }

    public class Block
    {
        private Protocol.StructBlock ChosenBlock;

        private byte[] Data = new byte[Constant.BufferSize - Constant.PrefixSize - 2];

        private ushort Crc;
        private bool CorrectCrc;

        public Block(byte[] Buffer, byte Company, ushort Position, ushort MaxSize)
        {
            byte Code = Buffer[Position];

            foreach (Protocol.StructBlock CurrentBlock in Protocol.BlockList)
            {
                if ((CurrentBlock.Company == Company) && (CurrentBlock.Code == Code) && (CurrentBlock.Size <= MaxSize))
                {
                    Crc = System.BitConverter.ToUInt16(Buffer, Position + CurrentBlock.Size - 2);
                    CorrectCrc = (Crc == CRC16.Compute(Buffer, Position, CurrentBlock.Size - 2));

                    if (!CorrectCrc)
                    {
                        if (Properties.Settings.Default.IgnoreCRC)
                            Console.WriteLine("CRC error in block");
                        else
                            throw new Exception("CRC error in block").SetCode(Constant.ExceptionCRC);
                    }

                    ChosenBlock = CurrentBlock;

                    if (Properties.Settings.Default.ShowBlock) Console.WriteLine(ChosenBlock.Name);

                    Array.Copy(Buffer, Position, Data, 0, ChosenBlock.Size);

                    return;
                }
            }

            throw new Exception("Unknown block");
        }

        public ushort Size
        {
            get
            {
                if (ChosenBlock == null) throw new Exception("Empty block");

                return ChosenBlock.Size;
            }
        }

        public void Write(DbConnect DbConnect)
        {
            string FieldString = "";
            string ParameterString = "";

            foreach (Protocol.StructValue CurrentValue in ChosenBlock.ValueList)
            {
                // сверяем значение флага
                if (!CurrentValue.CheckFlag(Data, Size)) continue;

                if (CurrentValue.Field.Length == 0)
                {
                    // неизвестное поле

                    throw new Exception("Unknown field: " + CurrentValue.Name);
                }
                else if (CurrentValue.Parameter.Length > 0)
                {
                    // поле из префикса

                    if (!DbConnect.CheckParameter(CurrentValue.Parameter)) throw new Exception("Unknown parameter: " + CurrentValue.Name);

                    FieldString += (FieldString.Length > 0 ? "," : "") + "`" + CurrentValue.Field + "`";

                    ParameterString += (ParameterString.Length > 0 ? "," : "") + "@" + CurrentValue.Parameter;
                }
                else if (CurrentValue.Type.Length > 0 && CurrentValue.Size > 0)
                {
                    // поле из блока, распознаем тип и длину

                    if (CurrentValue.Offset + CurrentValue.Size >= Size) throw new Exception("Value is out of block limits: " + CurrentValue.Name);

                    if (CurrentValue.Type == "uint" && CurrentValue.Size == 1)
                    {
                        DbConnect.AddParameter("_" + CurrentValue.Field, Data[CurrentValue.Offset]);
                    }
                    else if (CurrentValue.Type == "uint" && CurrentValue.Size == 2)
                    {
                        DbConnect.AddParameter("_" + CurrentValue.Field, System.BitConverter.ToUInt16(Data, CurrentValue.Offset));
                    }
                    else if (CurrentValue.Type == "uint" && CurrentValue.Size == 4)
                    {
                        DbConnect.AddParameter("_" + CurrentValue.Field, System.BitConverter.ToUInt32(Data, CurrentValue.Offset));
                    }
                    else if (CurrentValue.Type == "uint" && CurrentValue.Size == 8)
                    {
                        DbConnect.AddParameter("_" + CurrentValue.Field, System.BitConverter.ToUInt64(Data, CurrentValue.Offset));
                    }
                    else if (CurrentValue.Type == "float" && CurrentValue.Size == 4)
                    {
                        if (BitConverter.ToString(Data, CurrentValue.Offset, 4) == "00-00-C0-7F") continue;
                        DbConnect.AddParameter("_" + CurrentValue.Field, System.BitConverter.ToSingle(Data, CurrentValue.Offset));
                    }
                    else if (CurrentValue.Type == "datetime" && CurrentValue.Size == 4)
                    {
                        DbConnect.AddParameter("_" + CurrentValue.Field, Converter.ToDateTime32(Data, CurrentValue.Offset));
                    }
                    else if (CurrentValue.Type == "char" && CurrentValue.Size > 0)
                    {
                        DbConnect.AddParameter("_" + CurrentValue.Field, System.BitConverter.ToString(Data, CurrentValue.Offset, CurrentValue.Size).Replace("-", string.Empty));
                    }
                    else if (CurrentValue.Type == "byte" && CurrentValue.Size > 0)
                    {
                        byte[] arr = new byte[CurrentValue.Size];
                        Array.Copy(Data, CurrentValue.Offset, arr, 0, CurrentValue.Size);
                        DbConnect.AddParameter("_" + CurrentValue.Field, arr);
                    }
                    else
                    {
                        throw new Exception("Unknown type: " + CurrentValue.Type + " of " + CurrentValue.Size.ToString() + "bytes");
                    }

                    FieldString += (FieldString.Length > 0 ? "," : "") + "`" + CurrentValue.Field + "`";

                    ParameterString += (ParameterString.Length > 0 ? "," : "") + "@_" + CurrentValue.Field;
                }
                else
                {
                    // неизвестное поле

                    throw new Exception("Unknown value: " + CurrentValue.Name);
                }
            }

            if (FieldString.Length > 0)
            {
                DbConnect.Query = "INSERT INTO " + ChosenBlock.Table + " (" + FieldString + ") VALUES (" + ParameterString + ")";

                try
                {
                    DbConnect.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception((Properties.Settings.Default.DebugInfo ? e.Message : "Database write error"));
                }
            }
        }
    }

    public class OutgoingPacket
    {
        private byte Id;
        private string TextId;

        public ushort Size;

        public byte Channel;
        public uint Serial;
        public byte Company;
        public byte Device;
        public byte SoftVersion;
        public byte HardVersion;
        public ulong Imei;
        public uint Phone;

        private byte Operation;
        private Receipt Receipt;
        private ushort Crc;

        public OutgoingPacket(IncomingPacket IncomingPacket)
        {
            Id = 0x69;
            TextId = "RTV";

            Channel = IncomingPacket.Channel;
            Serial = IncomingPacket.Serial;
            Company = IncomingPacket.Company;
            Device = IncomingPacket.Device;
            SoftVersion = IncomingPacket.SoftVersion;
            HardVersion = IncomingPacket.HardVersion;
            Imei = IncomingPacket.Imei;
            Phone = IncomingPacket.Phone;
        }

        public void WriteTest(DbConnect DbConnect)
        {
            DbConnect.ClearParameters();
            
            DbConnect.AddParameter("Channel", Channel);
            DbConnect.AddParameter("Serial", Serial);
            DbConnect.AddParameter("Company", Company);
            DbConnect.AddParameter("Device", Device);

            DbConnect.AddParameter("Code", 5);
            DbConnect.AddParameter("Data", "");

            DbConnect.Query = "INSERT INTO receipt_requests (`ts`,`serNUM`, `mfDEV`, `typeDEV`, `chNUM`, `Status`, `CodeOp`, `Data`) VALUES (now(), @Serial, @Company, @Device, @Channel, 0, @Code, @Data)";

            try
            {
                DbConnect.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception((Properties.Settings.Default.DebugInfo ? e.Message : "Database write error"));
            }
        }
        public void ReadData(DbConnect DbConnect)
        {
            DbDataReader Reader = null;

            long ReceiptId = 0;
            short ReceiptCode = 0;
            string ReceiptData = "";
            
            // ищем запрос для квитанции

            DbConnect.ClearParameters();

            DbConnect.AddParameter("Channel", Channel);
            DbConnect.AddParameter("Serial", Serial);
            DbConnect.AddParameter("Company", Company);
            DbConnect.AddParameter("Device", Device);

            DbConnect.Query = "SELECT id, codeop, data FROM receipt_requests WHERE sernum = @Serial AND mfdev = @Company AND typedev = @Device AND chnum = @Channel AND status = 0 LIMIT 1";

            try
            {
                Reader = DbConnect.ExecuteReader();

                if (Reader.Read())
                {
                    ReceiptId = Reader.GetInt64(Reader.GetOrdinal("id"));
                    ReceiptCode = Reader.GetInt16(Reader.GetOrdinal("codeop"));
                    ReceiptData = Reader.GetString(Reader.GetOrdinal("data")).Trim();
                }

                Reader.Close();
            }
            catch (Exception e)
            {
                if (Reader != null) Reader.Close();

                throw new Exception((Properties.Settings.Default.DebugInfo ? e.Message : "Database read error"));
            }

            // отмечаем отправляемый запрос

            WriteData(DbConnect, 1);

            // читаем и кодируем запрос

            if (ReceiptId > 0)
            {
                Receipt = new Receipt(Company, ReceiptId, ReceiptCode, ReceiptData);

                Receipt.Read(DbConnect);
            }
        }
        public void WriteData(DbConnect DbConnect, int Status)
        {
            if (Receipt == null) return;

            DbConnect.ClearParameters();

            DbConnect.AddParameter("Id", Receipt.Id);
            DbConnect.AddParameter("Status", Status);

            DbConnect.Query = "UPDATE receipt_requests SET `status` = @Status WHERE id = @Id";

            try
            {
                DbConnect.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception((Properties.Settings.Default.DebugInfo ? e.Message : "Database write error"));
            }
        }
        
        public void EncodePrefix(byte[] Buffer)
        {
            // префикс

            if (Receipt == null)
            {
                Operation = 0;
                Size = (ushort)(Constant.PrefixSize + 2);
            }
            else
            {
                Operation = 1;
                Size = (ushort)(Constant.PrefixSize + 2 + Receipt.Size + 3);
            }
            Size = (ushort)(Constant.PrefixSize + 2 + (Receipt != null ? (Receipt.Size + 3) : 0));

            Buffer[0] = Id;
            Array.Copy(System.Text.Encoding.ASCII.GetBytes(TextId), 0, Buffer, 1, 3);
            Array.Copy(System.BitConverter.GetBytes(Size), 0, Buffer, 4, 2);
            Buffer[6] = SoftVersion;
            Buffer[7] = HardVersion;
            Buffer[8] = Channel;
            Array.Copy(System.BitConverter.GetBytes(Serial), 0, Buffer, 9, 4);
            Buffer[13] = Company;
            Buffer[14] = Device;
            Array.Copy(System.BitConverter.GetBytes(Imei), 0, Buffer, 15, 8);
            Array.Copy(System.BitConverter.GetBytes(Phone), 0, Buffer, 23, 4);
            Buffer[27] = 0;
            Buffer[28] = 0;
            Buffer[29] = 0;
            Buffer[30] = 0;
            Buffer[31] = Operation;

            Crc = CRC16.Compute(Buffer, 0, Size - 2);
            Array.Copy(System.BitConverter.GetBytes(Crc), 0, Buffer, Size - 2, 2);
        }
        public void EncodeData(byte[] Buffer)
        {
            ushort Offset = Constant.PrefixSize;

            if (Receipt == null) return;

            Buffer[Offset] = Receipt.Code;
            Array.Copy(System.BitConverter.GetBytes((ushort)(Receipt.Size + 3)), 0, Buffer, Offset + 1, 2);
            Array.Copy(Receipt.Data, 0, Buffer, Offset + 3, Receipt.Size);
        }
    }

    public class Receipt
    {
        public long Id;
        public byte Code;
        public string Parameters;

        public byte[] Data = new byte[Constant.BufferSize - Constant.PrefixSize - 2];

        private Protocol.StructReceipt ChosenReceipt;

        public Receipt(byte Company, long ReceiptId, short ReceiptCode, string ReceiptData)
        {
            Id = ReceiptId;

            Code = (byte)ReceiptCode;

            Parameters = ReceiptData;

            foreach (Protocol.StructReceipt CurrentReceipt in Protocol.ReceiptList)
            {
                if ((CurrentReceipt.Company == Company) && (CurrentReceipt.Code == Code))
                {
                    ChosenReceipt = CurrentReceipt;

                    if (ChosenReceipt.Size > Constant.BufferSize - Constant.PrefixSize + 3) throw new Exception("Message is too long");

                    if (Properties.Settings.Default.ShowReceipt) Console.WriteLine(ChosenReceipt.Name);

                    return;
                }
            }

            throw new Exception("Unknown receipt");
        }

        public ushort Size
        {
            get
            {
                if (ChosenReceipt == null) throw new Exception("Empty receipt");

                return ChosenReceipt.Size;
            }
        }

        public void Read(DbConnect DbConnect)
        {
            DbDataReader Reader = null;

            // для начала заполняем нулями, после поверх реальными данными
            for (int i = 0; i < ChosenReceipt.Size; i++) Data[i] = 0;

            if (ChosenReceipt.Table.Length > 0)
            {
                DbConnect.ClearParameters();

                DbConnect.AddParameter("Id", Id);

                DbConnect.Query = "SELECT * FROM " + ChosenReceipt.Table + " WHERE idpx = @Id LIMIT 1";

                try
                {
                    Reader = DbConnect.ExecuteReader();

                    if (!Reader.Read())
                    {
                        Reader.Close();
                        Reader = null;

                        throw new Exception("Request read error");
                    }
                }
                catch (Exception e)
                {
                    if (Reader != null) Reader.Close();

                    throw new Exception((Properties.Settings.Default.DebugInfo ? e.Message : "Database read error"));
                }
            }

            foreach (Protocol.StructValue CurrentValue in ChosenReceipt.ValueList)
            {
                if (CurrentValue.Type.Length == 0 || CurrentValue.Size == 0)
                {
                    // неизвестное поле

                    throw new Exception("Unknown type: " + CurrentValue.Name);
                }
                else if (CurrentValue.Parameter.Length > 0)
                {
                    // поле из основной таблицы

                    string Parameter;

                    // получаем строку по индексу
                    try
                    {
                        Parameter = GetParameter(Parameters, int.Parse(CurrentValue.Parameter));
                    }
                    catch (Exception e)
                    { 
                        throw new Exception((Properties.Settings.Default.DebugInfo ? e.Message : "Parameter read error"));
                    }

                    // преобразуем к типу, кодируем, сохраняем в буфер квитанции
                    if (CurrentValue.Type == "uint" && CurrentValue.Size == 1)
                    {
                        Array.Copy(System.BitConverter.GetBytes(Convert.ToByte(Parameter, CurrentValue.Base)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "uint" && CurrentValue.Size == 2)
                    {
                        Array.Copy(System.BitConverter.GetBytes(Convert.ToInt16(Parameter, CurrentValue.Base)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "uint" && CurrentValue.Size == 4)
                    {
                        Array.Copy(System.BitConverter.GetBytes(Convert.ToInt32(Parameter, CurrentValue.Base)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "uint" && CurrentValue.Size == 8)
                    {
                        Array.Copy(System.BitConverter.GetBytes(Convert.ToInt64(Parameter, CurrentValue.Base)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "float" && CurrentValue.Size == 4)
                    {
                        Array.Copy(System.BitConverter.GetBytes(Single.Parse(Parameter)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "datetime" && CurrentValue.Size == 4)
                    {
                        Array.Copy(Converter.FromDateTime32(DateTime.ParseExact(Parameter, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "char" && CurrentValue.Size > 0)
                    {
                        if (Parameter.Length != CurrentValue.Size * 2) throw new Exception("Error in hex value");
                        Array.Copy(Converter.StringToByteArray(Parameter), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else
                    {
                        throw new Exception("Unknown type: " + CurrentValue.Type + " of " + CurrentValue.Size.ToString() + "bytes");
                    }
                }
                else if (CurrentValue.Field.Length > 0 && Reader != null)
                {
                    // поле из дополнительной таблицы

                    int Ordinal;

                    // проверяем наличие поля
                    try
                    {
                        Ordinal = Reader.GetOrdinal(CurrentValue.Field);
                    }
                    catch (Exception e)
                    { 
                        throw new Exception((Properties.Settings.Default.DebugInfo ? e.Message : "Field read error"));
                    }

                    // получаем значение, кодируем, сохраняем в буфер квитанции
                    if (CurrentValue.Type == "uint" && CurrentValue.Size == 1)
                    {
                        Array.Copy(System.BitConverter.GetBytes(Reader.GetByte(Ordinal)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "uint" && CurrentValue.Size == 2)
                    {
                        Array.Copy(System.BitConverter.GetBytes(Reader.GetInt16(Ordinal)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "uint" && CurrentValue.Size == 4)
                    {
                        Array.Copy(System.BitConverter.GetBytes(Reader.GetInt32(Ordinal)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "uint" && CurrentValue.Size == 8)
                    {
                        Array.Copy(System.BitConverter.GetBytes(Reader.GetInt64(Ordinal)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "float" && CurrentValue.Size == 4)
                    {
                        Array.Copy(System.BitConverter.GetBytes(Reader.GetFloat(Ordinal)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "datetime" && CurrentValue.Size == 4)
                    {
                        Array.Copy(Converter.FromDateTime32(Reader.GetDateTime(Ordinal)), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "char" && CurrentValue.Size > 0)
                    {
                        string str = Reader.GetString(Ordinal).Trim();
                        if (str.Length != CurrentValue.Size * 2) throw new Exception("Error in hex value");
                        Array.Copy(Converter.StringToByteArray(str), 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else if (CurrentValue.Type == "byte" && CurrentValue.Size > 0)
                    {
                        Reader.GetBytes(Ordinal, 0, Data, CurrentValue.Offset, CurrentValue.Size);
                    }
                    else
                    {
                        throw new Exception("Unknown type: " + CurrentValue.Type + " of " + CurrentValue.Size.ToString() + "bytes");
                    }
                }
                else if (CurrentValue.Field.Length > 0 && Reader == null)
                {
                    // поле из дополнительной таблицы
                    // запись не найдена - поля не заполняем
                }
                else
                {
                    // неизвестное поле

                    throw new Exception("Unknown value: " + CurrentValue.Name);
                }
            }

            if (Reader != null) Reader.Close();
        }

        private string GetParameter(string Parameters, int number)
        {
            if (number == 0) return Parameters;

            char[] delimiter = {':'};
            
            string[] array = Parameters.Split(delimiter, number);

            if (array.Length < number) throw new Exception("Parameter read error");
            
            return array[number - 1];
        }
    }
}
