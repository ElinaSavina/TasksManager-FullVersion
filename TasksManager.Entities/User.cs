using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TasksManager.Entities
{
    public class User : DomainObject
    {
        [Required]
        [MaxLength(64)]
        public string Login { get; set; }
        [Required]
        [MaxLength(512)]
        public string Password { get; set; }

        public ICollection<Project> Projects { get; set; }
       
        public string RoleName { get; set; }
        public Role Role { get; set; }
    }
}
