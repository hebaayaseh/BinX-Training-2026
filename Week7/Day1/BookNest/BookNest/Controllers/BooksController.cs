using BookNest.Data;
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
            var memberId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                return NotFound("Book not found");

            if (book.AvailableCopies <= 0)
                return Conflict("No available copies to reserve");

            book.AvailableCopies -= 1;

            dbContext.Reservations.Add(new Models.Reservation
            {
                BookId = id,
                MemberId = memberId
            });

            await dbContext.SaveChangesAsync();
            return Ok($"Reserved '{book.Title}' successfully. Remaining copies: {book.AvailableCopies}");
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
    }
}