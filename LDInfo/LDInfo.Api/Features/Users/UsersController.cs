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
        public async Task<IActionResult> GetAllAsync()
        {
            var models = await this.userService.AllAsync();

            var modelsDtos = mapper.Map<List<UserDto>>(models);

            return Ok(modelsDtos);
        }
    }
}
