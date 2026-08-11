using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Data
{
    public class CardioTrackDbContext : DbContext
    {
        public CardioTrackDbContext(DbContextOptions<CardioTrackDbContext> options)
        : base(options) { }

        public DbSet<User> users => Set<User>();
        public DbSet<Patient> patients => Set<Patient>();
        public DbSet<VitalSign> vitalSigns => Set<VitalSign>();
        public DbSet<VitalSignAlert> vitalSignAlerts => Set<VitalSignAlert>();
        public DbSet<MedicalHistory> medicalHistories => Set<MedicalHistory>();
        public DbSet<Medication> medications => Set<Medication>();
        public DbSet<Appointment> appointments => Set<Appointment>();
        public DbSet<EmailVerificationCode> emailVerificationCodes => Set<EmailVerificationCode>();
        public DbSet<RefreshToken> refreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User 
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Id).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Role)
                .HasConversion<string>();

                entity.HasOne(e => e.LinkedPatient)
                .WithOne(p => p.LinkedUser)
                .OnDelete(DeleteBehavior.SetNull);
            });
            // Patient
            modelBuilder.Entity<Patient>(entity =>
            { 
                entity.ToTable("Patients");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.FullName).IsRequired();
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);
                entity.Property(e=> e.Address).HasMaxLength(100);
                entity.Property(e => e.BloodType)
                .HasConversion<string>();
                entity.Property(e=>e.Gender)
                .HasConversion<string>();

                entity.HasOne(e => e.Doctor)
                .WithMany(d => d.PatientsAsDoctor)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            // Medication
            modelBuilder.Entity<Medication>(entity =>
            {
                entity.ToTable("Medications")
                .HasKey(e => e.Id);
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.Dosage).HasMaxLength(50);
                entity.Property(e=>e.DrugName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Frequency).HasMaxLength(50);

                entity.HasOne(e=>e.Patient)
                .WithMany(e=>e.Medications)
                .HasForeignKey(e=>e.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.PrescribedByDoctor)
                .WithMany(e => e.Medications)
                .HasForeignKey(e => e.PrescribedByDoctorId)
                .OnDelete(DeleteBehavior.Restrict);
                
            });

            // MedicalHistory
            modelBuilder.Entity<MedicalHistory>(entity =>
            {
                entity.ToTable("Medical-Histories")
                .HasKey(e => e.Id);

                entity.Property(e => e.Note).HasMaxLength(200);
                entity.Property(e => e.Condition).HasMaxLength(200);

                entity.HasOne(e => e.Patient)
                .WithMany(e => e.MedicalHistories)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.RecordedByDoctor)
                .WithMany(e => e.MedicalHistories)
                .HasForeignKey(e => e.RecordedByDoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            // VitalSign 
            modelBuilder.Entity<VitalSign>(entity =>
            {
                entity.ToTable("Vital-Signs")
                .HasKey(e => e.Id);

                entity.HasOne(e => e.Patient)
                .WithMany(e => e.VitalSigns)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.RecordedByUser)
                .WithMany(e => e.VitalSigns)
                .HasForeignKey(e => e.RecordedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            // VitalSignAlert
            modelBuilder.Entity<VitalSignAlert>(entity =>
            {
                entity.ToTable("Vital-Sign-Alerts")
                .HasKey(e => e.Id);

                entity.Property(e => e.Message).HasMaxLength(200);
                entity.Property(e => e.AlterType)
                .HasConversion<string>();
                entity.Property(e => e.Severity)
                .HasConversion<string>();

                entity.HasOne(e => e.Patient)
                .WithMany(e => e.VitalSignAlerts)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.VitalSign)
                .WithMany(e => e.VitalSignAlerts)
                .HasForeignKey(e => e.VitalSignId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            // Appointment 
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointments")
                .HasKey(e => e.Id);

                entity.Property(e => e.Status)
                .HasConversion<string>();
                entity.Property(e => e.Reason).HasMaxLength(200);

                entity.HasOne(e => e.Doctor)
                .WithMany(d => d.AppointmentsAsDoctor)   
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.CreatedByUser)
                .WithMany(u => u.AppointmentsCreated)      
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Patient)
                .WithMany(e => e.Appointments)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

                

            });

            // EmailVerificationCode 
            modelBuilder.Entity<EmailVerificationCode>(entity =>
            {
                entity.ToTable("EmailVerificationCode")
                .HasKey(e => e.Id);

                entity.Property(e => e.Purpose).HasMaxLength(50);

                
                entity.HasOne(e => e.User)
                .WithMany(e => e.EmailVerificationCodes)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            });

        }

    }
}

