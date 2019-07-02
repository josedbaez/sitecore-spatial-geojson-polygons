namespace Sitecore.Spatial.GeoJson.Providers.Solr
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Maintenance;
    using Sitecore.ContentSearch.Security;
    using Sitecore.ContentSearch.SolrProvider;

    //Create a SwitchOnRebuildSolrSearchIndex if using this strategy
    public class SolrSearchIndex : Sitecore.ContentSearch.SolrProvider.SolrSearchIndex
    {
        public SolrSearchIndex(string name, string core, IIndexPropertyStore propertyStore)
            : base(name, core, propertyStore)
        {
        }

        public SolrSearchIndex(string name, string core, IIndexPropertyStore propertyStore, string @group)
            : base(name, core, propertyStore, group)
        {
        }

        public override IProviderSearchContext CreateSearchContext(SearchSecurityOptions options = SearchSecurityOptions.Default)
        {
            if (this.Group != IndexGroup.Experience)
            {
                return new Sitecore.Spatial.GeoJson.Providers.Solr.SolrSearchContext(this, options);
            }
            return new SolrAnalyticsSearchContext(this, options);
        }
    }
}