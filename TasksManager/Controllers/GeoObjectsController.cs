using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Services.GeoObjects;
using TaskManager.Services.Groups;
using TasksManager.ViewModel;
using TasksManager.ViewModel.GeoObjects;
using TasksManager.ViewModel.Group;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class GeoObjectsController : Controller
    {
        private readonly IGeoObjectsService _geoObjectsService;
        public GeoObjectsController(IGeoObjectsService geoObjectsService)
        {
            _geoObjectsService = geoObjectsService;
        }

        [HttpGet("children/{id}")]
        [ProducesResponseType(200, Type = typeof(GeoObjectResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetChildrenAsync(int id)
        {
            var response = await _geoObjectsService.GetChildren(id);
            return response == null
                ? (IActionResult)NotFound("Geo object Not Found")
                : Ok(response);
        }
        [HttpGet("{level}")]
        [ProducesResponseType(200, Type = typeof(GeoObjectResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRootsAsync(int level)
        {
            var response = await _geoObjectsService.GetRoot(level);
            return response == null
                ? (IActionResult)NotFound("Geo object Not Found")
                : Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete(int id)
        {
            await _geoObjectsService.Delete(id);
            return NoContent();
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(GeoObjectResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddGeoObjectAsync([FromBody]AddGeoObjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _geoObjectsService.AddNode(request);
                return Created(Url.RouteUrl(new {response.Id}), response);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
