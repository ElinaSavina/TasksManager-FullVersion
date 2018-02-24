using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TasksManager.Entities
{
    public class Project : DomainObject
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }
        
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<Task> Tasks { get; set; }
        [NotMapped]
        public int OpenTasksCount => Tasks?.Count(t => t.Status != TaskStatus.Completed) ?? 0; 

        public ProjectLock Lock { get; set; }

        public User User { get; set; }       
        public int UserId { get; set; }
        
    }
}
