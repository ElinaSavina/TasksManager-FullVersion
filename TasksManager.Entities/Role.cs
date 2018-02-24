using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TasksManager.Entities
{
    public class Role
    {
        [Key]
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
    }
}
