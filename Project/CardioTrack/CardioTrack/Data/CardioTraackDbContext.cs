using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Data
{
    public class CardioTraackDbContext : DbContext
    {
        public CardioTraackDbContext(DbContextOptions<CardioTraackDbContext> options)
        : base(options) { }

        public DbSet<User> users=> Set<User>();
        public DbSet<Patient> patients => Set<Patient>();
        public DbSet<VitalSign> vitalSigns => Set<VitalSign>();
        public DbSet<VitalSignAlert> vitalSignAlerts => Set<VitalSignAlert>();
        public DbSet<MedicalHistory> medicalHistories => Set<MedicalHistory>();
        public DbSet<Medication> medications => Set<Medication>();
        public DbSet<Appointment> appointments => Set<Appointment>();



    }
}
