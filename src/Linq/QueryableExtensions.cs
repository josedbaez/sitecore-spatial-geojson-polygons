namespace Sitecore.Spatial.GeoJson.Linq
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Sitecore.ContentSearch.Data;

    public static class QueryableExtensions
    {
      public static IQueryable<TSource> InsidePolygon<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Coordinate center)
       {
            // https://github.com/SolrNet/SolrNet/blob/master/Documentation/Querying.md#filter-queries
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            var method = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey));
            var arguments = new Expression[3]
                                         {
                                             source.Expression,
                                             Expression.Quote(keySelector),
                                             Expression.Constant(center, typeof(Coordinate))
                                         };
            return source.Provider.CreateQuery<TSource>((Expression)Expression.Call(null, method, arguments));
        }
    }
}