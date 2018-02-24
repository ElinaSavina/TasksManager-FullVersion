using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TasksManager.ViewModel.Group
{
    public class GroupResponse
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        public string[] Students { get; set; }
    }
}
