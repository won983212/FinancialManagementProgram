using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftcAPI
{
    class UserAccessToken
    {
        public string AccessToken { get; }
        public long ExpiresIn { get; }
        public string RefreshToken { get; }
        public string UserSeqNo { get; }

        public UserAccessToken(JObject obj)
        {
            AccessToken = obj.Value<string>("access_token");
            ExpiresIn = obj.Value<long>("expires_in");
            RefreshToken = obj.Value<string>("refresh_token");
            UserSeqNo = obj.Value<string>("user_seq_no");
        }
    }
}
