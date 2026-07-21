using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOP.LibrarySystem
{
    public class User
    {
        private string email;
        private string password;

        
        public string getEmail 
        { 
           get { return email; }
           set { email = value; }
        }
        public string getPassword
        {
            get { return password; }
            set { password = value; }
        }

        public User (string email, string password)
        {
            if (email == null || password == null)
                throw new ArgumentException("Email and Password is required");
            this.email = email;
            this.password = password;
        }
    }
}
