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
    }
}
