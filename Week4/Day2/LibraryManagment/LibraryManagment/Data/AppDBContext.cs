using LibraryManagment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagment.Data
{
    public class AppDBContext : IdentityDbContext<IdentityUser , IdentityRole, string>
    {

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<Author> authors => Set<Author>();
        public DbSet<Book> books => Set<Book>();
        public DbSet<Category> categories => Set<Category>();
        public DbSet<Borrowing> borrowings => Set<Borrowing>();
        public DbSet<Member> members => Set<Member>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BookAuthor>()
                .HasKey(e => new { e.BookId, e.AuthorId });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(e => e.Book)
                .WithMany(e => e.bookAuthors)
                .HasForeignKey(e => e.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(e => e.Author)
                .WithMany(e => e.bookAuthors)
                .HasForeignKey(e => e.AuthorId);

            modelBuilder.Entity<Book>()
                .HasOne(e => e.category)
                .WithMany(e => e.books)
                .HasForeignKey(e => e.categoryId);  
            
            modelBuilder.Entity<Borrowing>()
                .HasOne(e => e.Book)
                .WithMany(e => e.borrowing)
                .HasForeignKey(e => e.bookId);

            modelBuilder.Entity<Borrowing>()
                .HasOne(e => e.Member)
                .WithMany(e => e.borrowings)
                .HasForeignKey(e => e.memberId);    

        }

    }
}
