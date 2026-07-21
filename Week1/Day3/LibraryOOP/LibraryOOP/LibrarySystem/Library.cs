using LibraryOOP.InterFace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryOOP.LibrarySystem
{
    //    Hands-On Lab: Model a Small Domain
    //1. Design a small domain(e.g.a library, a task tracker, or an order system) with at least 2 related classes. ✔
    //2. Add at least one record type for an immutable data transfer object in your domain. ✔
    //3. Apply proper encapsulation: private backing fields, public properties, and a constructor that enforces valid initial state.✔
    //4. Define one interface implemented by two unrelated classes in your domain, demonstrating polymorphism with a
    //method that accepts the interface type.✔
    //5. Commit the domain model to your GitHub repository. ✔
    public class Library
    {
        private List<Book> books = new List<Book>();
        private List<User> users = new List<User>();

        public void Register(string email , string password)
        {
            foreach (var user in users)
            {
                if (user.getEmail == email)
                {
                    Console.WriteLine("email already exist");
                   
                }
            }
            users.Add(new User(email, password));
            Console.WriteLine("Account Crated Successfuly");
        }

        public bool login(string email, string password)
        {
            foreach(var user in users)
            {
                if(user.getEmail==email && user.getPassword==password)
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
            foreach(var book in books)
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
