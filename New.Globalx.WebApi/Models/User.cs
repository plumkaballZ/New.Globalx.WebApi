using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace New.Globalx.WebApi.Models
{
    public class User
    {
        public string Uid { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Created_At { get; set; }
        public string Updated_At { get; set; }
        public string Bill_Address_Id { get; set; }
        public string Ship_Address_Id { get; set; }
        public int Lvl { get; set; }
    }
}
