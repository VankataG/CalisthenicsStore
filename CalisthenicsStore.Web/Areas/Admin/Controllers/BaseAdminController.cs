using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static CalisthenicsStore.Common.RolesConstants;

namespace CalisthenicsStore.Web.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public abstract class BaseAdminController : Controller
    {
        protected string GetUserId()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return userId;
        }
    }
}
