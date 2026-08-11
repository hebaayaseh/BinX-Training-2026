using CardioTrack.Enums;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Data
{
    public class SeedData
    {
        private readonly CardioTrackDbContext dbContext;

        public SeedData(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SeedAllAsync()
        {
            await SeedAdminAsync();
            await SeedDoctorsAndNursesAsync();
            await SeedPatientsAsync();
            await SeedMedicalHistoryAsync();
            await SeedVitalSignsAsync();
            await SeedMedicationsAsync();
            await SeedAppointmentsAsync();
        }

        private async Task SeedAdminAsync()
        {
            bool adminExists = await dbContext.users
                .AnyAsync(u => u.Role == UserRole.Admin);
            if (adminExists) return;

            await dbContext.users.AddAsync(new User
            {
                FullName = "Heba Hesham",
                Email = "heba.ayaseh04@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Heba1234@"),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Role = UserRole.Admin,
                PhoneNumber = "123456789"
            });
            await dbContext.SaveChangesAsync();
        }

        private async Task SeedDoctorsAndNursesAsync()
        {
            bool doctorsExist = await dbContext.users
                .AnyAsync(u => u.Role == UserRole.Doctor);
            if (doctorsExist) return;

            var doctors = new List<User>
            {
                new User
                {
                    FullName = "Dr.Ahmad Ayaseh",
                    Email = "ahmadayaseh@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ahmad1234@"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Role = UserRole.Doctor,
                    PhoneNumber = "987654321"
                },
                new User
                {
                    FullName = "Dr.Souad Ayaseh",
                    Email = "souadayaseh@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Souad1234@"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Role = UserRole.Doctor,
                    PhoneNumber = "123987456"
                }
            };

            var nurses = new List<User>
            {
                new User
                {
                    FullName = "Nurse Sameer Ayaseh",
                    Email = "sameerayaseh@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Samerr1234@"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Role = UserRole.Nurse,
                    PhoneNumber = "741258963"
                },
                new User
                {
                    FullName = "Nurse Ameer Ayaseh",
                    Email = "ameerayaseh@gmail.com.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ameer1234@"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Role = UserRole.Nurse,
                    PhoneNumber = "369852147"
                }
            };

            await dbContext.users.AddRangeAsync(doctors);
            await dbContext.users.AddRangeAsync(nurses);
            await dbContext.SaveChangesAsync();
        }

        private async Task SeedPatientsAsync()
        {
            bool patientsExist = await dbContext.patients.AnyAsync();
            if (patientsExist) return;

            var doctor1 = await dbContext.users.FirstAsync(u => u.Email == "souadayaseh@gmail.com");
            var doctor2 = await dbContext.users.FirstAsync(u => u.Email == "ahmadayaseh@gmail.com");

            var patients = new List<Patient>
            {
                new Patient
                {
                    FullName = "Yazzan Ayaseh",
                    DateOfBirth = new DateTime(2000, 8, 24),
                    Gender = Gender.Male,
                    PhoneNumber = "521463987",
                    Address = "Jenin, Palestine",
                    BloodType = BloodType.A_Positive,
                    DoctorId = doctor1.Id,
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                },
                new Patient
                {
                    FullName = "Fatima Ayaseh",
                    DateOfBirth = new DateTime(2002, 12, 29),
                    Gender = Gender.Female,
                    PhoneNumber = "987456321",
                    Address = "Nablus, Palestine",
                    BloodType = BloodType.O_Negative,
                    DoctorId = doctor1.Id,
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                },
                new Patient
                {
                    FullName = "Ibrahim Ayaseh",
                    DateOfBirth = new DateTime(1997, 5, 7),
                    Gender = Gender.Male,
                    PhoneNumber = "456987123",
                    Address = "Jenin, Palestine",
                    BloodType = BloodType.AB_Negative,
                    DoctorId = doctor2.Id,
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                }
            };

            await dbContext.patients.AddRangeAsync(patients);
            await dbContext.SaveChangesAsync();
        }

        private async Task SeedMedicalHistoryAsync()
        {
            bool historyExists = await dbContext.medicalHistories.AnyAsync();
            if (historyExists) return;

            var ibrahim = await dbContext.patients.FirstAsync(p => p.FullName == "Ibrahim Ayaseh");
            var fatima = await dbContext.patients.FirstAsync(p => p.FullName == "Fatima Ayaseh");
            var doctor1 = await dbContext.users.FirstAsync(u => u.Email == "ahmadayaseh@gmail.com");
            var doctor2 = await dbContext.users.FirstAsync(u => u.Email == "souadayaseh@gmail.com");

            var histories = new List<MedicalHistory>
            {
                new MedicalHistory
                {
                    PatientId = ibrahim.Id,
                    Condition = "Hypertension",
                    Note = "Diagnosed 5 years ago, controlled with medication",
                    DiagnosisDate = new DateTime(2024, 6, 20),
                    RecordedByDoctorId = doctor1.Id
                },
                new MedicalHistory
                {
                    PatientId = fatima.Id,
                    Condition = "Type 2 Diabetes",
                    Note = "Requires regular monitoring",
                    DiagnosisDate = new DateTime(2020, 3, 25),
                    RecordedByDoctorId = doctor2.Id
                }
            };

            await dbContext.medicalHistories.AddRangeAsync(histories);
            await dbContext.SaveChangesAsync();
        }

        private async Task SeedVitalSignsAsync()
        {
            bool vitalsExist = await dbContext.vitalSigns.AnyAsync();
            if (vitalsExist) return;

            var yazan = await dbContext.patients.FirstAsync(p => p.FullName == "Yazan Ayash");
            var fatima = await dbContext.patients.FirstAsync(p => p.FullName == "Fatima Ayaseh");
            var nurse1 = await dbContext.users.FirstAsync(u => u.Email == "sameerayaseh@gmail.com");

            var vitalSigns = new List<VitalSign>
            {
                new VitalSign
                {
                    PatientId = yazan.Id,
                    RecordedAt = DateTime.UtcNow.AddDays(-2),
                    HeartRate = 78,
                    BloodPressureSystolic = 120,
                    BloodPressureDiastolic = 80,
                    OxygenSaturation = 98,
                    Temperature = 36.8M,
                    RecordedByUserId = nurse1.Id
                },
                new VitalSign
                {
                    PatientId = fatima.Id,
                    RecordedAt = DateTime.UtcNow.AddHours(-3),
                    HeartRate = 128,
                    BloodPressureSystolic = 155,
                    BloodPressureDiastolic = 95,
                    OxygenSaturation = 89,
                    Temperature = 37.5M,
                    RecordedByUserId = nurse1.Id
                }
            };

            await dbContext.vitalSigns.AddRangeAsync(vitalSigns);
            await dbContext.SaveChangesAsync();
        }

        private async Task SeedMedicationsAsync()
        {
            bool medsExist = await dbContext.medications.AnyAsync();
            if (medsExist) return;

            var yazan = await dbContext.patients.FirstAsync(p => p.FullName == "Yazan Ayaseh");
            var doctor1 = await dbContext.users.FirstAsync(u => u.Email == "ahmadayaseh@gmail.com");

            var medications = new List<Medication>
            {
                new Medication
                {
                    PatientId = yazan.Id,
                    DrugName = "Amlodipine",
                    Dosage = "5mg",
                    Frequency = "Once daily",
                    StartDate = DateTime.UtcNow.AddMonths(-6),
                    EndDate = null,
                    PrescribedByDoctorId = doctor1.Id,
                    IsActive = true
                }
            };

            await dbContext.medications.AddRangeAsync(medications);
            await dbContext.SaveChangesAsync();
        }

        private async Task SeedAppointmentsAsync()
        {
            bool appointmentsExist = await dbContext.appointments.AnyAsync();
            if (appointmentsExist) return;

            var yazan = await dbContext.patients.FirstAsync(p => p.FullName == "Yazan Ayaseh");
            var fatima = await dbContext.patients.FirstAsync(p => p.FullName == "Fatima Ayaseh");
            var doctor1 = await dbContext.users.FirstAsync(u => u.Email == "ahmadayaseh@gmail.com");
            var doctor2 = await dbContext.users.FirstAsync(u => u.Email == "souadayaseh@gmail.com");
            var nurse1 = await dbContext.users.FirstAsync(u => u.Email == "sameerayaseh@gmail.com");

            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    PatientId = yazan.Id,
                    DoctorId = doctor1.Id,
                    AppointmentDate = DateTime.UtcNow.AddDays(5),
                    Reason = "Routine follow-up",
                    Status = AppointmentStatus.Scheduled,
                    CreatedByUserId = nurse1.Id
                },
                new Appointment
                {
                    PatientId = fatima.Id,
                    DoctorId = doctor2.Id,
                    AppointmentDate = DateTime.UtcNow.AddDays(-3),
                    Reason = "Blood sugar check",
                    Status = AppointmentStatus.Completed,
                    CreatedByUserId = doctor2.Id
                }
            };

            await dbContext.appointments.AddRangeAsync(appointments);
            await dbContext.SaveChangesAsync();
        }
    }
}