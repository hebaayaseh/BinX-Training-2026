using CardioTrack.DTOs.AuditLog;
using CardioTrack.Interfaces.IAuditLog;

namespace CardioTrack.Tests.Services
{
    internal class FakeAuditLog : IAuditLog
    {
        public Task LogAsync(string action, string entityType, int entityId, object? oldValue = null, object? newValue = null)
            => Task.CompletedTask;

        public Task<AuditLogResponseDto> GetAuditLog()
            => Task.FromResult(new AuditLogResponseDto());
    }
}