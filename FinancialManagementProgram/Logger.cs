using FinancialManagementProgram.ViewModels;
using System;

namespace FinancialManagementProgram
{
    class Logger
    {
        public static void Info(object o)
        {
            MainViewModel.SnackMessageQueue.Enqueue(o.ToString());
        }

        public static void Warn(object o)
        {
            Console.WriteLine(o.ToString());
            MainViewModel.SnackMessageQueue.Enqueue("주의: " + o.ToString());
        }

        public static void Error(Exception e)
        {
            Console.WriteLine(e);
            MainViewModel.SnackMessageQueue.Enqueue("오류: " + e.Message);
        }
    }
}
