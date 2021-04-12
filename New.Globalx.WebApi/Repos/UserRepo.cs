using System.Collections.Generic;
using New.Globalx.WebApi.Models;

namespace New.Globalx.WebApi.Repos
{
    public class UserRepo : BaseRepo
    {
        public bool CheckUserLogin(string email)
        {
            var paramDic = new Dictionary<string, object> { { "@email", email } };

            return GetSingle<int>("xUserLogin_Check", paramDic) == 1;
        }
        public string LoginUser(string email, string pw)
        {
            var paramDic = new Dictionary<string, object> { { "@email", email }, { "@pw", pw } };

            return GetSingle<string>("xUserLogin_Login", paramDic);
        }
        public User GetUser(string refUid)
        {
            var paramDic = new Dictionary<string, object> { { "@loginUid", refUid } };

            return GetSingle<User>("xUser_GetSingle", paramDic);
        }
    }
}
