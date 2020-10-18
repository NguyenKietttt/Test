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
    public class PatientSvc : GenericSvc<PatientRep, Patients>
    {
        public SingleRsp AddPatients(List<PatientReq> listPatientsReq)
        {
            var res = new SingleRsp();

            List<Patients> listPatients = new List<Patients>();

            listPatientsReq.ForEach(p => listPatients.Add(new Patients
            {
                PatientId = p.PatientId,
                Age = p.Age,
                Sex = p.Sex,
                CityName = p.CityName,
                Status = p.Status,
                CountryId = p.CountryId
            }));

            res = _rep.AddPatients(listPatients);

            return res;
        }

        public SingleRsp UpdatePatients(List<PatientReq> listPatientsReq)
        {
            var res = new SingleRsp();

            List<Patients> listPatients = new List<Patients>();

            listPatientsReq.ForEach(p => listPatients.Add(new Patients
            {
                PatientId = p.PatientId,
                Age = p.Age,
                Sex = p.Sex,
                CityName = p.CityName,
                Status = p.Status,
                CountryId = p.CountryId
            }));

            res = _rep.UpdatePatients(listPatients);

            return res;
        }

        public SingleRsp GetListPatients()
        {
            var res = new SingleRsp();

            var countries = _rep.GetListPatients();
            res.Data = countries;

            return res;
        }

        public void CheckPatients()
        {
            var res = new SingleRsp();

            List<Patients> patients = _rep.GetAllPatients();
            List<PatientReq> patientReqs = GetPatient_FromWeb();

            if (patients.Count != 0)
            {
                var hashedIds = new HashSet<string>(patients.Select(p => p.PatientId.Trim()));
                var joinList = patientReqs.Where(p => hashedIds.Contains(p.PatientId)).ToList();
                var distinctList = patientReqs.Except(joinList).ToList();

                UpdatePatients(joinList);
                AddPatients(distinctList);
            }
            else
                AddPatients(patientReqs);
        }

        private List<PatientReq> GetPatient_FromWeb()
        {
            string htmlString = GetData.Instance.GetData_FromWeb("https://ncov.moh.gov.vn");

            List<PatientReq> listPatients = new List<PatientReq>();
            string xpath = "/html/body/div[1]/div/div/div/div/div[2]/div/div/section[2]/div/div[2]/table/tbody/tr";

            HtmlDocument htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(htmlString);

            var nodes = htmlDocument.DocumentNode.SelectNodes(xpath);

            nodes.ToList().ForEach(p => listPatients.Add(new PatientReq
            {
                PatientId = p.SelectSingleNode("td[1]").InnerText,
                Age = Convert.ToInt32(p.SelectSingleNode("td[2]").InnerText),
                Sex = p.SelectSingleNode("td[3]").InnerText,
                CityName = p.SelectSingleNode("td[4]").InnerText,
                Status = p.SelectSingleNode("td[5]").InnerText,
            }));

            foreach (var i in nodes)
            {
                string key = i.SelectSingleNode("td[6]").InnerText;
                foreach (var j in listPatients)
                {
                    string value;
                    if (j.CountryId == null)
                    {
                        if (GetData.Instance.CodeCountries.TryGetValue(key, out value))
                        {
                            j.CountryId = value;
                            break;
                        }
                    }
                }
            }
            return listPatients;
        }
    }
}
