public class LoginDto
{
    public required string Phone { get; set; }
    public required string Password { get; set; }
}
public class LoginResDto
{
    public UserResDto User { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public List<Permission> Permissions { get; set; } = [];
}