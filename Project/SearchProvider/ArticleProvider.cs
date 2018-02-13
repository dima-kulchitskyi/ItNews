using ItNews.Business.Entities;
using ItNews.Business.Search;
using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.SearchProvider
{
    public class ArticleProvider : SearchProvider<Article>, IArticleSearchProvider
    {
        protected override string[] SearchFields { get; } = new string[]
        {
            "Title",
            "Text",
            "Author"
        };

        protected override Document MapInstanceToDocument(Article instance)
        {
            var doc = new Document();

            doc.Add(new Field("Id", instance.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Date", instance.Date.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("AuthorId", instance.Author.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));

            doc.Add(new Field("Author", instance.Author.UserName, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("Title", instance.Title, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("Text", instance.Text, Field.Store.NO, Field.Index.ANALYZED));

            return doc;
        }
    }
}
