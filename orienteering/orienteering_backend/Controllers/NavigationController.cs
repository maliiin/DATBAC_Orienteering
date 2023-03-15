using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;

namespace orienteering_backend.Controllers
{
    [ApiController]
    [Route("api/navigation")]

    public class NavigationController : Controller
    {
        [HttpPost("AddImage")]
        public async Task<Guid> AddImage([FromForm] IFormFile test)
        {
            Console.WriteLine(test);
            return new Guid();
        }

        [HttpGet("test")]
        public void test(string formFile)
        {
            Console.WriteLine(formFile);

        }


    }
}
