using Microsoft.AspNetCore.Http;

namespace WebApplication1.Lib
{
    public static class UserIdentityExtension
    {
        public static string UserIdentity(this HttpContext context)
        {
            var user = context.User?.Identity?.Name;
            const string identityString = "identity";
            string identity;

            if (!context.Request.Cookies.ContainsKey(identityString))
            {
                if (string.IsNullOrWhiteSpace(user))
                {
                    identity = context.Request.Cookies.ContainsKey("ai_user")
                             ? context.Request.Cookies["ai_user"]
                             : context.Connection.Id;
                }
                else
                {
                    identity = user;
                }
                context.Response.Cookies.Append("identity", identity);
            }
            else
            {
                identity = context.Request.Cookies[identityString];
            }
            return identity;
        }
    }
}
