using CardioTrack.Data;
using CardioTrack.DTOs.LabResult;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAuditLog;
using CardioTrack.Interfaces.ILabResult;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.LabResult
{
    public class ManageLabResultService : IManageLabResult
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly IAuditLog log;
        public ManageLabResultService(CardioTrackDbContext dbContext, IAuditLog log)
        {
            this.dbContext = dbContext;
            this.log = log;
        }

        public async Task<LabResultResponseDto> CreateLabResultAsync(int userId,CreateLabResultDto request)
        {
            var technician = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive
                                     && u.Role == UserRole.Technician);

            if (technician == null)
                throw new ForbiddenException("Auth forbidden");


            var patient = await dbContext.patients
                .FirstOrDefaultAsync(p => p.Id == request.PatientId);

            if (patient == null)
                throw new BadRequestException("Patient not found");


            string resultFileUrl = null;

            if (request.ResultFile != null && request.ResultFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "lab-results"
                );

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName =$"{Guid.NewGuid()}{Path.GetExtension(request.ResultFile.FileName)}";

                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath,FileMode.Create );

                await request.ResultFile.CopyToAsync(stream);

                resultFileUrl = $"/uploads/lab-results/{fileName}";
            }

            Models.LabRequest? labRequest = null;

            if (request.LabRequestId.HasValue)
            {
                labRequest = await dbContext.labRequests
                    .Include(r => r.LabResult)
                    .FirstOrDefaultAsync(r =>
                        r.Id == request.LabRequestId.Value
                        && r.PatientId == patient.Id);

                if (labRequest == null)
                    throw new BadRequestException("Lab request not found for this patient");

                if (labRequest.LabResult != null)
                    throw new ConflictException("A result has already been uploaded for this lab request");
            }

            var labResult = new Models.LabResult
            {
                PatientId = patient.Id,
                TechnicianId = technician.Id,
                ResultFileUrl = resultFileUrl,
                LabRequestId = request.LabRequestId,
                Status = LabStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await dbContext.labResults.AddAsync(labResult);

            if (labRequest != null)
            {
                var oldStatus = labRequest.Status;

                labRequest.Status = LabRequestStatus.Completed;

                await log.LogAsync(
                    "Auto-complete Lab Request",
                    "LabRequest",
                    labRequest.Id,
                    oldStatus,
                    labRequest.Status
                );
            }

            await dbContext.SaveChangesAsync();

            await log.LogAsync(
     "Create Lab Result",
     "LabResult",
     labResult.Id,
     null,
     new
     {
         labResult.PatientId,
         labResult.TechnicianId,
         labResult.LabRequestId,
         labResult.ResultFileUrl,
         labResult.Status
     }
 );


            return new LabResultResponseDto
            {
                Id = labResult.Id,
                PatientId = patient.Id,
                PatientName = patient.FullName,
                TechnicianId = technician.Id,
                TechnicianName = technician.FullName,
                LabRequestId = labResult.LabRequestId,
                TestName = labRequest?.TestName,
                ResultFileUrl = labResult.ResultFileUrl,
                Status = labResult.Status,
                CreatedAt = labResult.CreatedAt
            };
        }

        public async Task<List<LabResultResponseDto>> GetLabResultsAsync(int userId, GetLabResultDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
            if (user == null)
                throw new ForbiddenException("Auth forbidden");

            var query = dbContext.labResults
                .Include(r => r.Patient)
                .Include(r => r.Technician)
                .Include(r => r.LabRequest)
                .AsQueryable();

            if (user.Role == UserRole.Patient)
            {
                query = query.Where(r => r.Patient.LinkedUserId == user.Id);
            }
            else if (user.Role == UserRole.Doctor)
            {
                query = query.Where(r => r.Patient.DoctorId == user.Id);
            }

            if (request.PatientId.HasValue)
                query = query.Where(r => r.PatientId == request.PatientId.Value);

            if (request.Status.HasValue)
                query = query.Where(r => r.Status == request.Status.Value);

            var results = await query
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return results.Select(r => new LabResultResponseDto
            {
                Id = r.Id,
                PatientId = r.PatientId,
                PatientName = r.Patient.FullName,
                TechnicianId = r.TechnicianId,
                TechnicianName = r.Technician.FullName,
                LabRequestId = r.LabRequestId,
                TestName = r.LabRequest?.TestName,
                ResultFileUrl = r.ResultFileUrl,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<string> UpdateLabResultStatusAsync(int userId, UpdateLabResultStatusRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive
                                     && (u.Role == UserRole.Technician || u.Role == UserRole.Admin));
            if (user == null)
                throw new ForbiddenException("Auth forbidden");

            var labResult = await dbContext.labResults
                .FirstOrDefaultAsync(r => r.Id == request.LabResultId);
            if (labResult == null)
                throw new NotFoundException("Lab result not found");

            var oldStatus = labResult.Status;
            labResult.Status = request.NewStatus;

            await log.LogAsync("Update Lab Result Status", "LabResult", labResult.Id, oldStatus, labResult.Status);
            await dbContext.SaveChangesAsync();

            return $"Lab result status updated to {labResult.Status}";
        }
    }
}