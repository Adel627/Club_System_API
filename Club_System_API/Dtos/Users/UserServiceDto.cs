namespace Club_System_API.Dtos.Users;

public record UserServiceDto(
    string ServiceName,
    DateTime? StartDate,
    DateTime? EndDate,
    string? Description,
    string? ImageBase64
);
