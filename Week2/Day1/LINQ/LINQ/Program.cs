using LibrarySystem;
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
            // Generic : 
            Repository<Book> bookRepository = new Repository<Book>();
            Repository<User> userRepository = new Repository<User>();

            bookRepository.Add(new Book(1, "C#"));
            bookRepository.Add(new Book(2, "ASP.NET"));

            userRepository.Add(new User("heba@gmail.com", "1234"));
            userRepository.Add(new User("hebahesham@gmail.com", "1234"));

            // ReadOnlyList :
            var books = bookRepository.ReadOnlyList();
            Console.WriteLine("Books : ");
            foreach(var book in books)
            {
                Console.WriteLine(book.getTitle);
            }
            Console.WriteLine();
            var users = userRepository.ReadOnlyList();
            Console.WriteLine("Users :");
            foreach (var user in users)
            {
                Console.WriteLine($"Email : {user.getEmail} Password :{user.getPassword}");
            }
            Console.WriteLine();
            // books.Add(new Book(3, "Java")); ==> Compiler Error because IReadOnlylist dont allow to add or remove data from list

            // Find Predicate :
            Book? foundBook = bookRepository.FindPredicate(b => b.getId == 1);
            Console.WriteLine("Is there a book with a Id = 1 ?");
            if (foundBook == null)
                Console.WriteLine("Book Not Found");
            else
                Console.WriteLine(foundBook.getTitle);
            Console.WriteLine();

            User? foundUser = userRepository.FindPredicate(u => u.getEmail == "heba@gmail.com");
            Console.WriteLine("Is there a email : heba@gmail.com ");
            if (foundUser == null)
                Console.WriteLine("Email Not Found");
            else
                Console.WriteLine($"Email : {foundUser.getEmail} Password :{foundUser.getPassword}");

            Console.WriteLine();
            // GetAll :

            var book2 = bookRepository.GetAll();
            Console.WriteLine("Books : ");

            foreach (var book in book2)
            {
                Console.WriteLine(book.getTitle+" From GetAll method");
            }
            Console.WriteLine();
            var user2 = userRepository.ReadOnlyList();
            Console.WriteLine("Users :");
            foreach (var user in user2)
            {
                Console.WriteLine($"Email : {user.getEmail} Password :{user.getPassword} From GetAll metfod");
            }

            book2.Add(new Book(3, "Java")); // ===> no have any compile error because GetAll method dont contain any restrictions that prevent this operation

            Console.WriteLine("Is there a book with a Id = 1 ?");
            List<Book>? foundBook2 = bookRepository.Find(b => b.getId == 1);
            if (foundBook == null)
                Console.WriteLine("Book Not Found");
            else
                Console.WriteLine(foundBook.getTitle+" From find method");

            Console.WriteLine();

            Console.WriteLine("Is there a email : heba@gmail.com ");
            List<User>? foundUser2 = userRepository.Find(u => u.getEmail == "heba@gmail.com");
            
            if (foundUser == null)
                Console.WriteLine("Email Not Found");
            else
                Console.WriteLine($"Email : {foundUser.getEmail} Password :{foundUser.getPassword}From find method");

        }

    }
}
