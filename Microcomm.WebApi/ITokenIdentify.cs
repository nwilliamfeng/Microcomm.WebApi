using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
 

namespace Microcomm.Web.Http
{
    public interface ITokenIdentify
    {
     
        Task<JsonResultData<bool>> Validate(string accessToken);
    }
}
