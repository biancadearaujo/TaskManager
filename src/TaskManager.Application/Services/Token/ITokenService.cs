using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services.Token;

public interface ITokenService
{
    string GenerateJwtToken(UserEntity user);
}