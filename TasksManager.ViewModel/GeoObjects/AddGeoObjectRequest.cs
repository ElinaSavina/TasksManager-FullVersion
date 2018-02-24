using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TasksManager.ViewModel.GeoObjects
{
    public class AddGeoObjectRequest
    {
        [MaxLength(32)]
        public string Name { get; set; }
        public int Level { get; set; }
        public int? ParentId { get; set; }
    }
}
