namespace Sitecore.Spatial.GeoJson.Providers.Solr
{
    using System;
    using System.Linq;
    using Sitecore.Abstractions;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Diagnostics;
    using Sitecore.ContentSearch.Linq.Common;
    using Sitecore.ContentSearch.Linq.Indexing;
    using Sitecore.ContentSearch.Linq.Solr;
    using Sitecore.ContentSearch.Pipelines.QueryGlobalFilters;
    using Sitecore.ContentSearch.SearchTypes;
    using Sitecore.ContentSearch.Security;
    using Sitecore.ContentSearch.Utilities;

    public class SolrSearchContext : Sitecore.ContentSearch.SolrProvider.SolrSearchContext, IProviderSearchContext, IDisposable
    {
        private readonly IContentSearchConfigurationSettings settings;

        public SolrSearchContext(Sitecore.ContentSearch.SolrProvider.SolrSearchIndex index, SearchSecurityOptions options = SearchSecurityOptions.EnableSecurityCheck)
            : base(index, options)
        {
            this.settings = this.Index.Locator.GetInstance<IContentSearchConfigurationSettings>();
        }

        public override IQueryable<TItem> GetQueryable<TItem>(params IExecutionContext[] executionContexts)
        {
            var linqToSolrIndex = new Sitecore.Spatial.GeoJson.Providers.Solr.LinqToSolrIndex<TItem>(this, executionContexts);
            if (this.settings.EnableSearchDebug())
                ((IHasTraceWriter)linqToSolrIndex).TraceWriter = new LoggingTraceWriter(SearchLog.Log);

            var queryable = ((Index<TItem, SolrCompositeQuery>)linqToSolrIndex).GetQueryable();
            if (typeof(TItem).IsAssignableFrom(typeof(SearchResultItem)))
            {
                var queryGlobalFiltersArgs = new QueryGlobalFiltersArgs(queryable, typeof(TItem), executionContexts.ToList());
                this.Index.Locator.GetInstance<BaseCorePipelineManager>().Run("contentSearch.getGlobalLinqFilters", queryGlobalFiltersArgs);
                queryable = (IQueryable<TItem>)queryGlobalFiltersArgs.Query;
            }

            return queryable;
        }
    }
}