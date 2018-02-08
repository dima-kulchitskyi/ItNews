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
    public class ArticleProvider :  SearchProvider<Article>, IArticleSearchProvider
    {
        protected override Article MapDocumentToInstance(Document doc)
        {
            return new Article
            {
                Id = doc.Get(nameof(Article.Id)),
                Date = DateTime.Parse(doc.Get(nameof(Article.Date))),
                Title = doc.Get(nameof(Article.Title)),
                Text = doc.Get(nameof(Article.Text)),
                ImageName = doc.Get(nameof(Article.ImageName)),
                Author = new AppUser
                {
                    Id = doc.Get("AuthorId"),
                    UserName = doc.Get("Author")
                }
            };
        }

        protected override Document MapInstanceToDocument(Article instance)
        {
            var doc = new Document();

            doc.Add(new Field(nameof(Article.Id), instance.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(nameof(Article.Date), instance.Date.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(nameof(Article.ImageName), instance.ImageName ?? "", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("AuthorId", instance.Author.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));

            doc.Add(new Field("Author", instance.Author.UserName, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(nameof(Article.Title), instance.Title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(nameof(Article.Text), instance.Text, Field.Store.YES, Field.Index.ANALYZED));

            return doc;
        }
    }
}
