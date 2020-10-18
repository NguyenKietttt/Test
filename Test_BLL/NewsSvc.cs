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
    public class NewsSvc : GenericSvc<NewsRep, News>
    {
        public SingleRsp AddNews()
        {
            var res = new SingleRsp();

            List<NewsReq> listNewsReq = CheckNews();
            List<News> listNews = new List<News>();

            listNewsReq.ForEach(p => listNews.Add(new News
            {
                NewId = p.NewId,
                Date = p.Date,
                Picture = p.Picture,
                Link = p.Link,
                Title = p.Title,
                Description = p.Description
            }));

            res = _rep.AddNews(listNews);

            return res;
        }

        public SingleRsp GetListNews()
        {
            var res = new SingleRsp();

            var news = _rep.GetListNews();
            res.Data = news;

            return res;
        }

        public object GetNewsPage(string keyWord, int page, int size)
        {
            var temp = _rep.GetListNews();

            var offSet = (page - 1) * size;
            var total = temp.Count();
            int totalPage = (total % size) == 0 ? (int)(total / size) : (int)((total / size) + 1);
            var data = temp.OrderByDescending(p => p.Date).Skip(offSet).Take(size).ToList();

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

        private List<NewsReq> GetNews_FromWeb()
        {
            List<NewsReq> listNewsReq = new List<NewsReq>();

            for (int newsPageCount = 1; newsPageCount <= 5; newsPageCount++)
            {
                if (newsPageCount == 1)
                {
                    string htmlString = GetData.Instance.GetData_FromWeb("https://tuoitre.vn/phong-chong-covid-19-e583.htm");

                    string xpath = "/html/body/div[1]/div/section/div/div[2]/div/div[1]/div/ul/li";

                    ParseNews_To_NewsReq(htmlString, xpath, listNewsReq);
                }
                else
                {
                    string htmlString = GetData.Instance.GetData_FromWeb("https://tuoitre.vn/timeline-thread/583/trang-" + newsPageCount.ToString() + ".htm");

                    string xpath = "//*[contains(@class, 'news-item')]";

                    ParseNews_To_NewsReq(htmlString, xpath, listNewsReq);
                }

            }


            return listNewsReq;
        }

        private List<NewsReq> CheckNews()
        {
            List<NewsReq> temp = GetNews_FromWeb();

            var hashedIds = new HashSet<string>(All.Select(p => p.Link));
            var joinList = temp.Where(p => hashedIds.Contains(p.Link)).ToList();
            var distinctList = temp.OrderBy(p => p.Date).Except(joinList).ToList();

            int newsCount = All.Max(p => p.NewId) + 1;

            for (int i = 0; i < distinctList.Count; i++)
            {
                distinctList[i].NewId = newsCount;
                newsCount++;
            }

            return distinctList;
        }

        private void ParseNews_To_NewsReq(string url, string xpath, List<NewsReq> listNewsReq)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(url);
            var nodes = htmlDocument.DocumentNode.SelectNodes(xpath).ToList();

            nodes.ForEach(p => listNewsReq.Add(new NewsReq
            {
                Date = DateTime.ParseExact(p.SelectSingleNode("div[1]/span[2]").InnerText + "/" + DateTime.Now.Year.ToString(), "dd/MM/yyyy", null),
                Picture = p.SelectSingleNode("a[1]/img").GetAttributeValue("src", "Nothing"),
                Link = "https://tuoitre.vn" + p.SelectSingleNode("div[2]/h3/a").GetAttributeValue("href", "Nothing"),
                Title = p.SelectSingleNode("div[2]/h3/a").InnerText,
                Description = p.SelectSingleNode("div[2]/p").InnerText
            }));
        }
    }
}
