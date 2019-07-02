namespace Sitecore.Spatial.GeoJson.Providers.Solr
{
    using Sitecore.ContentSearch.Linq.Nodes;
    using Sitecore.ContentSearch.Linq.Solr;
    using Sitecore.Spatial.GeoJson.Linq;

    using SolrNet;

    public class SolrQueryMapper : Sitecore.ContentSearch.Linq.Solr.SolrQueryMapper
    {
        public SolrQueryMapper(SolrIndexParameters parameters)
            : base(parameters)
        {
        }

        protected override AbstractSolrQuery Visit(QueryNode node, SolrQueryMapperState state)
        {
            if (node.NodeType == QueryNodeType.Custom)
            {
                if (node is InsidePolygonNode)
                {
                    return this.VisitInsidePolygon((InsidePolygonNode)node, state);
                }
            }

            return base.Visit(node, state);
        }

        protected virtual AbstractSolrQuery VisitInsidePolygon(InsidePolygonNode node, SolrQueryMapperState state)
        {
            AbstractSolrQuery abstractSolrQuery = new SolrQueryByField(node.Field, $"Intersects({node.Coordinate.Latitude} {node.Coordinate.Longitude})");
            if (!abstractSolrQuery)
            {
                return abstractSolrQuery & this.Visit(node.SourceNode, state);
            }

            return abstractSolrQuery;
        }

        protected virtual AbstractSolrQuery VisitInsidePolygon(InsidePolygonNode node, AbstractSolrQuery query)
        {
            AbstractSolrQuery abstractSolrQuery = new SolrQueryByField(node.Field, $"Intersects({node.Coordinate.Latitude} {node.Coordinate.Longitude})");
            if (!abstractSolrQuery)
            {
                return abstractSolrQuery & query;
            }

            return abstractSolrQuery;
        }
    }
}