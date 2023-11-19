using System.ComponentModel.DataAnnotations;

namespace LDInfo.Data.Entities
{
    public class Project
    {
        public Project()
        {
            TimeLogs = new List<TimeLog>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<TimeLog> TimeLogs { get; set; }
    }
}
