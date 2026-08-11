using CardioTrack.DTOs.Token;
using CardioTrack.Enums;

namespace CardioTrack.Interfaces.RefreshToken
{
    public interface ITokenService
    {
        Task<TokenResponseDto> IssueTokensAsync(int userId, string name, string email, string role, int? centerId, UserRole UserRole);
        Task<TokenResponseDto> RefreshAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}
