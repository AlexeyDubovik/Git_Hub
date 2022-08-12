using Forum.API;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Services
{
    public class HttpRequestGuidCheck
    {
        public Guid Check(HttpContext http, String? id)
        {
            Guid guid = Guid.Empty;
            if(id == null)
                return guid;
            try
            {
                guid = Guid.Parse(id!);
                return guid;
            }
            catch
            {
                http.Response.StatusCode = 409;
                return guid;
            }
        }
    }
}
