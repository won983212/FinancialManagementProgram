using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialManagementProgram.kftcAPI
{
    public class UserAccessToken
    {
        public string AccessToken { get; }
        public DateTime ExpiresAt { get; }
        public string RefreshToken { get; }
        public string UserSeqNo { get; }

        public UserAccessToken(BinaryReader reader)
        {
            AccessToken = reader.ReadString();
            ExpiresAt = new DateTime(reader.ReadInt64());
            RefreshToken = reader.ReadString();
            UserSeqNo = reader.ReadString();
        }

        public UserAccessToken(JObject obj)
        {
            AccessToken = obj.Value<string>("access_token");
            ExpiresAt = DateTime.Now.AddSeconds(obj.Value<long>("expires_in"));
            RefreshToken = obj.Value<string>("refresh_token");
            UserSeqNo = obj.Value<string>("user_seq_no");
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AccessToken);
            writer.Write(ExpiresAt.Ticks);
            writer.Write(RefreshToken);
            writer.Write(UserSeqNo);
        }
    }
}
