using AutoMapper;
using LDInfo.Api.Features.TimeLogs.Models;
using Microsoft.AspNetCore.Mvc;

namespace LDInfo.Api.Features.TimeLogs
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeLogsController : ControllerBase
    {
        private readonly ITimeLogService timeLogService;
        private readonly IMapper mapper;

        public TimeLogsController(ITimeLogService timeLogService, IMapper mapper)
        {
            this.timeLogService = timeLogService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            var models = await this.timeLogService.AllAsync(fromDate, toDate, sortBy, isAscending ?? true, pageNumber, pageSize);

            var modelsDtos = mapper.Map<List<TimeLogDetails>>(models);

            return Ok(modelsDtos);
        }
    }
}
