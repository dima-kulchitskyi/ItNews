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
using ItNews.Mvc.ModelBinders.Article;

namespace ItNews.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleManager articleManager;

        private CommentManager commentManager;


        public ArticleController(ArticleManager articleManager, CommentManager commentManager)
        {
            this.articleManager = articleManager;
            this.commentManager = commentManager;
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
                    ImageName = it.ImageName,
                    Date = it.Date,
                    TextPreview = (it.Text.Length > previewLength) ? it.Text.Substring(0, previewLength) + previewEnding : it.Text
                }).ToList(),
                PageSize = itemsCount,
                PageNumber = page
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Details(string id, [ModelBinder(typeof(PageNumberModelBinder))]int commentPage)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var article = await articleManager.GetArticle(id);

            if (article == null)
                return HttpNotFound();

            var comments = (await commentManager.GetArticleComments(id, 10, commentPage))
               .Select(comment => new CommentViewModel
               {
                   Author = comment.Author.UserName,
                   Id = comment.Id,
                   Date = comment.Date,
                   Text = comment.Text
               }).ToList();

            var model = new ArticleDetailsViewModel
            {
                Id = article.Id,
                Title = article.Title,
                AuthorName = article.Author.UserName,
                Date = article.Date.ToString("f"),
                Content = article.Text,
                ControlsAvailable = (User.Identity.IsAuthenticated && article.Author.Id == User.Identity.GetUserId()),
                Comments = comments,
                UserName = User.Identity.Name,
                commentPageCount = Convert.ToInt32(Math.Ceiling(await commentManager.GetArticleCommentsCount(id) / 10.0)),
                commentPageNumber = commentPage
            };

            if (!string.IsNullOrEmpty(article.ImageName))
            {
                model.HasImage = true;
                model.Image = Path.Combine(WebConfigurationManager.AppSettings["ImagesFolder"], article.ImageName);
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
                article.ImageName = fileName;
            }

            await articleManager.CreateArticle(article, User.Identity.GetUserId());

            return RedirectToAction("Details", new { id = article.Id });
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

            if (article.ImageName != null)
                model.OldImageName = Path.Combine(WebConfigurationManager.AppSettings["ImagesFolder"], article.ImageName);

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
                Text = model.Text,
                ImageName = article.ImageName
            };

            if (model.UploadedImage != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.UploadedImage.FileName);
                model.UploadedImage.SaveAs(Path.Combine(Server.MapPath(WebConfigurationManager.AppSettings["ImagesFolder"]), fileName));

                updatedArticle.ImageName = fileName;
            }

            await articleManager.UpdateArticle(updatedArticle, User.Identity.GetUserId());

            return RedirectToAction("Details", new { id = article.Id });
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
                ImagePath = article.ImageName,
                Title = article.Title,
                Text = article.Text,
                Date = article.Date.ToString("f")
            };

            if (!string.IsNullOrEmpty(article.ImageName))
            {
                model.ImagePath = Url.Content(Path.Combine(WebConfigurationManager.AppSettings["ImagesFolder"], article.ImageName));
            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateComment(CreateCommentViewModel model)
        {
            if (string.IsNullOrEmpty(model.ArticleId))
                return HttpNotFound();

            var article = await articleManager.GetArticle(model.ArticleId);
            if (article == null)
                return HttpNotFound();
            var comment = new Comment
            {
                Text = model.Text
            };

            await commentManager.CreateComment(comment, User.Identity.GetUserId(), model.ArticleId);

            return RedirectToAction("Details", new { id = model.ArticleId });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> DeleteComment(string id, string articleId)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var comment = await commentManager.GetComment(id);

            if (comment.Author.Id != User.Identity.GetUserId())
                return HttpNotFound();

            await commentManager.DeleteComment(comment, comment.Id);

            return RedirectToAction("Details", new { id = articleId });
        }
    }
}