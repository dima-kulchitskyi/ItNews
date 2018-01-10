using ItNews.Business.Entities;
using ItNews.Business.Managers;
using ItNews.Mvc.ViewModels.Article;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using ItNews.Mvc.ModelBinders.Article;
using ItNews.Mvc.ModelBinders.Article;

namespace ItNews.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleManager articleManager;


        public ArticleController(ArticleManager articleManager)
        {
            this.articleManager = articleManager;
        }

        [HttpGet]
        public async Task<ActionResult> Index(
            [ModelBinder(typeof(PageNumberModelBinder))]int page, 
            [ModelBinder(typeof(ItemsOnPageCountModelBinder))]int itemsCount)
        {
            var articles = await articleManager.GetPage(itemsCount, page, true);

            var previewLength = int.Parse(WebConfigurationManager.AppSettings["ArticleTextPreviewLength"]);
            var previewEnding = WebConfigurationManager.AppSettings["ArticleTextPreviewEnding"];

            var model = new ArticlesListViewModel
            {
                PageCount = Convert.ToInt32(Math.Ceiling(await articleManager.GetCount() / (double)itemsCount)),
                Articles = articles.Select(it => new ArticlesListPageItem
                {
                    Title = it.Title,
                    Id = it.Id,
                    Author = it.Author.UserName,
                    ImagePath = it.ImagePath,
                    Date = it.Date,
                    TextPreview = (it.Text.Length > previewLength) ? it.Text.Substring(0, previewLength) + previewEnding : it.Text
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
                Date = article.Date.ToString("f"),
                Content = article.Text,
                CanEdit = (User.Identity.IsAuthenticated && article.Author.Id == User.Identity.GetUserId())
            };

            if (!string.IsNullOrEmpty(article.ImagePath))
            {
                model.HasImage = true;
                model.Image = Path.Combine(WebConfigurationManager.AppSettings["ImagesFolder"], article.ImagePath);
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var article = new Article
            {
                Text = model.Text,
                Title = model.Title
            };

            if (model.Image != null && model.Image.ContentLength > 0)
            {
                var directory = Server.MapPath(WebConfigurationManager.AppSettings["ImagesFolder"]);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                model.Image.SaveAs(Path.Combine(directory, fileName));
                article.ImagePath = fileName;
            }

            await articleManager.CreateArticle(article, User.Identity.GetUserId());

            return RedirectToAction("Details", new { id = article.Id });
        }
    }
}