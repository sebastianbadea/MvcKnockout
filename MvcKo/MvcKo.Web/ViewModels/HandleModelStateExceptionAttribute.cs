using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcKo.Web.ViewModels
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class, AllowMultiple=false)]
    public sealed class HandleModelStateExceptionAttribute: FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentException("filtercontext");
            }

            if 
            (
                filterContext.Exception != null
                && typeof(ModelStateException).IsInstanceOfType(filterContext.Exception)
                && !filterContext.ExceptionHandled
            )
            {
                filterContext.ExceptionHandled = true;
                var response = filterContext.HttpContext.Response;
                response.Clear();
                response.ContentEncoding = Encoding.UTF8;
                response.HeaderEncoding = Encoding.UTF8;
                response.TrySkipIisCustomErrors = true;
                response.StatusCode = 400;
                filterContext.Result = new ContentResult 
                {
                    Content = (filterContext.Exception as ModelStateException).Message,
                    ContentEncoding = Encoding.UTF8
                };
            }
        }
    }
}