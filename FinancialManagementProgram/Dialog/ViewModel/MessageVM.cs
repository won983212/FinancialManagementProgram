namespace FinancialManagementProgram.Dialog.ViewModel
{
    class MessageVM : DialogViewModel
    {
        public MessageVM(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public string Title { get; }
        public string Message { get; }
    }
}
