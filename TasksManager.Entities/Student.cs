using System.ComponentModel.DataAnnotations;

namespace TasksManager.Entities
{
    public class Student : DomainObject
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
