using System.Threading.Tasks;
using Dtos.Projects;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace TestProjectRules.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly FilterService _filterService;

        public ProjectsController(FilterService filterService)
        {
            _filterService = filterService;
        }

        [HttpPost]
        public async Task<IActionResult> Filter([FromBody] ProjectsDto model)
        {
            await _filterService.FilterProjects(model);
            return Ok();
        }
    }
}