using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TasksManager.Entities
{
    public class GeoObject : DomainObject
    {
        public GeoObject()
        {
            Children = new List<GeoObject>();
        }
        [MaxLength(32)]
        public string Name { get; set; }
        public int Level { get; set; }
        public int? ParentId { get; set; }
        public GeoObject Parent { get; set; }
        public ICollection<GeoObject> Children { get; set; }
    }
}
