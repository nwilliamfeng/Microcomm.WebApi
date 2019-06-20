using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Newtonsoft.Json;

namespace Microcomm.Web.Http.Filters
{
    public class GlobalLogFilter : ActionFilterAttribute
    {
        private static ILog log ;
        private static Func<HttpActionExecutedContext, string> loggerHandle;

        public GlobalLogFilter()
        {
            if (log == null)
                log = LogUtil.CreateLogger("api_info");
            if (loggerHandle == null)
            {
                loggerHandle = actionExecutedContext =>
                {
                    string result = null;
                    if (actionExecutedContext.Response != null)
                        result = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                    var uri = actionExecutedContext.Request.RequestUri;
                    var headers = JsonConvert.SerializeObject(actionExecutedContext.ActionContext.Request.Headers);
                    var paras = JsonConvert.SerializeObject(actionExecutedContext.ActionContext.ActionArguments);
                    return $"after call url:{uri},headers:{headers},params:{paras} and the return result is : {result}";
                };
            }
        }

        public override  Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var msg = LoggerHandle(actionExecutedContext);
            this.Logger.Info(msg);
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        public Func<HttpActionExecutedContext, string> LoggerHandle
        {
            get { return loggerHandle; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                loggerHandle = value;
            }
        }

        public ILog Logger
        {
            get { return log; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                log = value;
            }
        }

        
    }
}