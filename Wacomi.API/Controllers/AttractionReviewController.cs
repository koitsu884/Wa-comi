using AutoMapper;
using Wacomi.API.Data;

namespace Wacomi.API.Controllers
{
    public class AttractionReviewController : DataController
    {
        public AttractionReviewController(IAppUserRepository appUserRepository, IMapper mapper) : base(appUserRepository, mapper)
        {
        }
    }
}