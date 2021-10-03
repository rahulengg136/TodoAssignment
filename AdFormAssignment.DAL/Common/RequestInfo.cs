using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AdFormAssignment.DAL.Common
{
    public class RequestInfo
    { 
        public static string GetCorrelationId( HttpRequest request)
        {
            return request.Headers.FirstOrDefault(x => x.Key == "x-correlation-id").Value.FirstOrDefault();
        }
    }
}
