using System;
using ItNews.Business.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Providers
{
    public interface ICommentProvider : IProvider<Comment>
    {
        Task<IList<Comment>> GetArticleCommentsPage(string ArticleId, int itemsCount, int commentPage);
        Task<int> GetArticleCommentsCount(string ArticleId);
    }
}
