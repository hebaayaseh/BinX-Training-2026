using CardioTrack.DTOs.Token;
using CardioTrack.Enums;

namespace CardioTrack.Interfaces.RefreshToken
{
    public interface ITokenService
    {
        Task<TokenResponseDto> IssueTokensAsync(int userId, string name, string email, UserRole role);
        Task<TokenResponseDto> RefreshAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}