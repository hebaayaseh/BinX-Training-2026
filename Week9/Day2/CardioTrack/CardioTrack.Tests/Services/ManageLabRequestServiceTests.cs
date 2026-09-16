using CardioTrack.Data;
using CardioTrack.DTOs.LabRequest;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Models;
using CardioTrack.Services.LabRequest;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CardioTrack.Tests.Services
{
    /// <summary>
    /// GAP #3 (P1) — the whole lab-request flow had zero tests. This is the
    /// highest-value data in the system: a patient seeing another patient's lab
    /// requests is a privacy breach, and a doctor changing a lab status bypasses
    /// the technician workflow.
    /// </summary>
    public class ManageLabRequestServiceTests
    {
        private static CardioTrackDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<CardioTrackDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new CardioTrackDbContext(options);
        }

        private static User CreateUser(string email, UserRole role, bool isActive = true) => new User
        {
            FullName = $"{role} {email}",
            Email = email,
            PasswordHash = "hash",
            PhoneNumber = "0599000000",
            IsActive = isActive,
            Role = role
        };

        // ================= UpdateLabRequestStatusAsync =================

        [Fact]
        public async Task UpdateLabRequestStatus_CallerIsDoctor_ThrowsForbidden()
        {
            // Arrange
            await using var db = CreateDbContext();
            var doctor = CreateUser("doctor@test.com", UserRole.Doctor);
            var patient = new Patient { FullName = "P", PhoneNumber = "059", Address = "A", DoctorId = 1 };
            await db.users.AddAsync(doctor);
            await db.patients.AddAsync(patient);
            await db.SaveChangesAsync();

            var labRequest = new CardioTrack.Models.LabRequest
            {
                RequestedByDoctorId = doctor.Id,
                PatientId = patient.Id,
                TestName = "CBC",
                Status = LabRequestStatus.Pending
            };
            await db.labRequests.AddAsync(labRequest);
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            // Act & Assert — only Technician or Admin may move a lab request along
            await Assert.ThrowsAsync<ForbiddenException>(() =>
                service.UpdateLabRequestStatusAsync(doctor.Id, new UpdateLabRequestStatusRequestDto
                {
                    LabRequestId = labRequest.Id,
                    NewStatus = LabRequestStatus.Collected
                }));

            // and the status must be untouched
            var unchanged = await db.labRequests.FirstAsync(r => r.Id == labRequest.Id);
            Assert.Equal(LabRequestStatus.Pending, unchanged.Status);
        }

        [Fact]
        public async Task UpdateLabRequestStatus_CallerIsDeactivatedTechnician_ThrowsForbidden()
        {
            await using var db = CreateDbContext();
            var technician = CreateUser("tech@test.com", UserRole.Technician, isActive: false);
            var patient = new Patient { FullName = "P", PhoneNumber = "059", Address = "A", DoctorId = 1 };
            await db.users.AddAsync(technician);
            await db.patients.AddAsync(patient);
            await db.SaveChangesAsync();

            var labRequest = new CardioTrack.Models.LabRequest
            {
                RequestedByDoctorId = 1,
                PatientId = patient.Id,
                TestName = "CBC"
            };
            await db.labRequests.AddAsync(labRequest);
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                service.UpdateLabRequestStatusAsync(technician.Id, new UpdateLabRequestStatusRequestDto
                {
                    LabRequestId = labRequest.Id,
                    NewStatus = LabRequestStatus.Seen
                }));
        }

        [Fact]
        public async Task UpdateLabRequestStatus_RequestDoesNotExist_ThrowsNotFound()
        {
            await using var db = CreateDbContext();
            var technician = CreateUser("tech@test.com", UserRole.Technician);
            await db.users.AddAsync(technician);
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                service.UpdateLabRequestStatusAsync(technician.Id, new UpdateLabRequestStatusRequestDto
                {
                    LabRequestId = 9999,
                    NewStatus = LabRequestStatus.Seen
                }));
        }

        [Fact]
        public async Task UpdateLabRequestStatus_SetToCompletedManually_ThrowsBadRequest()
        {
            // Completed is only reached by uploading a result — setting it by hand
            // would leave a "completed" request with no result attached.
            await using var db = CreateDbContext();
            var technician = CreateUser("tech@test.com", UserRole.Technician);
            var patient = new Patient { FullName = "P", PhoneNumber = "059", Address = "A", DoctorId = 1 };
            await db.users.AddAsync(technician);
            await db.patients.AddAsync(patient);
            await db.SaveChangesAsync();

            var labRequest = new CardioTrack.Models.LabRequest
            {
                RequestedByDoctorId = 1,
                PatientId = patient.Id,
                TestName = "CBC"
            };
            await db.labRequests.AddAsync(labRequest);
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            await Assert.ThrowsAsync<BadRequestException>(() =>
                service.UpdateLabRequestStatusAsync(technician.Id, new UpdateLabRequestStatusRequestDto
                {
                    LabRequestId = labRequest.Id,
                    NewStatus = LabRequestStatus.Completed
                }));
        }

        [Fact]
        public async Task UpdateLabRequestStatus_ValidTechnician_UpdatesStatus()
        {
            await using var db = CreateDbContext();
            var technician = CreateUser("tech@test.com", UserRole.Technician);
            var patient = new Patient { FullName = "P", PhoneNumber = "059", Address = "A", DoctorId = 1 };
            await db.users.AddAsync(technician);
            await db.patients.AddAsync(patient);
            await db.SaveChangesAsync();

            var labRequest = new CardioTrack.Models.LabRequest
            {
                RequestedByDoctorId = 1,
                PatientId = patient.Id,
                TestName = "CBC",
                Status = LabRequestStatus.Pending
            };
            await db.labRequests.AddAsync(labRequest);
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            var message = await service.UpdateLabRequestStatusAsync(technician.Id,
                new UpdateLabRequestStatusRequestDto
                {
                    LabRequestId = labRequest.Id,
                    NewStatus = LabRequestStatus.Collected
                });

            Assert.Contains("Collected", message);
            var updated = await db.labRequests.FirstAsync(r => r.Id == labRequest.Id);
            Assert.Equal(LabRequestStatus.Collected, updated.Status);
        }

        // ================= GetLabRequestsAsync — data isolation =================

        [Fact]
        public async Task GetLabRequests_AsPatient_ReturnsOnlyOwnRequests()
        {
            // Arrange — two patients, each with one lab request
            await using var db = CreateDbContext();

            var doctor = CreateUser("doctor@test.com", UserRole.Doctor);
            var patientUserA = CreateUser("patientA@test.com", UserRole.Patient);
            var patientUserB = CreateUser("patientB@test.com", UserRole.Patient);
            await db.users.AddRangeAsync(doctor, patientUserA, patientUserB);
            await db.SaveChangesAsync();

            var patientA = new Patient
            {
                FullName = "Patient A",
                PhoneNumber = "0591",
                Address = "A",
                DoctorId = doctor.Id,
                LinkedUserId = patientUserA.Id
            };
            var patientB = new Patient
            {
                FullName = "Patient B",
                PhoneNumber = "0592",
                Address = "B",
                DoctorId = doctor.Id,
                LinkedUserId = patientUserB.Id
            };
            await db.patients.AddRangeAsync(patientA, patientB);
            await db.SaveChangesAsync();

            await db.labRequests.AddRangeAsync(
                new CardioTrack.Models.LabRequest
                {
                    RequestedByDoctorId = doctor.Id,
                    PatientId = patientA.Id,
                    TestName = "Troponin A"
                },
                new CardioTrack.Models.LabRequest
                {
                    RequestedByDoctorId = doctor.Id,
                    PatientId = patientB.Id,
                    TestName = "Troponin B"
                });
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            // Act — patient A asks for lab requests
            var results = await service.GetLabRequestsAsync(patientUserA.Id, new GetLabRequestsRequestDto());

            // Assert — patient B's data must not appear
            Assert.Single(results);
            Assert.Equal("Troponin A", results[0].TestName);
            Assert.DoesNotContain(results, r => r.PatientId == patientB.Id);
        }

        [Fact]
        public async Task GetLabRequests_AsPatient_CannotQueryAnotherPatientById()
        {
            // Even when the patient explicitly passes someone else's PatientId,
            // the role filter must win.
            await using var db = CreateDbContext();

            var doctor = CreateUser("doctor@test.com", UserRole.Doctor);
            var patientUserA = CreateUser("patientA@test.com", UserRole.Patient);
            await db.users.AddRangeAsync(doctor, patientUserA);
            await db.SaveChangesAsync();

            var patientA = new Patient
            {
                FullName = "Patient A",
                PhoneNumber = "0591",
                Address = "A",
                DoctorId = doctor.Id,
                LinkedUserId = patientUserA.Id
            };
            var patientB = new Patient
            {
                FullName = "Patient B",
                PhoneNumber = "0592",
                Address = "B",
                DoctorId = doctor.Id,
                LinkedUserId = null
            };
            await db.patients.AddRangeAsync(patientA, patientB);
            await db.SaveChangesAsync();

            await db.labRequests.AddAsync(new CardioTrack.Models.LabRequest
            {
                RequestedByDoctorId = doctor.Id,
                PatientId = patientB.Id,
                TestName = "Private Result"
            });
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            var results = await service.GetLabRequestsAsync(patientUserA.Id,
                new GetLabRequestsRequestDto { PatientId = patientB.Id });

            Assert.Empty(results);
        }

        [Fact]
        public async Task GetLabRequests_AsDeactivatedUser_ThrowsForbidden()
        {
            await using var db = CreateDbContext();
            var user = CreateUser("gone@test.com", UserRole.Doctor, isActive: false);
            await db.users.AddAsync(user);
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                service.GetLabRequestsAsync(user.Id, new GetLabRequestsRequestDto()));
        }

        // ================= CreateLabRequestAsync =================

        [Fact]
        public async Task CreateLabRequest_ValidDoctor_CreatesOneRequestPerTestName()
        {
            await using var db = CreateDbContext();
            var doctor = CreateUser("doctor@test.com", UserRole.Doctor);
            await db.users.AddAsync(doctor);
            await db.SaveChangesAsync();

            var patient = new Patient
            {
                FullName = "Test Patient",
                PhoneNumber = "0591",
                Address = "A",
                DoctorId = doctor.Id
            };
            await db.patients.AddAsync(patient);
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            var results = await service.CreateLabRequestAsync(doctor.Id, new CreateLabRequestDto
            {
                PatientId = patient.Id,
                TestNames = new List<string> { "CBC", "Lipid Panel", "Troponin" }
            });

            Assert.Equal(3, results.Count);
            Assert.All(results, r => Assert.Equal(LabRequestStatus.Pending, r.Status));
            Assert.All(results, r => Assert.Equal(doctor.Id, r.RequestedByDoctorId));
            Assert.Equal(3, await db.labRequests.CountAsync());
        }

        [Fact]
        public async Task CreateLabRequest_CallerIsNurse_ThrowsBadRequest()
        {
            await using var db = CreateDbContext();
            var nurse = CreateUser("nurse@test.com", UserRole.Nurse);
            await db.users.AddAsync(nurse);
            await db.SaveChangesAsync();

            var patient = new Patient { FullName = "P", PhoneNumber = "059", Address = "A", DoctorId = 1 };
            await db.patients.AddAsync(patient);
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            await Assert.ThrowsAsync<BadRequestException>(() =>
                service.CreateLabRequestAsync(nurse.Id, new CreateLabRequestDto
                {
                    PatientId = patient.Id,
                    TestNames = new List<string> { "CBC" }
                }));

            Assert.Equal(0, await db.labRequests.CountAsync());
        }

        [Fact]
        public async Task CreateLabRequest_PatientDoesNotExist_ThrowsBadRequest()
        {
            await using var db = CreateDbContext();
            var doctor = CreateUser("doctor@test.com", UserRole.Doctor);
            await db.users.AddAsync(doctor);
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            await Assert.ThrowsAsync<BadRequestException>(() =>
                service.CreateLabRequestAsync(doctor.Id, new CreateLabRequestDto
                {
                    PatientId = 9999,
                    TestNames = new List<string> { "CBC" }
                }));
        }

        [Fact]
        public async Task CreateLabRequest_EmptyTestNames_ThrowsBadRequest()
        {
            await using var db = CreateDbContext();
            var doctor = CreateUser("doctor@test.com", UserRole.Doctor);
            await db.users.AddAsync(doctor);
            await db.SaveChangesAsync();

            var patient = new Patient { FullName = "P", PhoneNumber = "059", Address = "A", DoctorId = doctor.Id };
            await db.patients.AddAsync(patient);
            await db.SaveChangesAsync();

            var service = new ManageLabRequestService(db, new FakeAuditLog());

            await Assert.ThrowsAsync<BadRequestException>(() =>
                service.CreateLabRequestAsync(doctor.Id, new CreateLabRequestDto
                {
                    PatientId = patient.Id,
                    TestNames = new List<string>()
                }));
        }
    }
}