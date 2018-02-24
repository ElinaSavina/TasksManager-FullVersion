using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TasksManager.Entities
{
    public class Tag : DomainObject
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        public ICollection<TagsInTask> Tasks { get; set; }
        [NotMapped]
        public int? TasksCount => Tasks?.Count ?? 0;
        [NotMapped]
        public int? OpenTasksCount => Tasks?.Select(x => x.Task).Count(t => t.Status != TaskStatus.Completed) ?? 0;
    }
}
