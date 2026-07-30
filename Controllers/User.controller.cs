using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/v1/user")]
public class UserController : ControllerBase
{
    public readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // Get: api/v1/user => Read all user
    [HttpGet]
    public async Task<IActionResult> LoadUsers()
    {
        List<UserResDto> response = await _userService.GetAllUsers();
        return Ok(response);
    }

    // Get: api/v1/user/{userId} => Read a user
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        UserResDto? response = await _userService.GetUserById(userId);
        return response == null ? NotFound("User not found!") : Ok(response);
    }

    // Get: api/v1/user/select => Select users
    [HttpGet("select")]
    public async Task<IActionResult> SelectUsers()
    {
        List<SelectUserRes> response = await _userService.SelectUsers();
        return Ok(response);
    }

    // Post: api/v1/user => Create a new user
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto userData)
    {
        UserResDto newUser = await _userService.CreateUser(userData);
        return Created($"/api/v1/user/{newUser.Id}", newUser);
    }

    // Update: api/v1/user/{userId} => Update a user
    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto userData)
    {
        bool result = await _userService.UpdateUser(userId, userData);

        if (!result)
            return NotFound(new
            {
                success = false,
                message = "User not found"
            });

        return Ok(new
        {
            success = true,
            message = "User updated successfully"
        });
    }

    // Delete: api/v1/user/{userId} => Delete a user
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        bool response = await _userService.DeleteUser(userId);
        return response ? NoContent() : NotFound("User not found!");
    }
}