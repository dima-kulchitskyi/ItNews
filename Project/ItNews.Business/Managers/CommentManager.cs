using System;
using ItNews.Business.Entities;
using ItNews.Business.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Managers
{
    public class CommentManager : Manager<Comment>
    {
        private IUserProvider userProvider;
        private IArticleProvider articleProvider;
        private ICommentProvider commentProvider;
        public CommentManager(ICommentProvider provider, IUnitOfWorkFactory unitOfWorkFactory, IUserProvider userProvider, IArticleProvider articleProvider)
            : base(provider, unitOfWorkFactory)
        {
            commentProvider = provider;
            this.userProvider = userProvider;
            this.articleProvider = articleProvider;
        }

        public async Task CreateComment(Comment comment, string authorId, string articleId)
        {
            using (var uow = unitOfWorkFactory.GetUnitOfWork())
            {
                var user = await userProvider.Get(authorId);
                var article = await articleProvider.Get(articleId);
                comment.Author = user ?? throw new InvalidOperationException("No user with given id");
                comment.Article = article ?? throw new InvalidOperationException("No exist article with this ID");
                comment.Date = DateTime.Now;
                uow.BeginTransaction();
                await commentProvider.SaveOrUpdate(comment);
                uow.Commit();
            }
        }

        public Task<IList<Comment>> GetArticleComments(string id)
        {
            return commentProvider.GetArticleComments(id);
        }
        public async Task UpdateComment(Comment comment, string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));

            var oldComment = await commentProvider.Get(comment.Id) ?? throw new ArgumentException("Comment does not exists"); ;

            var author = await userProvider.Get(authorId) ?? throw new ArgumentException($"User with {nameof(authorId)} does not exists");

            if (oldComment.Author.Id != authorId)
                throw new InvalidOperationException($"Comment is not owned by user with {nameof(authorId)}");

            comment.Date = DateTime.Now;
            comment.Author = author;

            using (var uow = unitOfWorkFactory.GetUnitOfWork())
            {
                uow.BeginTransaction();
                await commentProvider.SaveOrUpdate(comment);
                uow.Commit();
            }
        }

        public async Task DeleteComment(Comment comment, string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));

            using (var uow = unitOfWorkFactory.GetUnitOfWork())
            {
                uow.BeginTransaction();
                await commentProvider.Delete(comment);
                uow.Commit();
            }
        }
        public Task<Comment> GetComment(string id)
        {
            return commentProvider.Get(id);
        }
    }
}
