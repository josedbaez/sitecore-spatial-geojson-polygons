namespace Sitecore.Spatial.GeoJson.Linq
{
    using System;
    using System.Linq.Expressions;
    using Sitecore.ContentSearch.Data;
    using Sitecore.ContentSearch.Linq.Common;
    using Sitecore.ContentSearch.Linq.Nodes;

    public class ExpressionParser : Sitecore.ContentSearch.Linq.Parsing.ExpressionParser
    {
        public ExpressionParser(Type elementType, Type itemType, FieldNameTranslator fieldNameTranslator)
            : base(elementType, itemType, fieldNameTranslator)
        {
        }

        protected override QueryNode VisitMethodCall(MethodCallExpression methodCall)
        {
            if (methodCall.Method.DeclaringType == typeof(QueryableExtensions))
            {
                return this.VisitQueryableExtensionMethod(methodCall);
            }

            return base.VisitMethodCall(methodCall);
        }

        protected override QueryNode VisitQueryableExtensionMethod(MethodCallExpression methodCall)
        {
            switch (methodCall.Method.Name)
            {
                case "InsidePolygon":
                    return this.InsidePolygonMethod(methodCall);
                default:
                    return base.VisitQueryableExtensionMethod(methodCall);
            }
        }

        protected virtual QueryNode InsidePolygonMethod(MethodCallExpression methodCall)
        {
            var sourceNode = this.Visit(this.GetArgument(methodCall.Arguments, 0));
            var lambdaExpression = this.Convert<LambdaExpression>(this.StripQuotes(this.GetArgument(methodCall.Arguments, 1)));
            var constantExpression = (ConstantExpression)this.GetArgument(methodCall.Arguments, 2);
            if (!(this.Visit(lambdaExpression.Body) is FieldNode fieldNode))
            {
                throw new NotSupportedException($"Can't run method on: '{typeof(FieldNode).FullName}'. Expression used:'{methodCall.Arguments[1].Type.FullName}'");
            }

            return new InsidePolygonNode(sourceNode, fieldNode.FieldKey, (Coordinate)constantExpression.Value);
        }
    }
}