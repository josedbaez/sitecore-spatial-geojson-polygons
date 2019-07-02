namespace Sitecore.Spatial.GeoJson.Linq
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Sitecore.ContentSearch.Linq.Common;
    using Sitecore.ContentSearch.Linq.Indexing;
    using Sitecore.ContentSearch.Linq.Parsing;

    public class GenericQueryable<TElement, TQuery> : Sitecore.ContentSearch.Linq.Parsing.GenericQueryable<TElement, TQuery>
    {
        private readonly Sitecore.ContentSearch.Linq.Parsing.ExpressionParser expressionParser;

        public GenericQueryable(Index<TElement, TQuery> index, QueryMapper<TQuery> queryMapper, IQueryOptimizer queryOptimizer, FieldNameTranslator fieldNameTranslator, Sitecore.ContentSearch.Linq.Parsing.ExpressionParser expressionParser)
            : base(index, queryMapper, queryOptimizer, fieldNameTranslator)
        {
            this.expressionParser = expressionParser;
        }

        protected GenericQueryable(Index<TQuery> index, QueryMapper<TQuery> queryMapper, IQueryOptimizer queryOptimizer, Expression expression, Type itemType, FieldNameTranslator fieldNameTranslator, Sitecore.ContentSearch.Linq.Parsing.ExpressionParser expressionParser)
            : base(index, queryMapper, queryOptimizer, expression, itemType, fieldNameTranslator)
        {
            this.expressionParser = expressionParser;
        }

        public override IQueryable<TQueryElement> CreateQuery<TQueryElement>(Expression expression)
        {
            return new Sitecore.Spatial.GeoJson.Linq.GenericQueryable<TQueryElement, TQuery>(this.Index, this.QueryMapper, this.QueryOptimizer, expression, this.ItemType, this.FieldNameTranslator, this.expressionParser);
        }

        protected override TQuery GetQuery(Expression expression)
        {
            IndexQuery indexQuery = this.expressionParser.Parse(expression);
            IndexQuery indexQuery2 = this.QueryOptimizer.Optimize(indexQuery);
            this.Trace(indexQuery, "GenericQueryable query (Raw):");
            this.Trace(indexQuery2, "GenericQueryable query (Optimized):");
            TQuery val = this.QueryMapper.MapQuery(indexQuery2);
            this.Trace(new Sitecore.Spatial.GeoJson.Linq.GenericDumpable(val), "Mapped query:");
            return val;
        }
    }
}