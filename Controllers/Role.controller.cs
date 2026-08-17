using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/v1/role")]
public class RoleController : ControllerBase
{
    public readonly IRoleService _roleService;
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    // Get: api/v1/role => Read all roles
    [HttpGet]
    public async Task<IActionResult> LoadRoles()
    {
        List<RoleResDto> response = await _roleService.GetAllRoles();
        return Ok(response);
    }

    // Get: api/v1/role/permission => Read all Permissions
    [HttpGet("permission")]
    public async Task<IActionResult> LoadPermissions()
    {
        List<PermissionResDto> response = await _roleService.GetAllPermissions();
        return Ok(response);
    }

    // Get: api/v1/role/{roleId} => Read a role
    [HttpGet("{roleId:int}")]
    public async Task<IActionResult> GetRole(int roleId)
    {
        RoleResDto? response = await _roleService.GetRoleById(roleId);
        return response == null ? NotFound("Role not found!") : Ok(response);
    }

    // Get: api/v1/role/select => Select roles
    [HttpGet("select")]
    public async Task<IActionResult> SelectRoles()
    {
        List<SelectRoleRes> response = await _roleService.SelectRoles();
        return Ok(response);
    }

    // Post: api/v1/project => Create a new project
    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleDto roleData)
    {
        int roleId = await _roleService.CreateRole(roleData);
        return Created($"/api/v1/role/{roleId}", roleId);
    }

    // Update: api/v1/project/{roleId} => Update a role
    [HttpPut("{roleId:int}")]
    public async Task<IActionResult> UpdateRole(int roleId, [FromBody] CreateRoleDto roleData)
    {
        bool result = await _roleService.UpdateRole(roleId, roleData);

        if (!result)
            return NotFound(new
            {
                success = false,
                message = "Role not found"
            });

        return Ok(new
        {
            success = true,
            message = "Role updated successfully"
        });
    }

    // Delete: api/v1/role/{roleId} => Delete a role
    [HttpDelete("{roleId:int}")]
    public async Task<IActionResult> DeleteRole(int roleId)
    {
        bool response = await _roleService.DeleteRole(roleId);
        return response ? NoContent() : NotFound("Role not found!");
    }
}