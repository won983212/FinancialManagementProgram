namespace FinancialManagementProgram.Dialog.ViewModel
{
    class AddAccountVM : DialogViewModel
    {
        public bool HasError { get; set; }

        public string Label { get; set; }

        public string BankName { get; set; }

        public int ColorIndex { get; set; } = 0;

        public string Memo { get; set; }
    }
}
