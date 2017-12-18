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

        public NewsController(ArticleManager articleManager)
        {
            this.articleManager = articleManager;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var itemsOnPageCount = int.Parse(WebConfigurationManager.AppSettings["NewsListItemsOnPageCount"]);
            var page = await articleManager.GetListSegmentAsync(itemsOnPageCount, DateTime.MaxValue, true);

            //TODO: use entity mapper
            var model = page.Select(it => new ArticlesListPageItem
            {
                Title = it.Title,
                UrlPath = it.Id,
                Author = it.Author.UserName,
                ImagePath = it.ImagePath,
                Date = it.Date,
                Text = it.Text
            }).ToList();
            return View(model);
        }
    }
}