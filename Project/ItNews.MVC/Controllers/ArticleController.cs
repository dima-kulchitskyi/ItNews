using ItNews.Business.Managers;
using ItNews.Mvc.ViewModels.Article;
using Ninject;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ItNews.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleManager articleManager;

        private readonly int defaultItemsOnPageCount = int.Parse(WebConfigurationManager.AppSettings["NewsListItemsOnPageDefaultCount"]);

        private readonly int articleTextPreviewLength = int.Parse(WebConfigurationManager.AppSettings["ArticleTextPreviewLength"]);

        private readonly string ImagesFolderPath = WebConfigurationManager.AppSettings["IamgesFolder"];

        public ArticleController(ArticleManager articleManager)
        {
            this.articleManager = articleManager;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int itemsCount = 0)
        {
            if (itemsCount <= 0)
                itemsCount = defaultItemsOnPageCount;

            var articles = await articleManager.GetPage(itemsCount, page - 1, true);

            var model = new ArticlesListViewModel
            {
                PageCount = Convert.ToInt32(Math.Ceiling(await articleManager.GetCount() / (double)itemsCount)),
                Articles = articles.Select(it =>
                new ArticlesListPageItem
                {
                    Title = it.Title,
                    Id = it.Id,
                    Author = it.Author.UserName,
                    ImagePath = it.ImagePath,
                    Date = it.Date,
                    TextPreview = it.Text.Substring(0, articleTextPreviewLength > it.Text.Length ? it.Text.Length : articleTextPreviewLength)
                }).ToList(),
                PageSize = itemsCount,
                PageNumber = page
            };

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
                Image = Path.Combine(ImagesFolderPath, article.ImagePath),
                Date = article.Date
            };

            return View(model);
        }
    }
}