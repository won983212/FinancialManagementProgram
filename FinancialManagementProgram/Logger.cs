using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
