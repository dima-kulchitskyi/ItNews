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

        public Task<IList<Comment>> GetArticleComments(string id, int itemsCount, int commentPage)
        {
            return provider.GetArticleCommentsPage(id, itemsCount, commentPage);
        }
        public Task<int> GetArticleCommentsCount(string id)
        {
            return provider.GetArticleCommentsCount(id);
        }
        public async Task UpdateComment(Comment comment, string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));

            var oldComment = await provider.Get(comment.Id) ?? throw new ArgumentException("Comment does not exists"); ;

            var author = await userManager.GetById(authorId) ?? throw new ArgumentException($"User does not exists");

            if (oldComment.Author.Id != authorId)
                throw new InvalidOperationException($"Comment is not owned by user with {nameof(authorId)}");

            comment.Date = DateTime.Now;
            comment.Author = author;

            using (var uow = provider.GetUnitOfWork())
            {
                uow.BeginTransaction();
                await provider.SaveOrUpdate(comment);
                uow.Commit();
            }
        }

        public async Task DeleteComment(Comment comment, string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));

            using (var uow = provider.GetUnitOfWork())
            {
                uow.BeginTransaction();
                await provider.Delete(comment);
                uow.Commit();
            }
        }
        public Task<Comment> GetComment(string id)
        {
            return provider.Get(id);
        }
        public Task<int> GetCount()
        {
            return provider.GetCount();
        }
    }
}
