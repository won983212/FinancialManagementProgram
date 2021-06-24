using System;
using System.IO;
using System.Text;

namespace FinancialManagementProgram.Data
{
    public class Transaction : ObservableObject
    {
        private string _label;
        private TransactionCategory _category;
        private long _amount;
        private BankAccount _account;
        private string _description = "";
        private DateTime _transDateTime;


        public Transaction()
        { }

        public Transaction(DataManager dataManager, BinaryReader reader)
        {
            Label = reader.ReadString();

            long categoryId = reader.ReadInt64();
            Category = TransactionCategory.GetCategory(categoryId);
            if (Category == null)
                Logger.Error(new InvalidOperationException("알 수 없는 CategoryID입니다. (" + categoryId + ")"));

            Amount = reader.ReadInt64();
            Account = dataManager.FindAccount(reader.ReadInt64());
            Description = reader.ReadString();
            TransDateTime = new DateTime(reader.ReadInt64());
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Label);
            writer.Write(Category.ID);
            writer.Write(Amount);
            writer.Write(_account.ID);
            writer.Write(Description);
            writer.Write(TransDateTime.Ticks);
        }

        /// <summary>
        /// Amount는 값이 수정되면 Analyzed Data가 망가지므로 Revalidate해야 한다.
        /// </summary>
        /// <param name="src"></param>
        public void Copy(Transaction src)
        {
            Label = src.Label;
            Category = src.Category;
            Amount = src.Amount;
            Account = src.Account;
            Description = src.Description;
            TransDateTime = src.TransDateTime;
        }

        public void DeserializeFromCSVLine(DataManager dataManager, string line)
        {
            StringBuilder sb = new StringBuilder();
            bool inText = false;
            int index = 0;
            char c;

            for (int i = 0; i < line.Length; i++)
            {
                c = line[i];
                if (c == '\"')
                {
                    if (inText)
                    {
                        if (i < line.Length - 1 && line[i + 1] == '\"') // if next char is also -> "
                        {
                            sb.Append('\"');
                            i++;
                        }
                        else
                        {
                            inText = false;
                        }
                    }
                    else
                    {
                        inText = true;
                    }
                }
                else if (c == ',')
                {
                    if (inText)
                        throw new InvalidDataException("따옴표가 완전히 닫히지 않았습니다: " + line);
                    ApplyProperty(dataManager, index++, sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            ApplyProperty(dataManager, index, sb.ToString());
        }

        private void ApplyProperty(DataManager dataManager, int index, string text)
        {
            switch (index)
            {
                case 0:
                    Label = text;
                    if (string.IsNullOrWhiteSpace(Label))
                        throw new InvalidDataException("거래명은 반드시 입력되어야합니다: " + text);
                    break;
                case 1:
                    TransactionCategory _category = TransactionCategory.GetCategory(text);
                    if (_category == null)
                        throw new InvalidDataException("찾을 수 없는 카테고리입니다: " + text);
                    else
                        Category = _category;
                    break;
                case 2:
                    if (long.TryParse(text, out long _amount))
                        Amount = _amount;
                    else
                        throw new InvalidDataException("거래량은 정수로 입력해야합니다: " + text);
                    break;
                case 3:
                    Account = dataManager.FindAccount(text);
                    if (Account == null)
                        throw new InvalidDataException("찾을 수 없는 자산명입니다: " + text);
                    break;
                case 4:
                    if (DateTime.TryParse(text, out DateTime _date))
                        TransDateTime = _date;
                    else
                        throw new InvalidDataException("날짜 포맷을 읽을 수 없습니다: " + text);
                    break;
                case 5:
                    if (DateTime.TryParse(text, out DateTime _time))
                        TransDateTime = new DateTime(TransDateTime.Year, TransDateTime.Month, TransDateTime.Day, _time.Hour, _time.Minute, _time.Second);
                    else
                        throw new InvalidDataException("시각 포맷을 읽을 수 없습니다: " + text);
                    break;
                case 6:
                    Description = text;
                    break;
                default:
                    throw new InvalidDataException("CSV라인에 데이터가 너무 많습니다: " + (index + 1) + "번째 라인");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is Transaction == false)
                return false;
            Transaction target = (Transaction)obj;
            return TransDateTime == target.TransDateTime && Label == target.Label && Amount == target.Amount;
        }

        public override int GetHashCode()
        {
            return TransDateTime.GetHashCode();
        }


        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged();
            }
        }

        public TransactionCategory Category
        {
            get => _category;
            set
            {
                _category = value;
                if (_category == null)
                {
                    Logger.Error(new ArgumentNullException("Category 데이터가 손실되었습니다:" + Label));
                    _category = TransactionCategory.GetCategory(TransactionCategory.UnknownCategoryID);
                }
                OnPropertyChanged();
            }
        }

        public long Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public BankAccount Account
        {
            get => _account;
            set
            {
                _account = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string TransDate { get; private set; }

        public DateTime TransDateTime
        {
            get => _transDateTime;
            set
            {
                _transDateTime = value;
                TransDate = _transDateTime.ToString("yyyyMMdd");
                OnPropertyChanged();
                OnPropertyChanged(nameof(TransDate));
            }
        }


        public string FormattedTransDate
        {
            get => string.Format("{0}.{1}.{2}", TransDate.Substring(0, 4), TransDate.Substring(4, 2), TransDate.Substring(6, 2));
        }

        public string FormattedTransDateTime
        {
            get => TransDateTime.ToString();
        }
    }
}
