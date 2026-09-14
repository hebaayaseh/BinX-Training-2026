using CardioTrack.Data;
using CardioTrack.DTOs.LabRequest;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAuditLog;
using CardioTrack.Interfaces.ILabRequest;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.LabRequest
{
    public class ManageLabRequestService : IManageLabRequest
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly IAuditLog log;
        public ManageLabRequestService(CardioTrackDbContext dbContext, IAuditLog log)
        {
            this.dbContext = dbContext;
            this.log = log;
        }

        public async Task<List<LabRequestResponseDto>> CreateLabRequestAsync(int doctorUserId, CreateLabRequestDto request)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(d => d.Id == doctorUserId
                                     && d.IsActive
                                     && d.Role == UserRole.Doctor);
            if (doctor == null)
                throw new BadRequestException("Doctor not found");

            var patient = await dbContext.patients
                .FirstOrDefaultAsync(p => p.Id == request.PatientId);
            if (patient == null)
                throw new BadRequestException("Patient not found");

            if (request.TestNames == null || !request.TestNames.Any())
                throw new BadRequestException("Must add at least one test");

            if (request.AppointmentId.HasValue)
            {
                var appointmentExists = await dbContext.appointments
                    .AnyAsync(a => a.Id == request.AppointmentId.Value && a.PatientId == patient.Id);
                if (!appointmentExists)
                    throw new BadRequestException("Appointment not found for this patient");
            }

            var newRequests = request.TestNames
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(testName => new Models.LabRequest
                {
                    RequestedByDoctorId = doctor.Id,
                    PatientId = patient.Id,
                    AppointmentId = request.AppointmentId,
                    TestName = testName.Trim(),
                    Status = LabRequestStatus.Pending,
                    RequstedAt = DateTime.UtcNow
                })
                .ToList();

            await dbContext.labRequests.AddRangeAsync(newRequests);

            foreach (var labRequest in newRequests)
                await log.LogAsync("Create Lab Request", "LabRequest", labRequest.Id, null, new
                {
                    labRequest.PatientId,
                    labRequest.RequestedByDoctorId,
                    labRequest.AppointmentId,
                    labRequest.TestName,
                    labRequest.Status
                });

            await dbContext.SaveChangesAsync();

            return newRequests.Select(r => new LabRequestResponseDto
            {
                Id = r.Id,
                PatientId = patient.Id,
                PatientName = patient.FullName,
                RequestedByDoctorId = doctor.Id,
                DoctorName = doctor.FullName,
                AppointmentId = r.AppointmentId,
                TestName = r.TestName,
                Status = r.Status,
                RequstedAt = r.RequstedAt
            }).ToList();
        }

        public async Task<List<LabRequestResponseDto>> GetLabRequestsAsync(int userId, GetLabRequestsRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
            if (user == null)
                throw new ForbiddenException("Auth forbidden");

            var query = dbContext.labRequests
                .Include(r => r.Patient)
                .Include(r => r.RequestedByDoctor)
                .AsQueryable();

            if (user.Role == UserRole.Patient)
            {
                query = query.Where(r => r.Patient.LinkedUserId == user.Id);
            }
            else if (user.Role == UserRole.Doctor)
            {
                query = query.Where(r => r.RequestedByDoctorId == user.Id);
            }

            if (request.PatientId.HasValue)
                query = query.Where(r => r.PatientId == request.PatientId.Value);

            if (request.Status.HasValue)
                query = query.Where(r => r.Status == request.Status.Value);

            var results = await query
                .OrderByDescending(r => r.RequstedAt)
                .ToListAsync();

            return results.Select(r => new LabRequestResponseDto
            {
                Id = r.Id,
                PatientId = r.PatientId,
                PatientName = r.Patient.FullName,
                RequestedByDoctorId = r.RequestedByDoctorId,
                DoctorName = r.RequestedByDoctor?.FullName,
                AppointmentId = r.AppointmentId,
                TestName = r.TestName,
                Status = r.Status,
                RequstedAt = r.RequstedAt
            }).ToList();
        }

        public async Task<string> UpdateLabRequestStatusAsync(int userId, UpdateLabRequestStatusRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive
                                     && (u.Role == UserRole.Technician || u.Role == UserRole.Admin));
            if (user == null)
                throw new ForbiddenException("Auth forbidden");

            var labRequest = await dbContext.labRequests
                .FirstOrDefaultAsync(r => r.Id == request.LabRequestId);
            if (labRequest == null)
                throw new NotFoundException("Lab request not found");

            if (request.NewStatus == LabRequestStatus.Completed)
                throw new BadRequestException("Status 'Completed' is set automatically when a result is uploaded");

            var oldStatus = labRequest.Status;
            labRequest.Status = request.NewStatus;

            await log.LogAsync("Update Lab Request Status", "LabRequest", labRequest.Id, oldStatus, labRequest.Status);
            await dbContext.SaveChangesAsync();

            return $"Lab request status updated to {labRequest.Status}";
        }
    }
}