namespace Sitecore.Spatial.GeoJson.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using GeoJSON.Net.Feature;
    using GeoJSON.Net.Geometry;
    using Newtonsoft.Json;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Data;
    using Sitecore.Spatial.GeoJson.Linq;
    using Sitecore.Spatial.GeoJson.Models;

    public class PolygonController : Controller
    {
        // GET: Polygon
        public ActionResult Index()
        {
            var latStr = Request.QueryString["lat"];
            var lngStr = Request.QueryString["lng"];

            if (string.IsNullOrEmpty(latStr) || string.IsNullOrEmpty(lngStr) || 
                !double.TryParse(latStr, out double lat) || !double.TryParse(lngStr, out double lng))
            {
                return null;
            }


            var index = ContentSearchManager.GetIndex("sc9xp0_spatial_master");
            using (var context = index.CreateSearchContext())
            {
                var queryable = context.GetQueryable<PolygonResultItem>().InsidePolygon(s => s.Polygon, new Coordinate(lat, lng));
                var list = queryable.ToList();
                if (list.Any())
                {
                    var res = list.First();
                    var multiPoly = JsonConvert.DeserializeObject<MultiPolygon>(res.PolygonResult);

                    var featureCol = new FeatureCollection();
                    var feature = new Feature(multiPoly);
                    featureCol.Features.Add(feature);
                    
                    var viewModel = new PolygonViewModel
                    {
                        PolygonsFeature = featureCol
                    };
                    return this.View("~/Views/Polygon.cshtml", viewModel);
                }
            }

            return null;
        }
    }
}