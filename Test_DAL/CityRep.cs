using Ncov_Common.Req;
using Ncovi_Common.DAL;
using Ncovi_Common.Rsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Test_DAL.Models;

namespace Ncov_DAL
{
    public class CityRep : GenericRep<NcovContext, Cities>
    {
        #region Methods
        public SingleRsp AddCities(List<Cities> listCities)
        {
            var res = new SingleRsp();

            using (var context = new NcovContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.AddRange(listCities);

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

        public SingleRsp UpdateCities(List<Cities> listCities)
        {
            var res = new SingleRsp();

            using (var context = new NcovContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.UpdateRange(listCities);

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

        public List<Cities> GetAllCities()
        {
            using (var context = new NcovContext())
            {
                var res = context.Cities.ToList();

                return res;
            }
        }
        #endregion
    }
}
