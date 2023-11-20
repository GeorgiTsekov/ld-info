using System.ComponentModel.DataAnnotations.Schema;

namespace LDInfo.Api.Features.Projects.Model
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
