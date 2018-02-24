using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TasksManager.DataAccess.Interfaces.Tasks;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tasks;
using Microsoft.AspNetCore.Authorization;
using TasksManager.AuthHandlers;
using TasksManager.DataAccess.Interfaces.Projects;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        IAuthorizationService _authorizationService;
        public TasksController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ListResponse<TaskResponse>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTasksListAsync(TaskFilter filter, ListOptions options, [FromServices]ITaskListQuery query)
        {
            try
            {
                if ((await _authorizationService.AuthorizeAsync(User, new Entities.Task(), Operations.Read)).Succeeded)
                {
                    var response = await query.RunAsync(filter, options);
                    return Ok(response);
                }
                return StatusCode(403, "Вы не можете просматривать задачи!");
            }
            catch (WrongSortPropertyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TaskResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTaskAsync([FromBody]CreateTaskRequest request, [FromServices]ICreateTaskCommand command, [FromServices]IGetProjectQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var project = await query.RunAsync(request.ProjectId);
                if ((await _authorizationService.AuthorizeAsync(User, project, Operations.CreateTask)).Succeeded)
                {
                    TaskResponse response = await command.ExecuteAsync(request);
                    return Created(Url.RouteUrl(new { response.Id }), response);
                }
                return StatusCode(403, "Вы не можете создать задачу для этого проекта!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{taskId}")]
        [ProducesResponseType(200, Type = typeof(TaskResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTaskAsync(int taskId, [FromServices]ITaskQuery query)
        {
            if ((await _authorizationService.AuthorizeAsync(User, new Entities.Task(), Operations.Read)).Succeeded)
            {
                TaskResponse response = await query.RunAsync(taskId);
                return response == null
                    ? (IActionResult)NotFound("Task Not Found")
                    : Ok(response);
            }
            return StatusCode(403, "Вы не можете просматривать задачи!");
        }

        [Authorize]
        [HttpPut("{taskId}")]
        [ProducesResponseType(200, Type = typeof(TaskResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTaskAsync(int taskId, [FromBody]UpdateTaskRequest request, [FromServices]IUpdateTaskCommand command, [FromServices]IGetTaskQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var task = await query.RunAsync(taskId);
            if ((await _authorizationService.AuthorizeAsync(User, task, Operations.Update)).Succeeded)
            {
                TaskResponse response = await command.ExecuteAsync(taskId, request);
                return response == null
                    ? (IActionResult)NotFound("Task Not Found")
                    : Ok(response);
            }
            return StatusCode(403, "Вы не можете изменить эту задачу!");
        }

        [Authorize]
        [HttpPut("{taskId}/tags/{tag}")]
        [ProducesResponseType(200, Type = typeof(TaskResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddTagToTaskTaskAsync(int taskId, string tag, [FromServices]IAddTagToTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            TaskResponse response = await command.ExecuteAsync(taskId, tag);
            return response == null
                ? (IActionResult)NotFound("Task Not Found")
                : Ok(response);
        }

        [Authorize]
        [HttpDelete("{taskId}/tags/{tag}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteTagFromTaskAsync(int taskId, string tag,[FromServices]IDeleteTagFromTaskCommand command)
        {
            await command.ExecuteAsync(taskId, tag);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{taskId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteTaskAsync(int taskId, [FromServices]IDeleteTaskCommand command, [FromServices]IGetTaskQuery query)
        {
            var task = await query.RunAsync(taskId);
            if ((await _authorizationService.AuthorizeAsync(User, task, Operations.Delete)).Succeeded)
            {
                await command.ExecuteAsync(taskId);
                return NoContent();
            }
            return StatusCode(403, "Вы не можете удалить эту задачу!");
        }
    }

}
