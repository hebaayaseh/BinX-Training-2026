using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOP.LibrarySystem
{
    // record 
    public record BorrowDetails
    {
        string Title;
        string email;
        string name;
        DateTime BorrowDate;
        public BorrowDetails(string Title , string email , string name , DateTime BorrowDate)
        {
            this.Title = Title;
            this.email = email;
            this.name = name;
            this.BorrowDate = BorrowDate;
        }
    }
}
