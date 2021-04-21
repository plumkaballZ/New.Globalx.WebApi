using System;
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
        public string CreateUserLogin(UserLogin userLogin)
        {
            var paramDic = new Dictionary<string, object>();
            var loginUid = Guid.NewGuid();
            paramDic.Add("@email", userLogin.Email);
            paramDic.Add("@pw", userLogin.Password);
            paramDic.Add("@mobile", userLogin.Phone);
            paramDic.Add("@uid", loginUid.ToString());
            paramDic.Add("@ip", userLogin.Ip);

            ExecuteSp("xUserLogin_Create", paramDic);

            return loginUid.ToString();
        }
        public bool CreateUser(string loginUid)
        {
            var paramDic = new Dictionary<string, object>
            {
                {"@loginUid", loginUid}, {"@userUid", Guid.NewGuid().ToString()}
            };
            ExecuteSp("xUser_Create", paramDic);
            return true;
        }
        public bool Update(User user)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>
            {
                {"@userUid", user.Uid}, {"@userId", user.Id}
            };

            ExecuteSp("xUser_Update", paramDic);

            return true;
        }
    }
}
