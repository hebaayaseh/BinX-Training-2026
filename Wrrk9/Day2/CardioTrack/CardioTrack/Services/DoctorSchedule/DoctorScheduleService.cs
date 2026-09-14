using CardioTrack.Data;
using CardioTrack.DTOs.DoctorSchedule;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAuditLog;
using CardioTrack.Interfaces.IDoctorSchedule;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.DoctorSchedule
{
    public class DoctorScheduleService : IDoctorSchedule
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly IAuditLog log;
        public DoctorScheduleService(CardioTrackDbContext dbContext,IAuditLog log)
        {
            this.dbContext = dbContext;
            this.log = log;
        }
        public async Task<AddDoctorScheduleResponseDto> AddDoctorSheduleAsync(int userId, AddDoctorSheduleRequestDto request)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(d => d.Id == userId 
                                     && d.IsActive
                                     && d.Role == UserRole.Doctor);

            if (doctor == null)
                throw new BadRequestException("Doctor not found");

            var hasConflict = await dbContext.doctorschedules
             .AnyAsync(s => s.DoctorId == doctor.Id
                   && s.DayOfWeek == request.DayOfWeek
                   && request.StartTime < s.EndTime
                   && request.EndTime > s.StartTime);
            if (hasConflict)
                throw new BadRequestException("Schedule conflict: overlapping time slot exists for this doctor");

            var doctorScheduale = new Doctorschedule
            {
                DoctorId = doctor.Id,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsActive = true
            };

            await dbContext.doctorschedules.AddAsync(doctorScheduale);
            await log.LogAsync("Add Doctor Schedule", "DoctorSchedule", doctorScheduale.Id, null, doctorScheduale);
            await dbContext.SaveChangesAsync();

            return new AddDoctorScheduleResponseDto
            {
                ScheduleId = doctorScheduale.Id,
                DoctorId = doctor.Id,
                DayOfWeek = doctorScheduale.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };
        }


        public async Task<AddDoctorScheduleResponseDto> UpdaeDoctorScheduleAsync(UpdateDoctorScheduleRequestDto request)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(d => d.Id == request.DoctorId
                                     && d.Role == UserRole.Doctor
                                     && d.IsActive);
            if (doctor == null)
                throw new BadRequestException("Doctor not found");

            var doctorSchedual = await dbContext.doctorschedules
                .FirstOrDefaultAsync(d => d.DoctorId == doctor.Id
                    && d.Id == request.ScheduleId
                    && d.IsActive);
            if (doctorSchedual == null)
                throw new BadRequestException("Schedule not found");


            var hasConflict = await dbContext.doctorschedules
                .AnyAsync(s => s.DoctorId == doctor.Id
                    && s.Id != doctorSchedual.Id
                    && s.IsActive
                    && s.DayOfWeek == request.DayOfWeek
                    && request.StartTime < s.EndTime
                    && request.EndTime > s.StartTime);
            if (hasConflict)
                throw new BadRequestException("Schedule conflict: overlapping time slot exists for this doctor");


            var affectedAppointments = await dbContext.appointments
                .Where(a => a.DoctorId == doctor.Id
                    && a.Status == AppointmentStatus.Scheduled
                    && a.AppointmentDate.DayOfWeek == doctorSchedual.DayOfWeek)
                .ToListAsync();

            foreach (var appointment in affectedAppointments)
            {
                appointment.Status = AppointmentStatus.Postponed; 
            }

            var oldValue = $"Day: {doctorSchedual.DayOfWeek}, {doctorSchedual.StartTime}-{doctorSchedual.EndTime}";
            doctorSchedual.IsActive = false;

            doctorSchedual.IsActive = false;

            var newSchedule = new Doctorschedule
            {
                DoctorId = doctor.Id,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };
            await dbContext.doctorschedules.AddAsync(newSchedule);
            await log.LogAsync("Add Doctor Schedule", "DoctorSchedule", doctorSchedual.Id, oldValue, doctorSchedual);
            await dbContext.SaveChangesAsync();

            return new AddDoctorScheduleResponseDto
            {
                DoctorId = doctor.Id,
                DayOfWeek = newSchedule.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };
        }
    }
}
