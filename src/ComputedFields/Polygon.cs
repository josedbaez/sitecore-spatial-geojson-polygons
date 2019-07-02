namespace Sitecore.Spatial.GeoJson.ComputedFields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Data.Items;

    public class Polygon : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            return item?.Fields["polygongeom"].Value;
        }
    }
}