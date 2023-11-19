using System.ComponentModel.DataAnnotations;

namespace LDInfo.Data.Entities
{
    public class User
    {
        public User()
        {
            TimeLogs = new List<TimeLog>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public virtual ICollection<TimeLog> TimeLogs { get; set; }
    }
}
