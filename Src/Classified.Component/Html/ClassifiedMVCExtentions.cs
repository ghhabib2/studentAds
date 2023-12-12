using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Classified.Component.Html
{
    public static class ClassifiedMvcExtentions
    {
        public static string GetPropertyName<TModel>
            (this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, object>> propertyNameExpr)
        {
            return ExpressionHelper.GetExpressionText(propertyNameExpr).Replace('.', '_');
        }
    }
}

