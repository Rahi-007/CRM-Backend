public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddAutoMapper(typeof(Program));
        services.AddProblemDetails();

        return services;
    }
}