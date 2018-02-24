using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TasksManager.Entities
{
    public class Group : DomainObject
    {
        public Group()
        {
            Students = new List<Student>();
        }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        public List<Student> Students { get; set; }
    }
}
