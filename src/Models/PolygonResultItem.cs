namespace Sitecore.Spatial.GeoJson.Models
{
    using System;

    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Data;
    using Sitecore.ContentSearch.SearchTypes;

    [Serializable]
    public class PolygonResultItem : SearchResultItem
    {

        [IndexField("polygon_grptgeom")]
        public Coordinate Polygon { get; set; }

        [IndexField("polygon_grptgeom")]
        public string PolygonResult { get; set; }
    }
}