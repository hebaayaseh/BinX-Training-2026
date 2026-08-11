using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagment.Models
{
    public class Borrowing
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Book")]
        public int bookId {get; set; }
        [ForeignKey("Member")]
        public int memberId { get; set; }
        public DateTime borrowDate { get; set; }
        public DateTime? returnDate { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal amount { get; set; }

        //Navigation Properties :
        public Book? Book { get; set; }
        public Member? Member { get; set; }
    }
}
