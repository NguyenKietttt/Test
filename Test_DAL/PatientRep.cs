using Ncovi_Common.DAL;
using Ncovi_Common.Rsp;
using System;
using System.Collections.Generic;
using System.Linq;
using Test_DAL.Models;

namespace Ncov_DAL
{
    public class PatientRep : GenericRep<NcovContext, Patients>
    {
        #region Methods
        public SingleRsp AddPatients(List<Patients> listPatients)
        {
            var res = new SingleRsp();

            using (var context = new NcovContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.AddRange(listPatients);

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

        public SingleRsp UpdatePatients(List<Patients> listPatients)
        {
            var res = new SingleRsp();

            using (var context = new NcovContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.UpdateRange(listPatients);

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

        public List<Patients> GetAllPatients()
        {
            using (var context = new NcovContext())
            {
                var res = context.Patients.ToList();

                return res;
            }
        }

        public object GetListPatients()
        {
            using (var context = new NcovContext())
            {
                var res = context.Patients.Select(p => new
                {
                    PatientId = p.PatientId.Trim(),
                    Age = p.Age,
                    Sex = p.Sex,
                    Status = p.Status,
                    CityName = p.CityName
                }).OrderBy(p => p.Status).ToList();

                return res;
            }
        }
        #endregion
    }
}
