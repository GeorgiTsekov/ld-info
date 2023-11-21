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
    }
}
