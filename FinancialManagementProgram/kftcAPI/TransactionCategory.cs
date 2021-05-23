using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftcAPI
{
    public class TransactionCategory
    {
        private static readonly Dictionary<string, string> _iconMap = new Dictionary<string, string>();


        public TransactionCategory(string categoryName)
        {
            if (!_iconMap.ContainsKey(categoryName))
                throw new InvalidOperationException("Unknown category: " + categoryName);

            Label = categoryName;
            IconName = _iconMap[categoryName];
        }


        public override bool Equals(object obj)
        {
            TransactionCategory target = obj as TransactionCategory;
            return target != null && Label.Equals(target.Label);
        }

        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }

        public override string ToString()
        {
            return Label;
        }


        static TransactionCategory()
        {
            _iconMap["편의점"] = "CartOutline";
            _iconMap["게임"] = "GamepadVariantOutline";
        }

        public string Label { get; }
        public string IconName { get; }
    }
}
