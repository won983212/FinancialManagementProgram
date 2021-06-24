using System;
using System.IO;

namespace FinancialManagementProgram.Data
{
    public enum AccountColor
    {
        Blue, Red, Yellow, Green, Black
    }

    public class BankAccount : ObservableObject, IPropertiesSerializable
    {
        private string _label;
        private string _bankName;
        private AccountColor _color;
        private string _memo = "";

        public BankAccount(long id)
        {
            ID = id;
        }

        public BankAccount(BinaryReader reader)
        {
            Deserialize(reader);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(Label);
            writer.Write(BankName);
            writer.Write((short)Color);
            writer.Write(Memo ?? "");
        }

        public void Deserialize(BinaryReader reader)
        {
            ID = reader.ReadInt64();
            Label = reader.ReadString();
            BankName = reader.ReadString();
            Color = (AccountColor)reader.ReadInt16();
            Memo = reader.ReadString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is BankAccount == false)
                return false;
            return ID == ((BankAccount)obj).ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString()
        {
            return Label;
        }


        public long ID { get; private set; }

        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged();
            }
        }

        public string BankName
        {
            get => _bankName;
            set
            {
                _bankName = value;
                OnPropertyChanged();
            }
        }

        public AccountColor Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        public string Memo
        {
            get => _memo;
            set
            {
                _memo = value;
                OnPropertyChanged();
            }
        }

        public TransactionGroup MonthlyTransactions { get; } = new TransactionGroup();

        public static string[] AccountColorItems
        {
            get => Enum.GetNames(typeof(AccountColor));
        }
    }
}
