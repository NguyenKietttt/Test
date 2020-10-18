using HtmlAgilityPack;
using Ncov_Common.Req;
using Ncov_DAL;
using Ncovi_Common.BLL;
using Ncovi_Common.Rsp;
using System;
using System.Collections.Generic;
using System.Linq;
using Test_DAL.Models;

namespace Ncov_BLL
{
    public class CitySvc : GenericSvc<CityRep, Cities>
    {
        #region Methods
        public SingleRsp AddCities(List<CityReq> listCitiesReq)
        {
            var res = new SingleRsp();

            List<Cities> listCities = new List<Cities>();

            listCitiesReq.ForEach(p => listCities.Add(new Cities
            {
                CityName = p.CityName,
                TotalCase = p.TotalCase,
                Active = p.Active,
                Recovered = p.Recovered,
                Deaths = p.Deaths
            }));

            res = _rep.AddCities(listCities);

            return res;
        }

        public SingleRsp UpdateCities(List<CityReq> listCitiesReq)
        {
            var res = new SingleRsp();

            List<Cities> listCities = new List<Cities>();

            listCitiesReq.ForEach(p => listCities.Add(new Cities
            {
                CityName = p.CityName,
                TotalCase = p.TotalCase,
                Active = p.Active,
                Recovered = p.Recovered,
                Deaths = p.Deaths
            }));

            res = _rep.UpdateCities(listCities);

            return res;
        }

        public SingleRsp GetAllCities()
        {
            var res = new SingleRsp();

            var m = _rep.GetAllCities().OrderByDescending(p => p.TotalCase);
            res.Data = m;

            return res;
        }

        private List<CityReq> GetCase_ByCity_FromWeb()
        {
            string htmlString = GetData.Instance.GetData_FromWeb("https://ncov.moh.gov.vn");

            List<CityReq> listCaseByCity = new List<CityReq>();
            string xpath = "/html/body/div[1]/div/div/div/div/div[2]/div/div/section[2]/div/div[1]/table/tbody/tr";

            HtmlDocument htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(htmlString);

            var nodes = htmlDocument.DocumentNode.SelectNodes(xpath);

            nodes.ToList().ForEach(p => listCaseByCity.Add(new CityReq
            {
                CityName = p.SelectSingleNode("td[1]").InnerText,
                TotalCase = Convert.ToInt32(p.SelectSingleNode("td[2]").InnerText),
                Active = Convert.ToInt32(p.SelectSingleNode("td[3]").InnerText),
                Recovered = Convert.ToInt32(p.SelectSingleNode("td[4]").InnerText),
                Deaths = Convert.ToInt32(p.SelectSingleNode("td[5]").InnerText)
            }));

            return listCaseByCity;
        }

        public void CheckCities()
        {
            var res = new SingleRsp();

            List<Cities> cities = _rep.GetAllCities();
            List<CityReq> cityReqs = GetCase_ByCity_FromWeb();

            if (cities.Count != 0)
            {
                var hashedIds = new HashSet<string>(cities.Select(p => p.CityName.Trim()));
                var joinList = cityReqs.Where(p => hashedIds.Contains(p.CityName)).ToList();
                var distinctList = cityReqs.Except(joinList).ToList();

                UpdateCities(joinList);
                AddCities(distinctList);
            }
            else
                AddCities(cityReqs);
        }
        #endregion
    }
}
