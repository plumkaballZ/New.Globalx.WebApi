using System;
using System.Collections.Generic;
using System.Linq;
using New.Globalx.WebApi.Models;

namespace New.Globalx.WebApi.Repos
{
    public class AddressRepo : BaseRepo
    {
        public Address Get(string uid)
        {
            var paramDic = new Dictionary<string, object> { { "@uid", uid } };

            return GetSingle<Address>("xAddress_Get_v1", paramDic);
        }
        public List<Address> GetAll(string email)
        {
            var paramDic = new Dictionary<string, object> { { "@email", email } };

            return GetList<Address>("xAddress_GetAll_v1", paramDic).ToList();
        }
        public Address Create(Address address)
        {
            var paramDic = new Dictionary<string, object>();

            var adrUid = Guid.NewGuid();

            paramDic.Add("@address1", address.Address1);
            paramDic.Add("@address2", address.Address2);
            paramDic.Add("@city", address.City);
            paramDic.Add("@zipcode", address.Zipcode);
            paramDic.Add("@firstName", address.Firstname);
            paramDic.Add("@lastName", address.Lastname);
            paramDic.Add("@phone", address.Phone);
            paramDic.Add("@email", address.Email);
            paramDic.Add("@uid", adrUid);
            paramDic.Add("@_ip", address.Ip);
            paramDic.Add("@countryId", address.CountryId);
            paramDic.Add("@stateId", address.StateId);

            ExecuteSp("xAddress_Create_v1", paramDic);

            address.Uid = adrUid.ToString();

            return address;
        }
        public bool Delete(string uid)
        {
            var paramDic = new Dictionary<string, object> { { "@uid", uid } };
            ExecuteSp("xAddress_Delete", paramDic);
            return true;
        }
      
        public Address GetPickedServicePoint(string orderUid)
        {
            var paramDic = new Dictionary<string, object> { { "@orderUid", orderUid } };

            return GetSingle<Address>("PickedServicePoint_Get", paramDic);
        }

    }
}
