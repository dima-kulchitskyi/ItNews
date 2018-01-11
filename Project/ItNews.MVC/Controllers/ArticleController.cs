using ItNews.Business.Entities;
using ItNews.Business.Managers;
using ItNews.Mvc.ViewModels.Article;
using ItNews.MVC.ViewModels.Article;
using Microsoft.AspNet.Identity;
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

        private readonly string ImagesFolderPath = WebConfigurationManager.AppSettings["ImagesFolder"];

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
                Date = article.Date.ToString("f"),
                Content = article.Text
            };

            if (!string.IsNullOrEmpty(article.ImagePath))
            {
                model.HasImage = true;
                model.Image = Url.Content(Path.Combine(ImagesFolderPath, article.ImagePath));
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

            var item = new Article
            {
                Text = model.Text,
                Title = model.Title
            };

            if (model.Image != null && model.Image.ContentLength > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                model.Image.SaveAs(Path.Combine(Server.MapPath(Url.Content(ImagesFolderPath)), fileName));
                item.ImagePath = fileName;
            }

            await articleManager.CreateArticle(item, User.Identity.GetUserId());

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var article = await articleManager.GetArticle(id);

            if (article == null)
                return HttpNotFound();

            var model = new EditViewModel
            {
                Id = article.Id,
                Text = article.Text,
                Title = article.Title,
            };

            if (article.ImagePath != null)
                model.OldImagePath = Url.Content(Path.Combine(ImagesFolderPath, article.ImagePath));

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var article = await articleManager.GetArticle(model.Id);

            if (article.Author.Id != User.Identity.GetUserId())
                return HttpNotFound();

            var updatedArticle = new Article
            {
                Id = model.Id,
                Title = model.Title,
                Text = model.Text
            };

            if (model.UploadedImage != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.UploadedImage.FileName);
                model.UploadedImage.SaveAs(Path.Combine(Server.MapPath(Url.Content(ImagesFolderPath)), fileName));

                updatedArticle.ImagePath = fileName;
            }

            await articleManager.UpdateArticle(updatedArticle, User.Identity.GetUserId());

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmartion(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var article = await articleManager.GetArticle(id);

            if (article.Author.Id != User.Identity.GetUserId())
                return HttpNotFound();

            await articleManager.DeleteArticle(article, article.Id);

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var article = await articleManager.GetArticle(id);

            if (article == null)
                return HttpNotFound();

            var model = new DeleteViewModel
            {
                ImagePath = article.ImagePath,
                Title = article.Title,
                Text = article.Text,
                Date = article.Date.ToString("f")
            };

            if (!string.IsNullOrEmpty(article.ImagePath))
            {
                model.ImagePath = Url.Content(Path.Combine(ImagesFolderPath, article.ImagePath)); 
            }
            return View(model);
        }

    }
}