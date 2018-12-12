using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Wacomi.API.Controllers
{
    public class Fallback : Controller
    {
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/clientapp/dist", "index.html"), "text/HTML");
        }
    }
}