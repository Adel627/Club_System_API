
using Club_System_API.Abstractions;
using Club_System_API.Dtos.Users;

namespace Club_System_API.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetAsync(string id);
    Task<Result<AddingUserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
   
    Task<Result> ToggleStatus(string id);
    Task<Result> Unlock(string id);
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
}