using LibraryOOP.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOP.ClothesBorrow
{
    // Unrelated Class 
    public class Clothes : IBorrowable
    {
        private string name;
        private string size;
        private string color;
        public string getName => name;
        public string getSize => size;
        public string getColor => color;
        public Clothes(string name, string size, string color)
        {
            this.name = name;
            this.size = size;
            this.color = color;
        }

        public void Borrow()
        {
            Console.WriteLine($"{name} with size: {size} eith color: {color} borrowed");
        }
    }
}
