using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApp.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Authorize] //yetkili olmayanlar cağıramaz
    public class BaseController : ControllerBase
    {

    }
}