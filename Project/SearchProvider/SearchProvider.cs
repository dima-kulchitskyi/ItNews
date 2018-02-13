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
        private static readonly Dictionary<string, FSDirectory> directories = new Dictionary<string, FSDirectory>();

        protected readonly Version luceneVersion = Version.LUCENE_30;

        protected readonly int searchResultsDefaultLimit = int.Parse(ConfigurationManager.AppSettings["SearchResultsDefaultLimit"]);

        protected readonly string directoryPath;

        public SearchProvider()
        {
            directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    ConfigurationManager.AppSettings["SearchProviderFolderName"],
                    ApplicationVariables.DataSourceProviderType,
                    typeof(T).FullName);
        }

        protected FSDirectory Directory
        {
            get
            {
                lock (directories)
                {
                    if (directories.ContainsKey(directoryPath))
                        return directories[directoryPath];

                    var dir = FSDirectory.Open(new DirectoryInfo(directoryPath));

                    if (IndexWriter.IsLocked(dir))
                        IndexWriter.Unlock(dir);

                    var lockFilePath = Path.Combine(directoryPath, "write.lock");

                    if (File.Exists(lockFilePath))
                        File.Delete(lockFilePath);

                    directories.Add(directoryPath, dir);

                    return dir;
                }
            }
        }

        protected abstract string[] SearchFields { get; }

        protected abstract Document MapInstanceToDocument(T instance);

        protected Analyzer GetAnalyzer()
        {
            return new StandardAnalyzer(luceneVersion);
        }

        protected IndexWriter GetIndexWriter()
        {
            return new IndexWriter(Directory, GetAnalyzer(), IndexWriter.MaxFieldLength.UNLIMITED);
        }

        protected Query ParseQuery(string searchQuery, QueryParser parser)
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
                    var searchQuery = new TermQuery(new Term(nameof(instance.Id), instance.Id));
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

        public string SearchOne(string searchQuery, string searchField = "")
        {
            return Search(searchQuery, 1, searchField).FirstOrDefault();
        }

        public IEnumerable<string> Search(string searchQuery, int maxResults = 0, string searchField = "")
        {
            IEnumerable<string> results = new List<string>();

            if (string.IsNullOrEmpty(searchQuery)) return results;

            searchQuery = string.Join(" ", searchQuery.Trim().Replace("-", " ").Split(' ')
             .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*"));

            if (string.IsNullOrWhiteSpace(searchQuery)) return results;

            using (var searcher = new IndexSearcher(Directory, true))
            {
                var hitsLimit = maxResults <= 0 ? searchResultsDefaultLimit : maxResults;
                var analyzer = GetAnalyzer();
                QueryParser parser;

                if (!string.IsNullOrEmpty(searchField))
                    parser = new QueryParser(luceneVersion, searchField, analyzer);
                else
                    parser = new MultiFieldQueryParser(luceneVersion, SearchFields, analyzer);

                var query = ParseQuery(searchQuery, parser);
                var hits = searcher.Search(query, null, hitsLimit, Sort.RELEVANCE).ScoreDocs;

                results = hits.Select(hit => searcher.Doc(hit.Doc).Get("Id")).ToList();

                analyzer.Close();
            }

            return results;
        }

        public void Clear(IEnumerable<string> ids)
        {
            using (var writer = GetIndexWriter())
            {
                foreach (var id in ids)
                {
                    var searchQuery = new TermQuery(new Term("Id", id));
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
