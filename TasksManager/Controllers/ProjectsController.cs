using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksManager.DataAccess.Interfaces.Projects;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Projects;
using TasksManager.Entities;
using Microsoft.EntityFrameworkCore;
using TasksManager.AuthHandlers;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        IAuthorizationService _authorizationService;
        public ProjectsController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ListResponse<ProjectResponse>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProjectsListAsync(ProjectFilter filter, ListOptions options, [FromServices]IProjectsListQuery query)
        {
            try
            {
                var response = await query.RunAsync(filter, options);
                return Ok(response);
            }
            catch (WrongSortPropertyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProjectResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateProjectAsync([FromBody]CreateProjectRequest request, [FromServices]ICreateProjectCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string user = User.Identity.Name;
            ProjectResponse response = await command.ExecuteAsync(request, user);
            return Created(Url.RouteUrl(new {response.Id}), response);
        }

        [HttpGet("{projectId}")]
        [ProducesResponseType(200, Type = typeof(ProjectResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProjectAsync(int projectId, [FromServices]IProjectQuery query)
        {        
            ProjectResponse response = await query.RunAsync(projectId);
            return response == null
                ? (IActionResult)NotFound("Project Not Found")
                : Ok(response);
        }

        //Обновление
        [Authorize]
        [HttpPut("{projectId}")]
        [ProducesResponseType(200, Type = typeof(ProjectResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateProjectAsync(int projectId, [FromBody]UpdateProjectRequest request, [FromServices]IUpdateProjectCommand command, [FromServices]IGetProjectQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string user = User.Identity.Name;
            var project = await query.RunAsync(projectId);
            try
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, project, Operations.Update);
                if (authorizationResult.Succeeded)                
                {
                    ProjectResponse response = await command.ExecuteAsync(projectId, request, user);
                    return response == null
                        ? (IActionResult)NotFound("Project Not Found")
                        : Ok(response);
                }
                return StatusCode(403, "Вы не можете изменить этот проект!");
            }
            catch (ProjectLockedException)
            {
                return BadRequest("В данный момент изменение проекта невозможно!");
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Ошибка параллелизма!");
            }
        }

        //Блокировка//
        [Authorize]
        [HttpPut("{projectId}/lock")]
        [ProducesResponseType(200, Type = typeof(ProjectResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> LockProjectAsync(int projectId, [FromServices]ILockProjectCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string user = User.Identity.Name;
            try
            {
                var response = await command.ExecuteAsync(projectId, user);
                return !response
                    ? (IActionResult)NotFound()
                    : Ok();
            }
            catch(ProjectLockedException)
            {
                return BadRequest("В данный момент проект уже заблокирован!");
            }
        }

        [Authorize]
        [HttpPut("{projectId}/unlock")]
        [ProducesResponseType(200, Type = typeof(ProjectResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UnlockProjectAsync(int projectId, [FromServices]IUnlockProjectCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string user = User.Identity.Name;
            await command.ExecuteAsync(projectId, user);

            return Ok();
        }
        
        //Удаление
        [Authorize]
        [HttpDelete("{projectId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> DeleteProjectAsync(int projectId, [FromServices]IDeleteProjectCommand command)
        {
            try
            {             
                if ((await _authorizationService.AuthorizeAsync(User, new Project(), Operations.Delete)).Succeeded)
                {
                    string user = User.Identity.Name;
                    await command.ExecuteAsync(projectId, user);
                    return NoContent();
                }

                return StatusCode(403, "Вы не можете удалить этот проект!");
            }
            catch (CannotDeleteProjectWithTasksException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ProjectLockedException)
            {
                return BadRequest("В данный момент удаление проекта невозможно!");
            }
        }
    }
}
