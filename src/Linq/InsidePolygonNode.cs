namespace Sitecore.Spatial.GeoJson.Linq
{
    using System.Collections.Generic;

    using Sitecore.ContentSearch.Data;
    using Sitecore.ContentSearch.Linq.Nodes;

    public class InsidePolygonNode : QueryNode
    {
        public InsidePolygonNode(QueryNode sourceNode, string field, Coordinate coordinateToSearch)
        {
            this.SourceNode = sourceNode;
            this.Field = field;
            this.Coordinate = coordinateToSearch;
        }

        public override QueryNodeType NodeType => QueryNodeType.Custom;

        public QueryNode SourceNode
        {
            get;
            protected set;
        }

        public string Field
        {
            get;
            protected set;
        }

        public Coordinate Coordinate
        {
            get;
            protected set;
        }

        public override IEnumerable<QueryNode> SubNodes
        {
            get
            {
                yield return this.SourceNode;
            }
        }
    }
}