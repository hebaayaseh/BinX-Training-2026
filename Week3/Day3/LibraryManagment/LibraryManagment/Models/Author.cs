using System.ComponentModel.DataAnnotations;

namespace LibraryManagment.Models
{
    public class Author
    {
        [Key]
        public int id { get; set; }
        [Required]
        [MaxLength(255)]
        public string fullName { get; set; }
        [MaxLength(100)]
        public string Country { get; set; }

        // Navigation Property
        public ICollection<BookAuthor> bookAuthors { get; set; } = new List<BookAuthor>();
    }
}
