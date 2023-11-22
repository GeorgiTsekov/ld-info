using AutoMapper;
using LDInfo.Api.Features.Users.Models;
using Microsoft.AspNetCore.Mvc;

namespace LDInfo.Api.Features.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
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
            var models = await this.userService
                .AllAsync(fromDate, toDate, sortBy, isAscending ?? true, pageNumber, pageSize);

            var modelsDtos = mapper.Map<List<UserDto>>(models);

            return Ok(modelsDtos);
        }

        [HttpGet]
        [Route(nameof(GetTop10))]
        public async Task<IActionResult> GetTop10(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var models = await this.userService.Top10Async(fromDate, toDate);

            var modelsDtos = new List<TopUserDetails>();

            foreach (var model in models)
            {
                var user = new TopUserDetails
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    WorkedHours = model.TimeLogs.Sum(x => x.Hours)
                };

                modelsDtos.Add(user);
            }

            return Ok(modelsDtos);
        }

        [HttpGet]
        [Route(nameof(GetByEmail))]
        public async Task<IActionResult> GetByEmail(
            string email,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var model = await this.userService.ByEmailAndDate(email, fromDate, toDate);

            return Ok(model);
        }
    }
}
