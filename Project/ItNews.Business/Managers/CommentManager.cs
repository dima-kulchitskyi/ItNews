using System;
using ItNews.Business.Entities;
using ItNews.Business.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Managers
{
    public class CommentManager : Manager<Comment, ICommentProvider>
    {
        private AppUserManager userManager;

        private ArticleManager articleManager;

        public CommentManager(ICommentProvider commentProvider, AppUserManager userManager, ArticleManager articleManager)
            : base(commentProvider)
        {
            this.userManager = userManager;
            this.articleManager = articleManager;
        }

        public async Task CreateComment(string commentText, string authorId, string articleId)
        {
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(commentText));

            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));

            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(articleId));

            using (var uow = provider.GetUnitOfWork())
            {
                var comment = new Comment
                {
                    Text = commentText,
                    Author = await userManager.GetById(authorId) ?? throw new InvalidOperationException("No user with given id"),
                    Article = await articleManager.GetById(articleId) ?? throw new InvalidOperationException("No article with given id"),
                    Date = DateTime.Now
                };

                uow.BeginTransaction();
                await provider.SaveOrUpdate(comment);
                uow.Commit();
            }
        }

        public Task<IList<Comment>> GetArticleComments(string articleId)
        {
            return provider.GetArticleComments(articleId);
        }
    }
}
