using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static CalisthenicsStore.Common.RolesConstants;

namespace CalisthenicsStore.Web.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public abstract class BaseAdminController : Controller
    {

    }
}
