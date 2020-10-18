using Ncovi_Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncov_Common.Req;
using Test_DAL.Models;

namespace Ncov_DAL
{
    public class CountryRep : GenericRep<NcovContext, Countries>
    {
        #region Methods
        public List<Countries> GetAllCountry()
        {
            using (var context = new NcovContext())
            {
                var res = context.Countries.ToList();

                return res;
            }
        }

        public HashSet<string> GetAllCountryID()
        {
            using (var context = new NcovContext())
            {
                var res = context.Countries.Select(p => p.CountryId).ToHashSet();

                return res;
            }
        }

        public List<CountryCasesReq> GetAllCountry_Have_Cases()
        {
            using (var context = new NcovContext())
            {
                var CountryCasesReq = from co in context.Countries
                                join ca in context.Cases on co.CountryId equals ca.CountryId
                                orderby ca.Deaths descending
                                where ca.Date == context.Cases.Max(p => p.Date)
                                select new CountryCasesReq
                                {
                                    CountryId = co.CountryId,
                                    CountryName = co.Name,
                                    Latitude = co.Latitude,
                                    Longitude = co.Longitude,
                                    Confirmed = ca.Confirmed,
                                    Active = ca.Active,
                                    Recovered = ca.Recovered,
                                    Deaths = ca.Deaths
                                };

                return CountryCasesReq.ToList();
            }
        }
        #endregion
    }
}
