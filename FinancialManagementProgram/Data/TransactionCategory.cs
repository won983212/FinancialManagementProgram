using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace FinancialManagementProgram.Data
{
    public class TransactionCategory : ObservableObject
    {
        public static readonly long UnknownCategoryID = 0;
        private static ObservableCollection<TransactionCategory> _categoryValues = new ObservableCollection<TransactionCategory>();
        private static Dictionary<long, TransactionCategory> _categoryMap { get; } = new Dictionary<long, TransactionCategory>();

        private string _label;
        private PackIconKind _icon;


        private TransactionCategory(string label, PackIconKind icon)
            : this(GenerateUniqueID(), label, icon)
        { }

        private TransactionCategory(long id, string label, PackIconKind icon)
        {
            ID = id;
            Label = label;
            Icon = icon;
        }


        public override bool Equals(object obj)
        {
            return obj is TransactionCategory target && ID.Equals(target.ID);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString()
        {
            return Label;
        }

        internal static void Deserialize(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
            {
                long id = reader.ReadInt64();
                string label = reader.ReadString();
                int icon = reader.ReadInt32();
                _categoryMap.Add(id, new TransactionCategory(id, label, (PackIconKind)icon));
            }

            if (len == 0)
                AddDefaults();

            FireNotifyCategoryValues();
        }

        internal static void Serialize(BinaryWriter writer)
        {
            if (_categoryMap.Count == 0)
                AddDefaults();

            writer.Write(_categoryMap.Count);
            foreach (var ent in _categoryMap)
            {
                writer.Write(ent.Key);
                writer.Write(ent.Value.Label);
                writer.Write((int)ent.Value.Icon);
            }
        }

        private static long GenerateUniqueID()
        {
            return CommonUtil.GenerateUniqueID((x) => _categoryMap.ContainsKey(x));
        }

        private static void AddDefaults()
        {
            TransactionCategory category = new TransactionCategory(UnknownCategoryID, "미분류", PackIconKind.HelpCircle);
            _categoryMap.Add(UnknownCategoryID, category);

            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream stream = asm.GetManifestResourceStream("FinancialManagementProgram.DefaultCategories.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] args = line.Split(',');
                    RegisterCategory(args[0], (PackIconKind)Enum.Parse(typeof(PackIconKind), args[1]));
                }
            }
        }

        public static TransactionCategory GetCategory(long id)
        {
            if (_categoryMap.TryGetValue(id, out TransactionCategory category))
                return category;
            Logger.Error(new InvalidOperationException("알 수 없는 Category ID: " + id));
            return null;
        }

        public static TransactionCategory GetCategory(string label)
        {
            foreach (TransactionCategory c in _categoryMap.Values)
            {
                if (c.Label == label)
                    return c;
            }
            return null;
        }

        public static TransactionCategory RegisterCategory(string label, PackIconKind icon)
        {
            TransactionCategory category = new TransactionCategory(label, icon);
            _categoryMap.Add(category.ID, category);
            FireNotifyCategoryValues();
            return category;
        }

        public static bool UnregisterCategory(long id)
        {
            bool result = _categoryMap.Remove(id);
            FireNotifyCategoryValues();
            return result;
        }

        private static void FireNotifyCategoryValues()
        {
            _categoryValues.Clear();
            foreach (TransactionCategory ent in _categoryMap.Values)
                _categoryValues.Add(ent);
        }


        public long ID { get; }

        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged();
            }
        }

        public PackIconKind Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged();
            }
        }

        public static IEnumerable<TransactionCategory> Categories
        {
            get => _categoryValues;
        }
    }

    public class CategorySerializer : IPropertiesSerializable
    {
        public void Deserialize(BinaryReader reader)
        {
            TransactionCategory.Deserialize(reader);
        }

        public void Serialize(BinaryWriter writer)
        {
            TransactionCategory.Serialize(writer);
        }
    }
}
