using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/v1/team")]
public class TeamController : ControllerBase
{
    public readonly ITeamService _teamService;
    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    // Get: api/v1/team => Read all teams
    [HttpGet]
    public async Task<IActionResult> LoadUsers()
    {
        List<TeamResDto> response = await _teamService.GetAllTeams();
        return Ok(response);
    }

    // Get: api/v1/team/{teamId} => Read a user
    [HttpGet("{teamId:int}")]
    public async Task<IActionResult> GetTeam(int teamId)
    {
        TeamResDto? response = await _teamService.GetTeamById(teamId);
        return response == null ? NotFound("Team not found!") : Ok(response);
    }

    // Get: api/v1/team/select => Select teams
    [HttpGet("select")]
    public async Task<IActionResult> SelectTeams()
    {
        List<SelectTeamRes> response = await _teamService.SelectTeams();
        return Ok(response);
    }

    // Post: api/v1/team => Create a new team
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateTeamDto teamData)
    {
        int teamId = await _teamService.CreateTeam(teamData);
        return Created($"/api/v1/user/{teamId}", teamId);
    }

    // Update: api/v1/team/{teamId} => Update a Team
    [HttpPut("{teamId:int}")]
    public async Task<IActionResult> UpdateTeam(int teamId, [FromBody] CreateTeamDto teamData)
    {
        bool result = await _teamService.UpdateTeam(teamId, teamData);

        if (!result)
            return NotFound(new
            {
                success = false,
                message = "Team not found"
            });

        return Ok(new
        {
            success = true,
            message = "Team updated successfully"
        });
    }

    // Delete: api/v1/team/{teamId} => Delete a team
    [HttpDelete("{teamId:int}")]
    public async Task<IActionResult> DeleteTeam(int teamId)
    {
        bool response = await _teamService.DeleteTeam(teamId);
        return response ? NoContent() : NotFound("Team not found!");
    }
}