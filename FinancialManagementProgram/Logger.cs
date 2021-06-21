using System;

namespace FinancialManagementProgram
{
    class Logger
    {
        public static void Info(object o)
        {
            Console.WriteLine(o.ToString());
        }

        public static void Warn(object o)
        {
            Console.WriteLine(o.ToString());
        }

        public static void Error(Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
