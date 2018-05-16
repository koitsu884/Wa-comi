using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class CityController : Controller
    {
        private readonly IDataRepository _repo;
        public CityController(IDataRepository repo){
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            return Ok(await _repo.GetCities());
        }

    }
}