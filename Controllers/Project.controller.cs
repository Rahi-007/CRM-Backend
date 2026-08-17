using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/v1/project")]
public class ProjectController : ControllerBase
{
    public readonly IProjectService _projectService;
    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // Get: api/v1/project => Read all project
    [HttpGet]
    public async Task<IActionResult> LoadProjects()
    {
        List<ProjectResDto> response = await _projectService.GetAllProjects();
        return Ok(response);
    }

    // Get: api/v1/project/{projectId} => Read a project
    [HttpGet("{projectId:int}")]
    public async Task<IActionResult> GetProject(int projectId)
    {
        ProjectResDto? response = await _projectService.GetProjectById(projectId);
        return response == null ? NotFound("Project not found!") : Ok(response);
    }

    // Get: api/v1/project/select => Select project
    [HttpGet("select")]
    public async Task<IActionResult> SelectProjects()
    {
        List<SelectProjectRes> response = await _projectService.SelectProjects();
        return Ok(response);
    }

    // Post: api/v1/project => Create a new project
    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectDto projectData)
    {
        int projectId = await _projectService.CreateProject(projectData);
        return Created($"/api/v1/project/{projectId}", projectId);
    }

    // Update: api/v1/project/{projectId} => Update a project
    [HttpPut("{projectId:int}")]
    public async Task<IActionResult> UpdateProject(int projectId, [FromBody] CreateProjectDto projectData)
    {
        bool result = await _projectService.UpdateProject(projectId, projectData);

        if (!result)
            return NotFound(new
            {
                success = false,
                message = "Project not found"
            });

        return Ok(new
        {
            success = true,
            message = "Project updated successfully"
        });
    }

    // Delete: api/v1/project/{projectId} => Delete a project
    [HttpDelete("{projectid:int}")]
    public async Task<IActionResult> DeleteProject(int projectId)
    {
        bool response = await _projectService.DeleteProject(projectId);
        return response ? NoContent() : NotFound("Project not found!");
    }
}