using ObserverNetLite.Service.Abstractions;
using ObserverNetLite.Service.DTOs;

namespace ObserverNetLite.API.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/users").WithTags("Users");

            // Get all users
            group.MapGet("/", async (IUserService userService) =>
            {
                var users = await userService.GetAllUsersAsync();
                return Results.Ok(users);
            })
            .WithName("GetAllUsers")
            .WithDescription("Get all users");

            // Get user by id
            group.MapGet("/{id}", async (Guid id, IUserService userService) =>
            {
                var user = await userService.GetUserByIdAsync(id);
                return user is null ? Results.NotFound() : Results.Ok(user);
            })
            .WithName("GetUserById")
            .WithDescription("Get user by ID");

            // Create user
            group.MapPost("/", async (CreateUserDto createUserDto, IUserService userService) =>
            {
                var user = await userService.CreateUserAsync(createUserDto);
                return Results.Created($"/api/users/{user.Id}", user);
            })
            .WithName("CreateUser")
            .WithDescription("Create a new user");

            // Update user
            group.MapPut("/{id}", async (Guid id, UpdateUserRequest updateUserDto, IUserService userService) =>
            {
                var userDto = new UserDto
                {
                    Id = id,
                    UserName = updateUserDto.UserName ?? string.Empty,
                    Email = updateUserDto.Email,
                    RoleIds = updateUserDto.RoleIds ?? new List<Guid>()
                };
                var success = await userService.UpdateUserAsync(userDto);
                return success == null ? Results.NotFound() : Results.Ok(success);
            })
            .WithName("UpdateUser")
            .WithDescription("Update an existing user");

            // Delete user
            group.MapDelete("/{id}", async (Guid id, IUserService userService) =>
            {
                var success = await userService.DeleteUserAsync(id);
                return success ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteUser")
            .WithDescription("Delete a user");
        }
    }
}
