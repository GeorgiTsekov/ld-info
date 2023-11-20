using Microsoft.AspNetCore.Mvc;

namespace LDInfo.Api.Features.Seeder
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeederController : ControllerBase
    {
        private readonly ISeederService seederService;

        public SeederController(ISeederService seederService)
        {
            this.seederService = seederService;
        }

        [HttpGet]
        [Route(nameof(GenerateNewData))]
        public async Task<IActionResult> GenerateNewData()
        {
            await this.seederService.Seed();

            return Ok();
        }
    }
}
