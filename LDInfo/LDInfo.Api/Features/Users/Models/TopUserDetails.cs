using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LDInfo.Api.Features.Users.Models
{
    public class TopUserDetails
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;
        public float WorkedHours { get; set; }
    }
}
