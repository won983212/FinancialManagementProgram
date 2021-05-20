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

        public static Task<UserAccessToken> GenerateToken()
        {
            TaskCompletionSource<UserAccessToken> tcs = new TaskCompletionSource<UserAccessToken>();
            new AuthWindow((param) => OnAuthCallback(param, tcs)).Show();
            return tcs.Task;
        }

        public static async Task GetUserInfo(UserAccessToken userToken)
        {
            string data = await Get(string.Format("{0}v2.0/user/me?user_seq_no={1}", Host, userToken.UserSeqNo), userToken.AccessToken);
            Console.WriteLine(data);
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

        public static string AuthorizeURL(AuthType authType)
        {
            return string.Format("{0}oauth/2.0/authorize?response_type=code&client_id={1}&redirect_uri=http://localhost/&scope=login inquiry&authorized_cert_yn=N&state={2}&auth_type={3}",
                Host, ClientID, State, (int)authType);
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
    }
}
