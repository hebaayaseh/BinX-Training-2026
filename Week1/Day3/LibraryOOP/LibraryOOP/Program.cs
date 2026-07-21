using LibraryOOP.ClothesBorrow;
using LibraryOOP.LibrarySystem;

namespace LibraryOOP
{
    internal class Program
    {
     
        static void Main(string[] args)
        {
            Library library = new Library();
            library.Register("Heba@gmail.com", "1234");
            if(library.login("Heba@gmail.com", "1234"))
            {
                Book book1 = new Book(1, "OOP");
                Clothes borrow = new Clothes("Shirt", "S", "black");
                library.AddBook(book1);

                Console.WriteLine("Book At Library : ");
                library.ShowBooks();

                Console.WriteLine("Borrow Book : ");
                library.Borrow(book1);

                // Unrelateed Class :
                Console.WriteLine("Borrow Clothes :");
                library.Borrow(borrow);

                BorrowDetails details = new BorrowDetails
                (
                    book1.getTitle,
                    "heba@gmail.com",
                    "Heba Hesham",
                     DateTime.Now
                );
                Console.WriteLine("Borrow Details : ");
                Console.WriteLine(details);

                library.RemoveBook(1);

                Console.WriteLine("Library After Remove Book : ");
                library.ShowBooks();

            }

        }
    }
}
