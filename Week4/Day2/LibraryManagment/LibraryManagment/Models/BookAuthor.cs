using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagment.Models
{
    public class BookAuthor
    {
        public int id { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        // Navigation Properties : 
        public Book? Book { get; set; }
        public Author? Author { get; set; }
    }
}
