using CardioTrack.Data;
using CardioTrack.DTOs.AuditLog;
using CardioTrack.Interfaces.IAuditLog;
using CardioTrack.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace CardioTrack.Services.AuditLogservice
{
    public class AuditLogService : IAuditLog
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AuditLogService(CardioTrackDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuditLogResponseDto> GetAuditLog()
        {
            var auditLogs = dbContext.auditLogs
                .Select(a => new AuditLogDto
                {
                    Id = a.Id,
                    PerformedByUserId = a.PerformedByUserId,
                    Action = a.Action,
                    EntityId = a.EntityId,
                    EntityName = a.EntityName,
                    NewValue = a.NewValue,
                    OldValue = a.OldValue,
                    Time=a.Time

                }).ToList();
            return new AuditLogResponseDto { auditLogs = auditLogs };

        }

        public async Task LogAsync(string action, string entityType, int entityId, object? oldValue = null, object? newValue = null)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null) return;

            var log = new AuditLog
            {
                PerformedByUserId = int.Parse(userIdClaim),
                Action = action,
                EntityName = entityType,
                EntityId = entityId,
                OldValue = oldValue != null ? JsonSerializer.Serialize(oldValue) : null,
                NewValue = newValue != null ? JsonSerializer.Serialize(newValue) : null,
                Time = DateTime.UtcNow
            };

            await dbContext.auditLogs.AddAsync(log);
            await dbContext.SaveChangesAsync();
        }
    }
}
