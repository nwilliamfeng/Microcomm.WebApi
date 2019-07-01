using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microcomm.Web.Http
{
    public static class HttpRequestMessageExtension
    {
        /// <summary>
        /// 返回请求对应客户端的IpV6地址
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public static string GetIP(this HttpRequestMessage requestMessage)
        {        
            //// Owin Hosting
            //if (requestMessage.Properties.ContainsKey("MS_OwinContext"))
            //{
            //    return HttpContext.Current != null
            //        ? HttpContext.Current.Request.GetOwinContext().Request.RemoteIpAddress
            //        : null;
            //}
            // Web Hosting
            if (requestMessage.Properties.ContainsKey("MS_HttpContext"))
            {
                return HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : null;
            }
            // Self Hosting
            if (requestMessage.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty property =(RemoteEndpointMessageProperty)requestMessage.Properties[RemoteEndpointMessageProperty.Name];
                return property != null ? property.Address : null;
            }
            return null;
        }

        /// <summary>
        /// 返回对应请求的所有ipv4地址
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetIPV4AddressList(this HttpRequestMessage requestMessage)
        {
            Func<string, IEnumerable<string>> func = addr =>
              {
                  if (addr == null)
                      return new string[] { };
                  IPHostEntry hostInfo = Dns.GetHostEntry(addr);
                  return hostInfo.AddressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(x => x.MapToIPv4().ToString());
              };
            if (requestMessage.Properties.ContainsKey("MS_HttpContext"))
            {
                var addr = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : null;
                return func(addr);
            }
 
            if (requestMessage.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty property = (RemoteEndpointMessageProperty)requestMessage.Properties[RemoteEndpointMessageProperty.Name];
                var addr = property != null ? property.Address : null;
                return func(addr);
            }
            return null;
        }

        public static bool AllowIP(this HttpRequestMessage requestMessage,IEnumerable<string> whiteIPs)
        {
            var ips = requestMessage.GetIPV4AddressList();
            return whiteIPs.Any(x => ips.Any(ip=>ip.Equals(x,StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
