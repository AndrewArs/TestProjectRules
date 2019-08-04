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
        private readonly FilterService filterService;

        public ProjectsController(FilterService filterService)
        {
            this.filterService = filterService;
        }

        [HttpPost]
        public async Task<IActionResult> Filter([FromBody] ProjectsDto model)
        {
            await filterService.FilterProjects(model);
            return Ok();
        }
    }
}