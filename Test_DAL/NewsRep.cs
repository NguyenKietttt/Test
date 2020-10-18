using Ncov_Common.Req;
using Ncovi_Common.DAL;
using Ncovi_Common.Rsp;
using System;
using System.Collections.Generic;
using System.Linq;
using Test_DAL.Models;

namespace Ncov_DAL
{
    public class NewsRep : GenericRep<NcovContext, News>
    {
        #region Methods
        public SingleRsp AddNews(List<News> listNews)
        {
            var res = new SingleRsp();

            using (var context = new NcovContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.AddRange(listNews);

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

        public List<NewsNoIDReq> GetListNews()
        {
            using (var context = new NcovContext())
            {
                var res = context.News.Select(p => new NewsNoIDReq
                {
                    Date = p.Date.ToString().Substring(0, 10),
                    Picture = p.Picture,
                    Link = p.Link,
                    Title = p.Title,
                    Description = p.Description
                }).OrderByDescending(p => p.Date).ToList();

                return res;
            }
        }
        #endregion
    }
}
