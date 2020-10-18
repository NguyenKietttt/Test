using Ncovi_Common.DAL;
using Ncovi_Common.Req;
using Ncovi_Common.Rsp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ncov_Common.Req;
using Test_DAL.Models;

namespace Ncov_DAL
{
    public class CasesRep : GenericRep<NcovContext, Cases>
    {
        public SingleRsp AddCases(List<Cases> listCases)
        {
            var res = new SingleRsp();

            using (var context = new NcovContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.AddRange(listCases);

                        context.SaveChanges();
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        res.SetError(ex.StackTrace);
                    }
                }
            }

            return res;
        }

        public List<CasesNameReq> GetAllCases_Have_CountryName()
        {
            using (var context = new NcovContext())
            {
                var listCases = from co in context.Countries
                                join ca in context.Cases on co.CountryId equals ca.CountryId
                                orderby ca.Deaths descending
                                where ca.Date == context.Cases.Max(p => p.Date)
                                select new CasesNameReq
                                {
                                    CountryName = co.Name,
                                    Confirmed = ca.Confirmed,
                                    Active = ca.Active,
                                    Recovered = ca.Recovered,
                                    Deaths = ca.Deaths
                                };

                return listCases.ToList();
            }
        }

        public List<Cases> GetCase_By_CountryID(string countryID)
        {
            using (var context = new NcovContext())
            {
                var res = context.Cases.Where(p => p.CountryId == countryID).ToList();

                return res;
            }
        }

        public List<Cases> GetGlobalCases()
        {
            using (var context = new NcovContext())
            {
                var res = context.Cases.FromSqlRaw("exec Global").ToList();

                return res;
            }
        }

        public List<Cases> GetNewestCases_ByCountryID(string countryID)
        {
            using (var context = new NcovContext())
            {
                var res = context.Cases.FromSqlRaw("exec NewestCaseByID @CountryID = '" + countryID + "'").ToList();

                return res;
            }
        }
    }
}
