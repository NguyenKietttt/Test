using Ncovi_Common.BLL;
using Ncovi_Common.Req;
using Ncovi_Common.Rsp;
using Ncov_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Ncov_Common.Req;
using Test_DAL.Models;

namespace Ncov_BLL
{
    public class CasesSvc : GenericSvc<CasesRep, Cases>
    {
        #region Methods
        public SingleRsp AddCases()
        {
            var res = new SingleRsp();

            List<Cases> listCases = new List<Cases>();
            List<CaseReqByCountry> listCaseReqByCountries = CheckCases();

            listCaseReqByCountries.ForEach(p => listCases.Add(new Cases
            {
                Date = p.Date,
                Confirmed = p.TotalConfirmed,
                Deaths = p.TotalDeaths,
                Recovered = p.TotalRecovered,
                Active = p.TotalConfirmed - (p.TotalDeaths + p.TotalRecovered),
                CountryId = p.CountryCode
            }));

            res = _rep.AddCases(listCases);

            return res;
        }

        public object GetGlobalCases()
        {
            var listGlobalCases = _rep.GetGlobalCases();

            var res = listGlobalCases
                .GroupBy(x => true)
                .Select(x => new
                {
                    TotalConfirmed = Convert.ToInt32(x.Sum(y => y.Confirmed)),
                    TotalActive = Convert.ToInt32(x.Sum(y => y.Active)),
                    TotalDeaths = Convert.ToInt32(x.Sum(y => y.Deaths)),
                    TotalRecovered = Convert.ToInt32(x.Sum(y => y.Recovered))
                });

            return res;
        }

        public object GetNewestCases_ByCountryID(string countryID)
        {
            var NewestCase = _rep.GetNewestCases_ByCountryID(countryID);

            var res = NewestCase.Select(p => new
            {
                CountryId = p.CountryId.Trim(),
                Confirmed = p.Confirmed,
                Deaths = p.Deaths,
                Recovered = p.Recovered,
                Active = p.Active
            });
            return res;
        }

        public object GetCasePages(string keyWord, int page, int size)
        {
            var temp = _rep.GetAllCases_Have_CountryName().Where(p => p.CountryName.Contains(keyWord));

            var offSet = (page - 1) * size;
            var total = temp.Count();
            int totalPage = (total % size) == 0 ? (int)(total / size) : (int)((total / size) + 1);
            var data = temp.OrderByDescending(p => p.Confirmed).Skip(offSet).Take(size).ToList();

            var res = new
            {
                Data = data,
                totalRecord = total,
                totalPages = totalPage,
                Page = page,
                Size = size
            };

            return res;
        }

        public SingleRsp GetCase_By_CountryID(string countryID)
        {
            var res = new SingleRsp();
            List<CasesReqByID> temp = new List<CasesReqByID>();

            var casesArr = _rep.GetCase_By_CountryID(countryID).OrderByDescending(p => p.Date).ToArray();

            for (int i = 0; i < casesArr.Length; i++)
            {
                CasesReqByID casesReqByID = new CasesReqByID();
                if (i + 1  < casesArr.Length)
                {
                    casesReqByID.Date = casesArr[i].Date.ToString().Substring(0, 10).Trim();
                    casesReqByID.NewConfirmed = casesArr[i].Confirmed - casesArr[i + 1].Confirmed;
                    casesReqByID.NewRecovered = casesArr[i].Recovered - casesArr[i + 1].Recovered;

                    temp.Add(casesReqByID);
                }
            }

            res.Data = temp;

            return res;
        }

        public List<CaseReqByCountry> GetCase_ByCountry_FromWeb()
        {
            string url = "https://api.covid19api.com/summary";

            List<CaseReqByCountry> listCase = GetData.Instance.ConvertJson_ToClass<CaseReqRoot>(url).Countries;

            return listCase.ToList();
        }

        private List<CaseReqByCountry> CheckCases()
        {
            CountrySvc countrySvc = new CountrySvc();

            var listCountryID = countrySvc.GetAllCountryID();

            List<CaseReqByCountry> listCountriesFromWeb = GetCase_ByCountry_FromWeb();

            var filteredList = listCountriesFromWeb.Where(p => listCountryID.Contains(p.CountryCode)).ToList();

            return filteredList;
        }
        #endregion
    }
}
