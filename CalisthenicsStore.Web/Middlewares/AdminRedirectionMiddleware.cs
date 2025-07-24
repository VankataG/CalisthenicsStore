using static CalisthenicsStore.Common.RolesConstants;

namespace CalisthenicsStore.Web.Middlewares
{
    public class AdminRedirectionMiddleware
    {
        private const string IndexPath = "/";
        private const string AdminIndexPath = "/Admin";


        private readonly RequestDelegate next;

        public AdminRedirectionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                if (context.User.IsInRole(AdminRoleName) && context.Request.Path == IndexPath)
                {
                    context.Response.Redirect(AdminIndexPath);
                    return;
                }
            }

            await this.next(context);
        }
    }
}
