using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;

namespace FinancialManagementProgram.Data
{
    public class TransactionCategory
    {
        // TODO 동일내역 유지
        public static readonly string UnknownCategoryLabel = "미분류";
        public static Dictionary<string, PackIconKind> CategoryMap { get; } = new Dictionary<string, PackIconKind>();


        public TransactionCategory(string categoryName)
        {
            if (!CategoryMap.ContainsKey(categoryName))
                throw new InvalidOperationException("Unknown category: " + categoryName);

            Label = categoryName;
            Icon = CategoryMap[categoryName];
        }


        public override bool Equals(object obj)
        {
            return obj is TransactionCategory target && Label.Equals(target.Label);
        }

        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }

        public override string ToString()
        {
            return Label;
        }

        public string Label { get; }
        public PackIconKind Icon { get; }
    }

    public class CategorySerializer : IPropertiesSerializable
    {
        public void Deserialize(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
                TransactionCategory.CategoryMap.Add(reader.ReadString(), (PackIconKind)reader.ReadInt32());
            if (len == 0)
                AddDefaults();
        }

        public void Serialize(BinaryWriter writer)
        {
            if (TransactionCategory.CategoryMap.Count == 0)
                AddDefaults();
            writer.Write(TransactionCategory.CategoryMap.Count);
            foreach (var ent in TransactionCategory.CategoryMap)
            {
                writer.Write(ent.Key);
                writer.Write((int)ent.Value);
            }
        }

        private void AddDefaults()
        {
            Dictionary<string, PackIconKind> map = TransactionCategory.CategoryMap;
            map[TransactionCategory.UnknownCategoryLabel] = PackIconKind.HelpCircle;
            map["식비"] = PackIconKind.Food;
            map["카페"] = PackIconKind.Coffee;
            map["쇼핑"] = PackIconKind.Basket;
            map["뷰티"] = PackIconKind.Brush;
            map["교통"] = PackIconKind.TrainCar;
            map["생활"] = PackIconKind.HomeCity;
            map["편의점"] = PackIconKind.CartOutline;
            map["술"] = PackIconKind.GlassMugVariant;
            map["주유"] = PackIconKind.Oil;
            map["세차"] = PackIconKind.CarWash;
            map["주거"] = PackIconKind.Building;
            map["통신"] = PackIconKind.SignalVariant;
            map["의료"] = PackIconKind.HospitalBox;
            map["금융"] = PackIconKind.Finance;
            map["문화"] = PackIconKind.Ticket;
            map["여가"] = PackIconKind.GamepadVariantOutline;
            map["여행"] = PackIconKind.WalletTravel;
            map["학습"] = PackIconKind.BookOpenVariant;
            map["경조"] = PackIconKind.Email;
            map["선물"] = PackIconKind.GiftOutline;
            map["운동"] = PackIconKind.Dumbbell;
        }
    }
}
