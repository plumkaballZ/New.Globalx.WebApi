using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace New.Globalx.WebApi.Models
{
    public class UserLogin
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Password_confirmation { get; set; }
        public string Ip { get; set; }
        public string Newsletter { get; set; }
    }
}
