using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.ViewModel.GeoObjects
{
    public class GeoObjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public List<GeoObjectResponse> Children { get; set; }
    }
}
