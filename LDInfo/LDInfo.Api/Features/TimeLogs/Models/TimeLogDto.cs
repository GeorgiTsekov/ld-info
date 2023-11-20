using System.ComponentModel.DataAnnotations.Schema;

namespace LDInfo.Api.Features.TimeLogs.Models
{
    public class TimeLogDto
    {
        [Column(TypeName = "float(18,2)")]
        public float Hours { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
    }
}
