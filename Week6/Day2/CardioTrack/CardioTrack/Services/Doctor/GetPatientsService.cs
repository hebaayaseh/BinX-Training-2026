using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IDoctor;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Reflection.Metadata.Ecma335;

namespace CardioTrack.Services.Doctor
{
    public class GetPatientsService : IGetPatients
    {
        private readonly CardioTrackDbContext dbContext;
        public GetPatientsService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PatientsDto> GetPatientAsync(int userId, GetPatientRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && (u.Role == UserRole.Doctor
                                     ||  u.Role == UserRole.Nurse)
                                     && u.IsActive);

            if (user == null)
                throw new InvalidTokenException("Auth forbidden");

            var patient = await dbContext.patients
                    .FirstOrDefaultAsync(p => p.Id == request.PatientId);
            if (patient == null)
                throw new BadRequestException("Patient not found");


            if (user.Role == UserRole.Doctor && patient.DoctorId != userId)
                throw new ForbiddenException("Doctors can only manage their own appointments");
            

                return new PatientsDto
                {
                    PatientId = patient.Id,
                    PhoneNumber = patient.PhoneNumber,
                    Gender = patient.Gender,
                    BloodType = patient.BloodType,
                    Address = patient.Address,
                    FullName = patient.FullName,
                    DateOfBirth = patient.DateOfBirth,

                };

            
        }

        public async Task<GetPatientsDto> GetPatientsAsync(int userId)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.Role == UserRole.Doctor
                                     && u.IsActive);

            if (doctor == null)
                throw new InvalidTokenException("Auth forbidden");

            var patients = await dbContext.patients
                .Where(p => p.DoctorId == doctor.Id)
                .Select(d => new PatientsDto
                {
                    PatientId = d.Id,
                    PhoneNumber = d.PhoneNumber,
                    Gender = d.Gender,
                    BloodType = d.BloodType,
                    Address = d.Address,
                    FullName = d.FullName,
                    DateOfBirth =d.DateOfBirth,
                    
                }).ToListAsync();

            return new GetPatientsDto 
            {
                Patients = patients
            };


        }
    }
}
