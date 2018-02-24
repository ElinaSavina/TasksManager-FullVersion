using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TasksManager.DataAccess.Interfaces.Tags;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tags;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ListResponse<TagResponse>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTagsListAsync(TagFilter filter, ListOptions options, [FromServices]ITagsListQuery query)
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

        [HttpDelete("tags/{tag}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteTagAsync(string tag, [FromServices]IDeleteTagCommand command)
        {
            await command.ExecuteAsync(tag);
            return NoContent();
        }
    }
}
