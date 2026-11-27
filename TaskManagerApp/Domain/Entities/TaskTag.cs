using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class TaskTag
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(7)]
        [RegularExpression("^#[0-9A-Fa-f]{6}$")]
        public string Color { get; set; } = "#6258E8";

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}