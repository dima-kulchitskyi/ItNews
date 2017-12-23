using ItNews.Business.Managers;
using ItNews.Mvc.ViewModels.News;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ItNews.Controllers
{
    public class NewsController : Controller
    {
        protected ArticleManager articleManager;
        protected readonly int DefaultItemsOnPageCount =  int.Parse(WebConfigurationManager.AppSettings["NewsListItemsOnPageCount"]);

        public NewsController(ArticleManager articleManager)
        {
            this.articleManager = articleManager;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int itemsCount = 2)
        {
            if (itemsCount <= 0)
                itemsCount = DefaultItemsOnPageCount;
            var articles = await articleManager.GetPage(itemsCount, page, true);
            
            //TODO: use entity mapper
            var model = articles.Select(it => new ArticlesListPageItem
            {
                Title = it.Title,
                UrlPath = it.Id,
                Author = it.Author.UserName,
                ImagePath = it.ImagePath,
                Date = it.Date,
                Text = it.Text             
            }).ToList();
            int nextPage = page++;
            return View(model);
        }
    }
}