public interface IUserService
{
    Task<List<UserResDto>> GetAllUsers();
    Task<List<SelectUserRes>> SelectUsers();
    Task<UserResDto?> GetUserById(Guid userId);
    Task<UserResDto> CreateUser(CreateUserDto createData);
    Task<bool> UpdateUser(Guid userId, UpdateUserDto updateData);
    Task<bool> DeleteUser(Guid userId);
}