using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItNews.Business.Providers;

namespace ItNews.Business.Managers
{
    public class ArticleManager : Manager<Article, IArticleProvider>
    {
        private AppUserManager userManager;

        public ArticleManager(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            userManager = dependencyResolver.Resolve<AppUserManager>();
        }

        public Task<IList<Article>> GetListSegmentAsync(int count, DateTime startDate)
        {
            return Provider.GetListSegment(count, startDate);
        }

        public Task<IList<Article>> GetPage(int count, int pageNumber, bool newFirst)
        {
            return Provider.GetPage(count, pageNumber);
        }

        public Task<Article> GetArticle(string id)
        {
            return Provider.Get(id);
        }

        public async Task CreateArticle(Article article, string authorId)
        {
            using (var uow = Provider.GetUnitOfWork())
            {
                var user = await userManager.GetById(authorId);

                article.Author = user ?? throw new InvalidOperationException("No user with given id");
                article.Date = DateTime.Now;

                uow.BeginTransaction();
                await Provider.SaveOrUpdate(article);
                uow.Commit();
            }
        }

        public Task<int> GetCount()
        {
            return Provider.GetCount();
        }

        public async Task<bool> IsOwnedBy(string articleId, string userId)
        {
            if (string.IsNullOrEmpty(articleId))
                throw new ArgumentNullException(nameof(articleId));

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var article = await Provider.Get(articleId);

            return (article?.Author.Id == userId);
        }

        public async Task UpdateArticle(Article article, string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));

            using (var uow = Provider.GetUnitOfWork())
            {
                var oldArticle = await Provider.Get(article.Id) ?? throw new ArgumentException("Article with given id does not exists"); 

                var author = await userManager.GetById(authorId) ?? throw new ArgumentException("User with given id does not exists");

                if (oldArticle.Author.Id != authorId)
                    throw new InvalidOperationException("Article is not owned by current user");

                article.Date = DateTime.Now;
                article.Author = author;

                uow.BeginTransaction();
                await Provider.SaveOrUpdate(article);
                uow.Commit();
            }
        }

        public async Task DeleteArticle(Article article, string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));

            using (var uow = Provider.GetUnitOfWork())
            {
                uow.BeginTransaction();
                await Provider.DeleteAsync(article);
                uow.Commit();
            }
        }
    }
}