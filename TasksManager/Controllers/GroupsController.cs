using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Services.Groups;
using TasksManager.Entities;
using TasksManager.ViewModel.Group;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class GroupsController : Controller
    {
        private readonly IGroupService _groupService;
        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(GroupResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetGroupAsync(int id)
        {
            var response = await _groupService.GetGroup(id);
            return response == null
                ? (IActionResult)NotFound("Group Not Found")
                : Ok(response);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(GroupResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateGroupAsync([FromBody]CreateGroupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _groupService.AddGroup(request);
            return Created(Url.RouteUrl(new { response.Id }), response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(GroupResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateGroupAsync(int id, [FromBody]UpdateGroupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _groupService.UpdateGroup(id, request);
            return response == null
                ? (IActionResult)NotFound("Group Not Found")
                : Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete(int id)
        {
            await _groupService.RemoveGroup(id);
            return NoContent();
        }
    }
}
