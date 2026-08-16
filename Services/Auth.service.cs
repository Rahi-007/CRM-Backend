using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
{
    public readonly IMapper _mapper;
    private readonly AppDbContext _appDbContext;
    private readonly IJwtService _jwtService;

    public AuthService(
        AppDbContext appDbContext,
        IMapper mapper,
        IJwtService jwtService)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    public async Task<LoginResDto> UserLogin(LoginDto userData)
    {
        User? user = await _appDbContext.Users
            .Include(x => x.Team)
            .Include(x => x.Role!).ThenInclude(x => x.Permissions)
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .FirstOrDefaultAsync(x => x.Phone == userData.Phone);

        if (user is null)
            throw new Exception("Invalid phone or password.");

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
            userData.Password,
            user.Password
        );

        if (!isPasswordValid)
            throw new Exception("Invalid phone or password.");

        string accessToken = _jwtService.GenerateToken(user);

        return new LoginResDto
        {
            User = _mapper.Map<UserResDto>(user),
            AccessToken = accessToken,
            Permissions = user.Role?.Permissions.ToList() ?? []
        };
    }

}