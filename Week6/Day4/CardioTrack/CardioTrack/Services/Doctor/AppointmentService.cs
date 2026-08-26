using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAuditLog;
using CardioTrack.Interfaces.IDoctor;
using CardioTrack.Models;
using CardioTrack.Services.AuditLogservice;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Doctor
{
    public class AppointmentService : IAppointment
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly IAuditLog log;
        public AppointmentService(CardioTrackDbContext dbContext,IAuditLog log)
        {
            this.dbContext = dbContext;
            this.log = log;
        }
        public async Task<AddAppointmentResponseDto> AddAppointmentAsync(int userId, AddAppointmentRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive
                                     && (u.Role == UserRole.Doctor || u.Role == UserRole.Nurse));
            if (user == null)
                throw new ForbiddenException("Auth forbidden");

            var patient = await dbContext.patients
                .FirstOrDefaultAsync(p => p.Id == request.PatientId && p.DoctorId == request.DoctorId);
            if (patient == null)
                throw new BadRequestException("Patient not found");

            var conflictExists = await dbContext.appointments
                .AnyAsync(a => a.DoctorId == request.DoctorId
                          && a.AppointmentDate == request.AppointmentDate
                          && a.Status == AppointmentStatus.Scheduled);
            if (conflictExists)
                throw new ConflictException("This time slot is already booked for the selected doctor");

            decimal fee = CalculateAppointmentFee(request.Reason);

            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var appointment = new Appointment
                {
                    PatientId = request.PatientId,
                    AppointmentDate = request.AppointmentDate,
                    DoctorId = request.DoctorId,
                    Status = AppointmentStatus.Scheduled,
                    Reason = request.Reason,
                    Fee = fee,
                    CreatedByUserId = user.Id
                };
                await dbContext.AddAsync(appointment);

                if (request.RelatedAlertId.HasValue)
                {
                    var alert = await dbContext.vitalSignAlerts
                        .FirstOrDefaultAsync(a => a.Id == request.RelatedAlertId.Value && !a.IsResolved);

                    if (alert == null)
                        throw new BadRequestException("Related alert not found or already resolved");

                    alert.IsResolved = true;
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new AddAppointmentResponseDto
                {
                    AppointmentId = appointment.Id,
                    PatientName = patient.FullName,
                    AppointmentDate = request.AppointmentDate,
                    DoctorId = request.DoctorId
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private decimal CalculateAppointmentFee(string reason)
        {
            return reason?.ToLower() switch
            {
                var r when r != null && r.Contains("emergency") => 150m,
                var r when r != null && r.Contains("follow-up") => 50m,
                var r when r != null && r.Contains("routine") => 75m,
                _ => 100m   
            };
        }

        public async Task<string> CancelAppointmentAsync(int userId, CancelAppointmentRequestDto request)
        {
            var user = await dbContext.users
                 .FirstOrDefaultAsync(u => u.Id == userId
                         && u.IsActive
                         && (u.Role == UserRole.Doctor
                             || u.Role == UserRole.Nurse));

            if (user == null)
                throw new ForbiddenException("Auth forbidden");

            var appointment = await dbContext.appointments
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId
                                     && a.DoctorId == request.DoctorId
                                     && a.Status == AppointmentStatus.Scheduled);

            if (appointment == null)
                throw new BadRequestException("Appointment not found");
            var oldValue = appointment;
            appointment.Status = AppointmentStatus.Canceled;

            await log.LogAsync("Canceled appointment ", "Appointment", appointment.Id, oldValue, appointment);
            await dbContext.SaveChangesAsync();
            return "تم الغاء الموعد.";
        }

        public async Task<string> CompleteAppointmentAsync(int userId, CompleteAppointmentRequestDto request)
        {
            var user = await dbContext.users
                 .FirstOrDefaultAsync(u => u.Id == userId
                         && u.IsActive
                         && (u.Role == UserRole.Doctor
                             || u.Role == UserRole.Nurse));

            if (user == null)
                throw new ForbiddenException("Auth forbidden");

            var appointment = await dbContext.appointments
                .Include(d=>d.Doctor)
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId
                                     && a.DoctorId == request.DoctorId
                                     && a.Status == AppointmentStatus.Scheduled);

            if (appointment == null)
                throw new BadRequestException("Appointment not found");
            var oldValue = appointment;
            appointment.Status = AppointmentStatus.Completed;

            await log.LogAsync("Schedule appointment ", "Appointment", appointment.Id, oldValue, appointment);
            await dbContext.SaveChangesAsync();
            return "تم اكتمال الموعد.";
        }

        public async Task<GetAppointmentResponseDto> GetAppointmentToNurseAsync(int userId, GetAppointmentsRequestDto request)
        {
            var user = await dbContext.users
                 .FirstOrDefaultAsync(u => u.Id == userId
                         && u.IsActive
                         &&  u.Role == UserRole.Nurse);

            if (user == null)
                throw new ForbiddenException("Auth forbidden");

            var doctor = await dbContext.users
                .FirstOrDefaultAsync(d => d.Id == request.DoctorId
                                     && d.IsActive
                                     && d.Role == UserRole.Doctor);

            if (doctor == null)
                throw new BadRequestException("Doctor not found");

            var appointments = await dbContext.appointments
                .Include(d => d.Doctor)
                .Where(a => a.Status == request.AppointmentStatus
                       && a.DoctorId == request.DoctorId)
                .Select(d => new AppointmentDto 
                { 
                    AppointmentId = d.Id,
                    AppointmentDate = d.AppointmentDate
                }).ToListAsync();

            return new GetAppointmentResponseDto 
            {
                Appointments = appointments 
            };


        }

        public async Task<GetAppointmentResponseDto> GetAppointmentToDuctorAsync(int userId, GetDoctorAppointmentRequestDto request)
        {
            var user = await dbContext.users
                  .FirstOrDefaultAsync(u => u.Id == userId
                          && u.IsActive
                          && u.Role == UserRole.Doctor);

            if (user == null)
                throw new ForbiddenException("Auth forbidden");

           

            var appointments = await dbContext.appointments
                .Include(d => d.Doctor)
                .Where(a => a.Status == request.AppointmentStatus
                       && a.DoctorId == user.Id)
                .Select(d => new AppointmentDto
                {
                    AppointmentId = d.Id,
                    AppointmentDate = d.AppointmentDate
                }).ToListAsync();

            return new GetAppointmentResponseDto
            {
                Appointments = appointments
            };
        }

        
    }
}
