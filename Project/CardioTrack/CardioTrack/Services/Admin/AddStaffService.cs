using CardioTrack.Data;
using CardioTrack.DTOs.Admin;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAdmin;
using CardioTrack.Interfaces.IEmail;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Admin
{
    public class AddStaffService : IAddStaff
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly IEmail email;
        public AddStaffService(CardioTrackDbContext dbContext , IEmail email)
        {
            this.dbContext = dbContext;
            this.email = email;
        }
        public async Task<string> AddDoctorAsync(int userId , AddDoctorRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive);

            if (user == null)
                throw new InvalidTokenException("Auth unauthorized");

            if (user.Role != UserRole.Admin)
                throw new ForbiddenException("Auth forbidden");

            var doctor = await dbContext.users
                .FirstOrDefaultAsync(e=>e.Email == request.Email);
            if (doctor != null)
                throw new ForbiddenException("Email exsist!");

            var password = GenerateTempPassword();

            await dbContext.AddAsync(new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                PhoneNumber = request.PhoneNumber,
                Role = UserRole.Doctor
            });
            await dbContext.SaveChangesAsync();

            await email.SendTempPasswordAsync(request.Email, request.FullName, password);
            return "تم التسجيل بنجاح.";

        }

        public async Task<string> AddNurseAsync(int userId, AddNurseRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive);

            if (user == null)
                throw new InvalidTokenException("Auth unauthorized");

            if (user.Role != UserRole.Admin)
                throw new ForbiddenException("Auth forbidden");

            var nurse = await dbContext.users
                .FirstOrDefaultAsync(e => e.Email == request.Email);
            if (nurse != null)
                throw new ForbiddenException("Email exsist!");

            var password = GenerateTempPassword();

            await dbContext.AddAsync(new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                PhoneNumber = request.PhoneNumber,
                Role = UserRole.Nurse
            });
            await dbContext.SaveChangesAsync();

            await email.SendTempPasswordAsync(request.Email, request.FullName, password);
            return "تم التسجيل بنجاح.";
        }

        private string GenerateTempPassword()
        {
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}
