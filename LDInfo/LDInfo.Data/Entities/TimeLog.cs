using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDInfo.Data.Entities
{
    public class TimeLog
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "float(18,2)")]
        public float Hours { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }
    }
}
