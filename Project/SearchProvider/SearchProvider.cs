using ItNews.Business;
using ItNews.Business.Entities;
using ItNews.Business.Search;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Version = Lucene.Net.Util.Version;

namespace ItNews.SearchProvider
{
    public abstract class SearchProvider<T> : ISearchProvider<T>
        where T : class, IEntity
    {
        protected const int SearchResultsLimit = 1000;

        private static readonly Dictionary<string, FSDirectory> directories = new Dictionary<string, FSDirectory>();

        private readonly Version luceneVersion = Version.LUCENE_30;

        protected FSDirectory Directory
        {
            get
            {
                lock (directories)
                {
                    var dirPath = DirectoryPath;

                    if (directories.ContainsKey(dirPath))
                        return directories[dirPath];

                    var dir = FSDirectory.Open(new DirectoryInfo(dirPath));

                    if (IndexWriter.IsLocked(dir))
                        IndexWriter.Unlock(dir);

                    var lockFilePath = Path.Combine(dirPath, "write.lock");

                    if (File.Exists(lockFilePath))
                        File.Delete(lockFilePath);

                    directories.Add(dirPath, dir);

                    return dir;
                }
            }
        }

        protected string DirectoryPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    ConfigurationManager.AppSettings["SearchProviderFolderName"],
                    DependencyResolver.Current.GetService<ApplicationVariables>().DataSourceProviderType,
                    typeof(T).FullName);
            }
        }

        private IEnumerable<T> MapToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapDocumentToInstance).ToList();
        }

        private IEnumerable<T> MapToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => MapDocumentToInstance(searcher.Doc(hit.Doc))).ToList();
        }

        protected abstract T MapDocumentToInstance(Document doc);

        protected abstract Document MapInstanceToDocument(T instance);

        protected Analyzer GetAnalyzer()
        {
            return new StandardAnalyzer(luceneVersion);
        }

        protected IndexWriter GetIndexWriter()
        {
            return new IndexWriter(Directory, GetAnalyzer(), IndexWriter.MaxFieldLength.UNLIMITED);
        }

        protected static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        public void AddOrUpdate(IEnumerable<T> instances)
        {
            using (var writer = GetIndexWriter())
            {
                foreach (var instance in instances)
                {
                    var searchQuery = new TermQuery(new Term("Id", instance.Id));
                    writer.DeleteDocuments(searchQuery);
                    writer.AddDocument(MapInstanceToDocument(instance));
                }

                writer.Analyzer.Close();
                //writer.Optimize();
            }
        }

        public void AddOrUpdate(T instance)
        {
            AddOrUpdate(new List<T> { instance });
        }

        public T SearchOne(string searchQuery, string searchField = "")
        {
            return Search(searchQuery, 1, searchField).FirstOrDefault();
        }

        public IEnumerable<T> Search(string searchQuery, int maxResults = 0, string searchField = "")
        {
            IEnumerable<T> results = new List<T>();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = string.Join(" ", searchQuery.Trim().Replace("-", " ").Split(' ')
                  .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*"));

                using (var searcher = new IndexSearcher(Directory, false))
                {
                    var hitsLimit = maxResults <= 0 ? SearchResultsLimit : maxResults;
                    var analyzer = GetAnalyzer();

                    if (!string.IsNullOrEmpty(searchField))
                    {
                        var parser = new QueryParser(luceneVersion, searchField, analyzer);
                        var query = ParseQuery(searchQuery, parser);
                        var hits = searcher.Search(query, hitsLimit).ScoreDocs;
                        results = MapToDataList(hits, searcher);
                    }
                    else
                    {
                        var parser = new MultiFieldQueryParser(luceneVersion,
                            typeof(T).GetProperties().Where(p => !p.GetAccessors().Any(a => !a.IsPublic)).Select(p => p.Name).ToArray(),
                            analyzer);

                        var query = ParseQuery(searchQuery, parser);
                        var hits = searcher.Search(query, null, hitsLimit, Sort.RELEVANCE).ScoreDocs;
                        results = MapToDataList(hits, searcher);
                    }
                    analyzer.Close();
                }
            }

            return results;
        }

        public void Clear(IEnumerable<string> ids)
        {
            using (var writer = GetIndexWriter())
            {
                foreach (var id in ids)
                {
                    var searchQuery = new TermQuery(new Term("Id", id.ToString()));
                    writer.DeleteDocuments(searchQuery);
                }

                writer.Analyzer.Close();
            }
        }

        public void Clear(string id)
        {
            Clear(new List<string> { id });
        }
    }
}
