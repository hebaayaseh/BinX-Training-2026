using LibraryManagment.Enum;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagment.Models
{
    public class Member
    {
        [Key]
        public int id { get; set; }
        [MaxLength(255)]
        public string name { get; set; }
        [MaxLength(255)]
        public string email { get; set; }
        [MaxLength(20)]
        public string phone { get; set; }
        public DateTime registerDate { get; set; }
        public UserRole Role {  get; set; }
        // Navigation Properties : 
        public ICollection<Borrowing> borrowings { get; set; } = new List<Borrowing>();
    }
}
