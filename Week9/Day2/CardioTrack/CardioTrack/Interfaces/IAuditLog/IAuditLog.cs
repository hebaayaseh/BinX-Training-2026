using CardioTrack.DTOs.AuditLog;

namespace CardioTrack.Interfaces.IAuditLog
{
    public interface IAuditLog
    {
        Task LogAsync(string action, string entityType, int entityId, object? oldValue = null, object? newValue = null);
        Task<AuditLogResponseDto> GetAuditLog();
    }
}
