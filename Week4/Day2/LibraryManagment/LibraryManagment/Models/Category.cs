using System.ComponentModel.DataAnnotations;

namespace LibraryManagment.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        [MaxLength(255)]
        public string name { get; set; }
        [MaxLength(255)]
        public string decription { get; set; }

        // Navigation Properties : 

        public ICollection<Book> books { get; set; } = new List<Book>();
    }
}
