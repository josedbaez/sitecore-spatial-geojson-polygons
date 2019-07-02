namespace Sitecore.Spatial.GeoJson.Providers.Solr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Linq.Common;
    using Sitecore.ContentSearch.Linq.Parsing;
    using Sitecore.ContentSearch.Linq.Solr;
    using Sitecore.ContentSearch.SolrProvider;

    public class LinqToSolrIndex<TItem> : Sitecore.ContentSearch.SolrProvider.LinqToSolrIndex<TItem>
    {
        private readonly Sitecore.ContentSearch.Linq.Solr.SolrQueryOptimizer queryOptimizer;

        public LinqToSolrIndex(Sitecore.ContentSearch.SolrProvider.SolrSearchContext context, IExecutionContext executionContext)
            : base(context, executionContext)
        {
        }

        public LinqToSolrIndex(Sitecore.ContentSearch.SolrProvider.SolrSearchContext context, IExecutionContext[] executionContexts)
            : base(context, executionContexts)
        {
            var solrIndexConfiguration = (SolrIndexConfiguration)context.Index.Configuration;
            var parameters = new SolrIndexParameters(solrIndexConfiguration.IndexFieldStorageValueFormatter, solrIndexConfiguration.VirtualFields, context.Index.FieldNameTranslator, executionContexts, solrIndexConfiguration.FieldMap);
            this.QueryMapper = new Sitecore.Spatial.GeoJson.Providers.Solr.SolrQueryMapper(parameters);
            this.queryOptimizer = new Sitecore.ContentSearch.Linq.Solr.SolrQueryOptimizer();
        }

        protected override IQueryOptimizer QueryOptimizer => this.queryOptimizer;

        protected override QueryMapper<SolrCompositeQuery> QueryMapper
        {
            get;
        }

        public override IQueryable<TItem> GetQueryable()
        {
            var expressionParser = new Sitecore.Spatial.GeoJson.Linq.ExpressionParser(typeof(TItem), this.ItemType, this.FieldNameTranslator);
            IQueryable<TItem> queryable = new Sitecore.Spatial.GeoJson.Linq.GenericQueryable<TItem, SolrCompositeQuery>(this, this.QueryMapper, this.QueryOptimizer, this.FieldNameTranslator, expressionParser);
            ((IHasTraceWriter)queryable).TraceWriter = ((IHasTraceWriter)this).TraceWriter;
            return this.GetTypeInheritance(typeof(TItem)).SelectMany(t => t.GetCustomAttributes(typeof(IPredefinedQueryAttribute), true).Cast<IPredefinedQueryAttribute>()).Aggregate(queryable, (q, a) => a.ApplyFilter(q, this.ValueFormatter));
        }

        private IEnumerable<Type> GetTypeInheritance(Type type)
        {
            yield return type;
            Type baseType = type.BaseType;
            while (baseType != (Type)null)
            {
                yield return baseType;
                baseType = baseType.BaseType;
            }
        }
    }
}