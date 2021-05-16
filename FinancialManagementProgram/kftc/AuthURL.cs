using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftc
{
    enum AuthType
    {
        Initial, Skip = 2
    }

    class AuthURL
    {
        private static readonly Random Rand = new Random();
        private static readonly string Host = "https://testapi.openbanking.or.kr/oauth/2.0/";
        private static readonly string ClientID = "e897ea89-1499-4553-b058-740b58db251b";

        public static string Authorize(AuthType authType)
        {
            return string.Format("{0}authorize?response_type=code&client_id={1}&redirect_uri=http://localhost/&scope=login inquiry&state={2}&auth_type={3}",
                Host, ClientID, GenerateRandom32(), (int)authType);
        }

        private static string GenerateRandom32()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < 32; i++)
            {
                int value = Rand.Next(0, 62);
                if(value < 10)
                    sb.Append((char)('0' + value));
                else if(value < 36)
                    sb.Append((char)('a' + value - 10));
                else
                    sb.Append((char)('A' + value - 36));
            }
            return sb.ToString();
        }
    }
}
