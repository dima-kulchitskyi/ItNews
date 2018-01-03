using ItNews.Business.Managers;
using ItNews.Mvc.ViewModels.News;
using Ninject;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ItNews.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleManager articleManager;
        private readonly int defaultItemsOnPageCount = int.Parse(WebConfigurationManager.AppSettings["NewsListItemsOnPageCount"]);
        private readonly int articleTextPreviewLength = int.Parse(WebConfigurationManager.AppSettings["ArticleTextPreviewLength"]);

        public ArticleController(ArticleManager articleManager)
        {
            this.articleManager = articleManager;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int itemsCount = 4)
        {
            if (itemsCount <= 0)
                itemsCount = defaultItemsOnPageCount;

            var articles = await articleManager.GetPage(itemsCount, page - 1, true);

            var model = new ArticlesList();
            model.PageCount = Convert.ToInt32(Math.Ceiling(await articleManager.GetCount() / (double)itemsCount));
            model.Articles = articles.Select(it =>
            new ArticlesListPageItem
            {
                Title = it.Title,
                UrlPath = it.Id,
                Author = it.Author.UserName,
                ImagePath = it.ImagePath,
                Date = it.Date,
                TextPreview = it.Text.Substring(0, articleTextPreviewLength > it.Text.Length ? it.Text.Length : articleTextPreviewLength)
            }).ToList();

            model.PageSize = itemsCount;
            model.PageNumber = page;

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