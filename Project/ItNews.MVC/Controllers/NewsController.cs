using ItNews.Business.Managers;
using ItNews.Mvc.ViewModels.News;
using ItNews.MVC.ViewModels.News;
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
            var articles = await articleManager.GetListSegmentAsync(itemsOnPageCount, DateTime.MaxValue, true);

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
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var article = await articleManager.GetArticle(id);

            if (article == null)
                return HttpNotFound();

            var model = new ArticleDetailsViewModel
            {
                Id = article.Id,
                Title = article.Title,
                AuthorName = article.Author.UserName,
                Image = article.ImagePath,
                Date = article.Date
            };

            return View(model);
        }
    }
}