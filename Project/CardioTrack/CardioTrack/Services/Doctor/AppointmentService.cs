using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IDoctor;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Doctor
{
    public class AppointmentService : IAppointment
    {
        private readonly CardioTrackDbContext dbContext;
        public AppointmentService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<AddAppointmentResponseDto> AddAppointmentAsync(int userId, AddAppointmentRequestDto request)
        {
            var user = await dbContext.users
                 .FirstOrDefaultAsync(u => u.Id == userId
                         && u.IsActive
                         && (u.Role == UserRole.Doctor
                             || u.Role == UserRole.Nurse));

            if (user == null)
                throw new ForbiddenException("Auth forbidden");

            var patient = await dbContext.patients
                .Include(u => u.Doctor)
                .FirstOrDefaultAsync(p => p.Id == request.PatientId
                                     && p.DoctorId == request.DoctorId);

            if (patient == null)
                throw new BadRequestException("Patient not found");

            var appointments = await dbContext.appointments
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentDate == request.AppointmentDate
                                     && a.DoctorId == request.DoctorId);

            if (appointments != null)
                throw new BadRequestException("Date not invalid");

            var appointment = new Appointment
                { 
                    PatientId = request.PatientId,
                    AppointmentDate = request.AppointmentDate,
                    DoctorId = request.DoctorId,
                    Status = AppointmentStatus.Scheduled,
                    Reason = request.Reason,
                    CreatedByUserId = user.Id
                };

            await dbContext.AddAsync(appointment);
            await dbContext.SaveChangesAsync();
            return new AddAppointmentResponseDto
            {
                AppointmentId = appointment.Id,
                PatientName = patient.FullName,
                AppointmentDate = request.AppointmentDate,
                DoctorId = request.DoctorId
            };

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

            appointment.Status = AppointmentStatus.Completed;
            await dbContext.SaveChangesAsync();
            return "تم اكتمال الموعد.";
        }
    }
}
