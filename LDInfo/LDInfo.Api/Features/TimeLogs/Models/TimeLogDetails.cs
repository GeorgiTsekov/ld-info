using System.ComponentModel.DataAnnotations.Schema;

namespace LDInfo.Api.Features.TimeLogs.Models
{
    public class TimeLogDetails
    {
        [Column(TypeName = "float(18,2)")]
        public float Hours { get; set; }
        public DateTime Date { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserFirstName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}
