using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsStore.Web.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        
    }
}
