using AutoMapper;
using LDInfo.Api.Features.Projects;
using LDInfo.Api.Features.Projects.Model;
using LDInfo.Api.Features.TimeLogs;
using LDInfo.Api.Features.TimeLogs.Models;
using LDInfo.Api.Features.Users;
using LDInfo.Api.Features.Users.Models;
using LDInfo.API.Utils;
using LDInfo.Data.Entities;

namespace LDInfo.Api.Features.Seeder
{
    public class SeederService : ISeederService
    {
        private readonly IUserService userService;
        private readonly IProjectService projectService;
        private readonly ITimeLogService timeLogService;
        private readonly IMapper mapper;

        public SeederService(IUserService userService, IProjectService projectService, ITimeLogService timeLogService, IMapper mapper)
        {
            this.userService = userService;
            this.projectService = projectService;
            this.timeLogService = timeLogService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Clear the tables in database
        /// Seed new Projects
        /// Seed new Users
        /// Seed new TimeLogs
        /// </summary>
        public async Task Seed()
        {
            await ResetDb();
            await SeedProjects();
            await SeedUsers();
        }

        /// <summary>
        /// Delete all users, timelogs and projects in our database
        /// </summary>
        private async Task ResetDb()
        {
            await this.timeLogService.DeleteAll();
            await this.projectService.DeleteAll();
            await this.userService.DeleteAll();
        }

        /// <summary>
        /// Seed projectNames and create 3 new Projects if they are not exist
        /// </summary>
        private async Task SeedProjects()
        {
            foreach (var projectName in StaticConstants.PROJECT_NAMES)
            {
                var project = await projectService.ByNameAsync(projectName);

                if (project == null)
                {
                    var projectDto = new ProjectDto
                    {
                        Name = projectName
                    };

                    var model = mapper.Map<Project>(projectDto);

                    await projectService.CreateAsync(model);
                }
            }
        }

        /// <summary>
        /// Create 100 new Users if they aren't exsits
        /// </summary>
        private async Task SeedUsers()
        {
            for (int i = 0; i < StaticConstants.USERS_COUNT; i++)
            {
                // get random from array of firstNames
                var firstNameRng = StaticConstants.FIRST_NAMES[Random.Shared.Next(StaticConstants.FIRST_NAMES.Length)];

                // get random from array of lastNames
                var lastNameRng = StaticConstants.LAST_NAMES[Random.Shared.Next(StaticConstants.LAST_NAMES.Length)];

                // get random from array of domains and create email = firstName.lastName@domain
                var emailRng = $"{firstNameRng}.{lastNameRng}@{StaticConstants.DOMAINS[Random.Shared.Next(StaticConstants.DOMAINS.Length)]}";

                var currentUser = await userService.ByEmailAsync(emailRng);

                // if user not exists create new one 
                if (currentUser == null)
                {
                    var userDto = new UserDto
                    {
                        FirstName = firstNameRng,
                        LastName = lastNameRng,
                        Email = emailRng
                    };

                    var user = mapper.Map<User>(userDto);

                    await userService.CreateAsync(user);

                    await SeedTimeLogs(user.Id);
                }
                else
                {
                    await SeedTimeLogs(currentUser.Id);
                }
            }
        }

        /// <summary>
        /// Create new seeds between 1 and 20 for current user with random date
        /// If the current user has worked more than 7.75 hours, we don't add any more hours for that day and just move on to the next timeLog
        /// </summary>
        private async Task SeedTimeLogs(Guid userId)
        {
            // get random between 1 and 20
            var timeLogsCountRnd = Random.Shared.Next(StaticConstants.TIME_LOGS_MIN_COUNT, StaticConstants.TIME_LOGS_MAX_COUNT);

            for (int j = 0; j < timeLogsCountRnd; j++)
            {
                // get random from array of projects
                var projectNameRnd = StaticConstants.PROJECT_NAMES[Random.Shared.Next(StaticConstants.PROJECT_NAMES.Length)];
                var project = await projectService.ByNameAsync(projectNameRnd);

                // get random between 0 and 10
                var dayRnd = Random.Shared.Next(StaticConstants.MIN_DATE, StaticConstants.MAX_DATE);

                // get date from today + dayRng
                var date = DateTime.Today.AddDays(dayRnd).Date;

                // get sum of all worked hours for current user for current date
                var user = await this.userService.ByIdAsync(userId);
                var hoursWorked = user.TimeLogs.Where(x => x.Date == date).Sum(x => x.Hours);

                // if hoursWorked is bigger then 7.75 current user cannot works more current day
                if (hoursWorked > StaticConstants.MAX_HOURS - StaticConstants.MIN_HOURS)
                {
                    continue;
                }

                // get max possible hours for current user and current date
                var maxPossibleHours = StaticConstants.MAX_HOURS - hoursWorked - StaticConstants.MIN_HOURS;

                // get random between maxPossibleHours and min hours 
                var hoursRnd = Random.Shared.NextDouble() * maxPossibleHours + StaticConstants.MIN_HOURS;

                // convert hoursRnd to float with 2 digits after the decimal point
                var hours = (float)Convert.ToDecimal(string.Format("{0:F2}", hoursRnd));

                var timeLogDto = new TimeLogDto
                {
                    Date = date,
                    UserId = userId,
                    ProjectId = project.Id,
                    Hours = hours
                };

                var timeLog = mapper.Map<TimeLog>(timeLogDto);

                await timeLogService.CreateAsync(timeLog);
            }
        }
    }
}
