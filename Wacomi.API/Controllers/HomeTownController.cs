using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class HomeTownController : Controller
    {
        private readonly IDataRepository _repo;
        public HomeTownController(IDataRepository repo){
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetHomeTowns()
        {
            return Ok(await _repo.GetHomeTowns());
        }
    }
}