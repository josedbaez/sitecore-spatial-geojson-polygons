# sitecore-spatial-geojson-polygons

See post on implementation: https://josedbaez.com/2019/07/sitecore-polygon-search/ 

This repository contains the implementation of a Linq extension to be able to query on points that intercept GeoJson polygons in solr indexes. 

Demo:
----
The demo provides a template with the spatial field, couple of sample items with polygons from openstreetmap that cover some cities; and a controller rendering that reads lat and lng from the querystring and returns the polygon of the item found (e.g. Big Ben’s coordinates http://sc9xp0.sc/?lat=-0.1268141&lng=51.5007292 will return London).

To use:
- Update solr schema with fields under /config
- Install sitecore package under /items-package
- Add Polygon rendering to an item and browse it providing lat and lng querystring parameters