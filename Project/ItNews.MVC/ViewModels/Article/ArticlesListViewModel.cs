using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.ViewModels.Article
{
    public class ArticlesListViewModel
    {
        public IList<ArticlesListPageItem> Articles { set; get; }
        public int PageNumber { set; get; }
        public int PageSize { set; get; }
        public int PageCount { set; get; }
    }
}
