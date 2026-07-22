using LINQ.LibrarySystem;
using System.ComponentModel.DataAnnotations;

namespace LINQ
{
    //    Hands-On Lab: LINQ Queries & an Async Method
    //1. Create a List of at least 8 objects from your Day 3 domain model with varied property values. ✔
    //2. Write 3 LINQ queries against the list: one filter, one projection, and one aggregation (Count, Sum, or Average).✔
    //3. Write an async method that simulates an I/O delay (Task.Delay) and returns a result, then await it from Main.✔
    //4. Wrap a risky operation (e.g.parsing user input) in a try/catch that catches a specific exception type and handles it
    //meaningfully.
    //5. Commit the day's work to your GitHub repository
    internal class Program
    {
        static async Task Main(string[] args)
        {
            List<Book> books = new List<Book>
            {
                new Book(1,"C#"),
                new Book(2,"Java"),
                new Book(3,"C++"),
                new Book(4,"C"),
                new Book(5,"Python"),
                new Book(6,"PHP"),
                new Book(7,"ASP.NET"),
                new Book(8,"JavaScript")
            };

            // Print Lisy :
            var printBook = books
                .Select(b => b.getTitle)
                .ToList();
            Console.WriteLine("Books : ");
            foreach(var book in printBook)
            {
                Console.WriteLine(book);
            }
            Console.WriteLine();

            // Filter :
            Console.WriteLine("Enter a character to filter the book : ");
            var ch = Console.ReadLine();
            var filter = books.Where(b => b.getTitle.ToLower().Contains(ch))
            .ToList();
            if (filter.Count() == 0)
                Console.WriteLine("Book Not Found");
            else
            {
                Console.WriteLine($"Books Contain the Character : ");
                foreach (var book in filter)
                {
                    Console.WriteLine(book.getTitle);
                }

                Console.WriteLine();

                // Projection : 
                var bookTitle = books.Select(t => t.getTitle)
                    .ToList();
                int count = 0;
                foreach (var book in filter)
                {
                    Console.WriteLine($"{count++} : {book.getTitle}");
                }

                Console.WriteLine();
            }

            // Aggregation 
            var countBooks = books.Count();
            Console.WriteLine($"Number of books : {countBooks}");

            Console.WriteLine();

            // Async Method :
            Library library = new Library();
            await library.Register("Heba@gmail.com", "1234");

            // Try & Catch :

            Console.WriteLine();

            try
            {
                Console.WriteLine($"Enter Book Id : ");
                int Id = int.Parse(Console.ReadLine());
                var findBook = books.Where(b => b.getId == Id)
                    .First();

                Console.WriteLine($"Book You Need : {findBook.getTitle}");
            }
            catch
            {
                Console.WriteLine("Book Not Found");
            }

        }

    }
}
