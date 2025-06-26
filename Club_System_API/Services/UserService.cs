using Club_System_API.Abstractions;
using Club_System_API.Abstractions.Consts;
using Club_System_API.Dtos.Users;
using Club_System_API.Errors;
using Club_System_API.Helper;
using Club_System_API.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Club_System_API.Services;

public class UserService(UserManager<ApplicationUser> userManager,

    ApplicationDbContext context) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    private readonly ApplicationDbContext _context = context;


    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
    {
        var user = await _userManager.Users
            .Where(x => x.Id == userId)
            .ProjectToType<UserProfileResponse>()
            .SingleAsync();

        return Result.Success(user);
    }

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {

        await _userManager.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(x => x.FirstName, request.FirstName)
                    .SetProperty(x => x.LastName, request.LastName)
                    .SetProperty(x => x.Image, FormFileExtensions.ConvertToBytes(request.Image))
            );

        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }


    public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
             await (from u in _context.Users
                    join ur in _context.UserRoles
                    on u.Id equals ur.UserId into userRoles
                    from ur in userRoles.DefaultIfEmpty()  // Ensure users without roles are included
                    join r in _context.Roles
                    on ur.RoleId equals r.Id into roles
                    from r in roles.DefaultIfEmpty()
                   // where r == null || r.Name != DefaultRoles.Member // Fix filtering
                    select new
                    {
                        u.Id,
                        u.MembershipNumber,
                        u.PhoneNumber,
                        u.FirstName,
                        u.LastName,
                        u.Birth_Of_Date,
                        u.MembershipId,
                        u.Image,
                        u.IsDisabled,
                        RoleName = r != null ? r.Name : null
                    })
                    .GroupBy(u => new { u.Id,u.PhoneNumber, u.FirstName, u.LastName, u.MembershipNumber, u.Birth_Of_Date, u.Image, u.MembershipId,  u.IsDisabled })
                    .Select(u => new UserResponse
                    (
                        u.Key.Id,
                        u.Key.MembershipNumber,
                        u.Key.PhoneNumber,
                        u.Key.FirstName,
                        u.Key.LastName,
                        u.Key.Birth_Of_Date,
                        u.Key.Image,
                        u.Key.MembershipId,
                        u.Key.IsDisabled,
                        u.Where(x => x.RoleName != null).Select(x => x.RoleName).Distinct().ToList() // Ensure distinct roles
                    ))
                    .ToListAsync(cancellationToken);


    public async Task<Result<UserResponse>> GetAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        var userRoles = await _userManager.GetRolesAsync(user);
        

        var response = (user, userRoles).Adapt<UserResponse>();

        return Result.Success(response);
    }

    public async Task<Result<AddingUserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {

        var user = request.Adapt<ApplicationUser>();
        user.MembershipNumber = GenerateMembershipNumberExtensions.GenerateMembershipNumber();
        user.UserName = user.MembershipNumber;
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded && request.IsMember)
        {
            await _userManager.AddToRoleAsync(user, DefaultRoles.Member);

            var response = user.Adapt<AddingUserResponse>();


            return Result.Success(response);
        }
        if (result.Succeeded && !request.IsMember)
        {
            await _userManager.AddToRoleAsync(user, DefaultRoles.Admin);

            var response = user.Adapt<AddingUserResponse>();


            return Result.Success(response);
        }

        var error = result.Errors.First();

        return Result.Failure<AddingUserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> ToggleStatus(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        user.IsDisabled = !user.IsDisabled;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }


}
