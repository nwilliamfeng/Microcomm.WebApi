using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microcomm.Web.Http.Handlers
{
    /// <summary>
    /// 客户端IP过滤
    /// </summary>
    public class IPFilterHandler:DelegatingHandler
    {
        public IEnumerable<string> WhiteIPs { get; set; }

        public IPFilterHandler(IEnumerable<string> whiteIPs)
        {
            if (whiteIPs == null)
                throw new ArgumentNullException("IP白名单为空。");
            this.WhiteIPs = whiteIPs;
        } 

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.AllowIP(this.WhiteIPs))
            {
                return await base.SendAsync(request, cancellationToken);
            }
            return request.CreateErrorResponse(HttpStatusCode.Unauthorized , "未授权的IP");
        }
    }
}
