using LibraryOOP.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOP.LibrarySystem
{
    public class Book : IBorrowable
    {
        private int Id;
        private string Title;
        private bool isBorrowed;

        public int getId 
        {
            get { return Id; } 
           
        }
       
        public string getTitle
        {
            get { return Title; }
            set { Title = value; }
        }
        public Book(int Id , string Title)
        {
            this.Id = Id;
            this.Title = Title;
            isBorrowed  = false;
        }

        public void Borrow()
        {
            isBorrowed = true;
            Console.WriteLine($"{Title} borrowed successfuly.");
        }
    }
}
