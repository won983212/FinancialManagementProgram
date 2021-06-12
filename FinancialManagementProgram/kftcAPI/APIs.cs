using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace FinancialManagementProgram.kftcAPI
{
    enum AuthType
    {
        Initial, Skip = 2
    }

    static class APIs
    {
        private static readonly Random Rand = new Random();
        private static readonly string Host = "https://testapi.openbanking.or.kr/";
        private static readonly string State = "wdWkTPVdX7aftsVKh4kheoajbdK4ei2Z"; // must be 32 bytes
        private static readonly string ClientID = "e897ea89-1499-4553-b058-740b58db251b";
        private static readonly string ClientSecret = "9f842eb0-c9d3-46d6-98ac-9aced3bda268";
        private static readonly string OrganizationCode = "M202112264";

        static APIs()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        public static Task<UserAccessToken> GenerateToken(bool isAuthOnly)
        {
            TaskCompletionSource<UserAccessToken> tcs = new TaskCompletionSource<UserAccessToken>();
            new AuthWindow(AuthorizeURL(AuthType.Initial, isAuthOnly), (param) => OnAuthCallback(param, tcs)).Show();
            return tcs.Task;
        }

        public static async Task<Tuple<string, BankAccount[]>> GetUserInfo(UserAccessToken userToken)
        {
            string data = await Get(string.Format("{0}v2.0/user/me?user_seq_no={1}", Host, userToken.UserSeqNo), userToken.AccessToken);
            JObject obj = JObject.Parse(data);
            CheckRequestError(obj);

            JArray accounts_json = obj["res_list"] as JArray;
            BankAccount[] accounts = new BankAccount[accounts_json.Count];

            if(accounts_json != null)
            {
                string user_ci = obj.Value<string>("user_ci");
                for (int i = 0; i < accounts.Length; i++)
                    accounts[i] = new BankAccount((JObject)accounts_json[i]);
                return new Tuple<string, BankAccount[]>(user_ci, accounts);
            } 
            else
            {
                throw new ArgumentException("Illegal response: Unknown json type. (GetUserInfo)");
            }
        }

        public static async Task GetAccountDetails(BankAccount account, UserAccessToken userToken, int offset, string fromDate)
        {
            string currentDate = CurrentDate();
            string url = string.Format("{0}v2.0/account/transaction_list/fin_num?bank_tran_id={1}&fintech_use_num={2}&inquiry_type=A&inquiry_base=D&from_date={3}&to_date={4}&sort_order=D&tran_dtime={5}"
                , Host, CreateBankTransId(offset), account.FintechUseNum, fromDate, currentDate.Substring(0, 8), currentDate);
            string data = await Get(url, userToken.AccessToken);

            JObject obj = JObject.Parse(data);
            CheckRequestError(obj);
            account.BalanceAmount = obj.Value<long>("balance_amt");

            foreach (JToken token in (JArray)obj["res_list"])
                account.Transactions.AddTransaction(new Transaction(account.BankName, (JObject)token));
        }

        private static void OnAuthCallback(string query, TaskCompletionSource<UserAccessToken> tcs)
        {
            Dictionary<string, string> parameters = ParseParameter(query);
            if (parameters.ContainsKey("error"))
            {
                MessageBox.Show(parameters["error_description"]);
                tcs.SetException(new IOException("error"));
                return;
            }

            if (!parameters.ContainsKey("code"))
            {
                tcs.SetException(new IOException("no_code_param"));
                return;
            }

            string tokenParam = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri=http://localhost/&grant_type=authorization_code",
                parameters["code"], ClientID, ClientSecret);

            Post(Host + "oauth/2.0/token", tokenParam).ContinueWith((action) =>
            {
                JObject obj = JObject.Parse(action.Result);
                if (!obj.ContainsKey("access_token"))
                    tcs.SetException(new IOException(obj.Value<string>("rsp_message")));
                else
                    tcs.SetResult(new UserAccessToken(obj));
            });
        }

        private static string AuthorizeURL(AuthType authType, bool isAuthOnly)
        {
            string endPoint = isAuthOnly ? "authorize_account" : "authorize";
            return string.Format("{0}oauth/2.0/{1}?response_type=code&client_id={2}&redirect_uri=http://localhost/&scope=login inquiry&authorized_cert_yn=N&state={3}&auth_type={4}",
                Host, endPoint, ClientID, State, (int)authType);
        }


        #region Util

        private static string CreateBankTransId(int offset)
        {
            return OrganizationCode + "U" + (DateTime.Now.Ticks / 100000 % 1000000000 + offset).ToString();
        }

        private static string CurrentDate()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private static void CheckRequestError(JObject data)
        {
            string code = data.Value<string>("rsp_code");
            string message = data.Value<string>("rsp_message");
            if (code != "A0000")
                throw new ArgumentException(code + ": " + message);
        }

        private static async Task<string> Get(string url, string accessToken)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            if (accessToken != null)
                request.Headers.Add("Authorization", "Bearer " + accessToken);

            string result;
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = await reader.ReadToEndAsync();
            }

            return result;
        }

        private static async Task<string> Post(string url, string data)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Method = "POST";

            using (Stream stream = await request.GetRequestStreamAsync())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(data);
                await writer.FlushAsync();
            }

            string result;
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = await reader.ReadToEndAsync();
            }

            return result;
        }

        public static Dictionary<string, string> ParseParameter(string query)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            foreach (string ent in query.Substring(1).Split('&'))
            {
                string[] token = ent.Split('=');
                param.Add(token[0], HttpUtility.UrlDecode(token[1]));
            }
            return param;
        }

        public static string GenerateRandom32()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 32; i++)
            {
                int value = Rand.Next(0, 62);
                if (value < 10)
                    sb.Append((char)('0' + value));
                else if (value < 36)
                    sb.Append((char)('a' + value - 10));
                else
                    sb.Append((char)('A' + value - 36));
            }
            return sb.ToString();
        }


        #endregion
    }
}
