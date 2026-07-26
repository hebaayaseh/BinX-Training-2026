using LINQ.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ.LibrarySystem
{
    public class Library
    {
        private List<Book> books = new List<Book>();
        private List<User> users = new List<User>();

        public async Task Register(string email, string password)
        {
            foreach (var user in users)
            {
                if (user.getEmail == email)
                {
                    await Task.Delay(3000);
                    Console.WriteLine("email already exist");

                }
            }
            users.Add(new User(email, password));
            Console.WriteLine("Account Crated Successfuly");
        }

        public bool login(string email, string password)
        {
            foreach (var user in users)
            {
                if (user.getEmail == email && user.getPassword == password)
                {
                    Console.WriteLine("Login Successfuly");
                    return true;
                }
            }
            Console.WriteLine("Invalid Email Or Password!");
            return false;
        }
        public void AddBook(Book book)
        {
            books.Add(book);
            Console.WriteLine("Book Added Succssfuly");
        }

        public void RemoveBook(int Id)
        {
            books.RemoveAll(b => b.getId == Id);
            Console.WriteLine("Book Removed Succssfuly");
        }

        public void ShowBooks()
        {
            int count = 1;
            foreach (var book in books)
            {
                Console.WriteLine($"{count} : {book.getId} {book.getTitle}");
                count++;
            }
        }
        // polymorphism 
        public void Borrow(IBorrowable borrow)
        {
            borrow.Borrow();
        }

    }
}
