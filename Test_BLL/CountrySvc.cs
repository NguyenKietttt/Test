using Ncovi_Common.BLL;
using Ncovi_Common.Rsp;
using Ncov_DAL;
using System.Collections.Generic;
using Test_DAL.Models;

namespace Ncov_BLL
{
    public class CountrySvc : GenericSvc<CountryRep, Countries>
    {
        #region Methods
        public SingleRsp GetAllCountry()
        {
            var res = new SingleRsp();

            var countries = _rep.GetAllCountry();
            res.Data = countries;

            return res;
        }

        public HashSet<string> GetAllCountryID()
        {
            var res = _rep.GetAllCountryID();

            return res;
        }

        public SingleRsp GetAllCountry_Have_Cases()
        {
            var res = new SingleRsp();

            var listCountries = _rep.GetAllCountry_Have_Cases();
            res.Data = listCountries;

            return res;
        }
        #endregion
    }
}
