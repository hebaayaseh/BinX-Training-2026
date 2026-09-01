using BookNest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookNest.Data
{
    public class BookNestDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public BookNestDbContext(DbContextOptions<BookNestDbContext> options)
        : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<MemberProfile> MemberProfiles => Set<MemberProfile>();

     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  


            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservations");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).HasConversion<string>();

                entity.HasOne(e => e.Book)
                .WithMany(b => b.Reservations)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Member)
                .WithMany(m => m.Reservations)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MemberProfile>(entity =>
            {
                entity.ToTable("MemberProfiles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.ApplicationUserId).IsUnique();

                entity.HasOne(e => e.ApplicationUser)
            .WithOne()
            .HasForeignKey<MemberProfile>(e => e.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}