using CardioTrack.Data;
using CardioTrack.DTOs.Admin;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAdmin;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Admin
{
    public class GetStaffService : IGetStaff
    {
        private readonly CardioTrackDbContext dbContext;
        public GetStaffService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<GetStaffResponseDto> GetStaffAsync(int userId)
        {
            var admin = await dbContext.users
                .FirstOrDefaultAsync(u=>u.Id == userId
                                     && u.IsActive
                                     && u.Role == UserRole.Admin);

            if (admin == null)
                throw new ForbiddenException("Auth forbidden");
            var doctors = await dbContext.users
                .Where(d => d.Role == UserRole.Doctor)
                .Select(d=>new DoctorDto
                {
                    DoctorId = d.Id,
                    DoctorName = d.FullName
                })
                .ToListAsync();

            var nurses = await dbContext.users
                .Where(n => n.Role == UserRole.Nurse)
                .Select(n=>new NurseDto { 
                    NurseId = n.Id,
                    NurseName = n.FullName
                }).ToListAsync();

            return new GetStaffResponseDto
            {
                Doctors = doctors,
                Nurses = nurses
            };
        }
    }
}
