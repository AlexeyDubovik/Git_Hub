using Forum.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Text.Json;

namespace Forum.Middleware
{
    public class SessionAuthMiddleware
    {
        private readonly RequestDelegate _next; 
        public SessionAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            String? userID     = context.Session.GetString("UserID");
            String? AuthMoment = context.Session.GetString("authMoment");            
            if (userID != null){
                authService.Set(userID);
                if (AuthMoment != null && authService.User != null) {
                    var time = DateTime.FromFileTime(DateTime.Now.Ticks - Convert.ToInt64(AuthMoment));
                    if (time.Day == 7) {
                        context.Session.Remove("UserID");
                        context.Session.Remove("authMoment");
                    }
                    authService.User.LogMoment = new DateTime(Convert.ToInt64(AuthMoment));
                    //
                    //нужно обновить БД AuthMoment
                    //
                    if (authService.User.LogMoment != null) {
                        DateTime Value = authService.User.LogMoment!.Value;
                        var differance = DateTime.Now - Value;
                        context.Items.Add("fromAuthMiddleware", "You authorized " + (int)differance.TotalSeconds + " second ago");
                    }
                }
            }
            await _next(context);
        }
    }
}
