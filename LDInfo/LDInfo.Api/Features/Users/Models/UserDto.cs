using LDInfo.Api.Features.TimeLogs.Models;
using System.ComponentModel.DataAnnotations;

namespace LDInfo.Api.Features.Users.Models
{
    public class UserDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public virtual ICollection<TimeLogDetails> TimeLogs { get; set; } = new List<TimeLogDetails>();
    }
}
