using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace Microcomm.Web.Http.Filters
{
    public class JObjectParamValidateAttribute : ActionFilterAttribute
    {



        public string Params { get; set; }



        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(Params))
            {
                var paras = Params.Split(',');
                var p = actionContext.ActionArguments.Select(x => x.Value).FirstOrDefault();
                if (actionContext.ActionArguments.Count == 0 || !(p is Newtonsoft.Json.Linq.JObject))
                    actionContext.Response = await actionContext.Request.JsonResult(new JsonResultData().SetFail($"缺少参数：{Params}")).ExecuteAsync(cancellationToken);
                else
                {
                    var dic = p as Newtonsoft.Json.Linq.JObject;
                    var losts = paras.Where(x => !dic.ContainsKey(x));
                    if (losts.Count() > 0)
                        actionContext.Response = await actionContext.Request.JsonResult(new JsonResultData().SetFail($"缺少参数：{string.Join(",", losts.ToArray())}")).ExecuteAsync(cancellationToken);
                }
            }
            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }



    }
}