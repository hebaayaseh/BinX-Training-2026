using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace LibraryManagment.Models
{
    public class Book
    {
        public int id { get; set; }
        [MaxLength(200)]
        public string title { get; set; }
        [MaxLength(13)]
        public string ISBN { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal price { get; set; }
        public DateTime publishDate { get; set; }
        public int categoryId { get; set; }

        // Navigation Properties :
        public ICollection<BookAuthor> bookAuthors { get; set; } = new List<BookAuthor>();
        public Category? category { get; set; }
        public ICollection<Borrowing> borrowing { get; set; } = new List<Borrowing>();

    }
}
