using BookNest.Data;
using BookNest.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookNest.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly BookNestDbContext dbContext;

        public BooksController(BookNestDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await dbContext.Books
                .Select(b => new { b.Id, b.Title, b.Author, b.AvailableCopies })
                .ToListAsync();
            return Ok(books);
        }

        [Authorize(Policy = "MemberOnly")]
        [HttpPost("{id}/reserve")]
        public async Task<IActionResult> ReserveBook(int id)
        {
            var memberProfileId = int.Parse(User.FindFirst("memberProfileId")!.Value);

            var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                throw new NotFoundException("Book not found");

            if (book.AvailableCopies <= 0)
                throw new ConflictException("No available copies to reserve");

            book.AvailableCopies -= 1;
            dbContext.Reservations.Add(new Models.Reservation { BookId = id, MemberId = memberProfileId });
            await dbContext.SaveChangesAsync();

            return Ok($"Reserved '{book.Title}' successfully.");
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Models.Book book)
        {
            book.AvailableCopies = book.TotalCopies;
            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();
            return Ok(book);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("all-reservations")]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await dbContext.Reservations
                .Include(r => r.Book)
                .Include(r => r.Member)
                .Select(r => new { r.Id, BookTitle = r.Book!.Title, MemberName = r.Member!.UserName, r.Status })
                .ToListAsync();
            return Ok(reservations);
        }

        [Authorize(Policy = "MemberOnly")]
        [HttpGet("my-reservations")]
        public async Task<IActionResult> GetMyReservations()
        {
            var memberProfileId = int.Parse(User.FindFirst("memberProfileId")!.Value);

            var reservations = await dbContext.Reservations
                .Where(r => r.MemberId == memberProfileId)
                .Select(r => new
                {
                    r.Id,
                    BookTitle = r.Book!.Title,
                    r.ReservedAt,
                    r.Status
                })
                .ToListAsync();

            return Ok(reservations);
        }


        [Authorize(Policy = "MemberOnly")]
        [HttpGet("my-reservations/{id}")]
        public async Task<IActionResult> GetMyReservationById(int id)
        {
            var memberProfileId = int.Parse(User.FindFirst("memberProfileId")!.Value);

            var reservation = await dbContext.Reservations
                .Where(r => r.Id == id && r.MemberId == memberProfileId)
                .Select(r => new
                {
                    r.Id,
                    BookTitle = r.Book!.Title,
                    r.ReservedAt,
                    r.Status
                })
                .FirstOrDefaultAsync();

            if (reservation == null)
                return NotFound("Reservation not found");

            return Ok(reservation);
        }
    }
}