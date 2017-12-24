using ItNews.Business.Entities;
using ItNews.Mvc.ViewModels.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.ViewModels.News
{
    public class ArticlesList
    {
        public IList<ArticlesListPageItem> Articles { set; get; }
        public int PageNumber { set; get; }
        public int PageSize { set; get; }
        public bool NextAvailable { set; get; }
        public bool PrevAvailable { set; get; }
    }
}
