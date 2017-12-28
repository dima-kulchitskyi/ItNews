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
        protected readonly int defaultItemsOnPageCount =  int.Parse(WebConfigurationManager.AppSettings["NewsListItemsOnPageCount"]);
        protected readonly int articleTextPreviewLength = int.Parse(WebConfigurationManager.AppSettings["ArticleTextPreviewLength"]);

        public NewsController(ArticleManager articleManager)
        {
            this.articleManager = articleManager;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int itemsCount = 4)
        {
            if (itemsCount <= 0)
                itemsCount = defaultItemsOnPageCount;

            var articles = await articleManager.GetPagesAsync(itemsCount, page - 1, true);
            
            var model = new ArticlesList();
            model.PageCount = Convert.ToInt32(Math.Ceiling(await articleManager.GetCountAsync() / (double)itemsCount));
            model.Articles = articles.Select(it => 
            new ArticlesListPageItem
            {
                Title = it.Title,
                UrlPath = it.Id,
                Author = "",//it.Author.UserName,
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

            var article = await articleManager.GetAsync(id);

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