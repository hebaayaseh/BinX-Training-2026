using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    // T must be a reference type (class) dont accept int or string ... 
    // This repositry works at any class even different class 
    public class Repository<T> where T : class
    {
        private readonly List<T> item = new List<T>();
        // Add :
        public void Add(T item)
        {
            this.item.Add(item);
        }

        // GetAll :
        public List<T> GetAll()
        {
            return item;
        }

        // IReadOnlyList :
        public IReadOnlyList<T> ReadOnlyList()
        {
            return item.AsReadOnly();
        }

        // Find :
        public List<T> Find(Func<T,bool> itemes)
        {
            return item.Where(itemes).ToList();
        }

        //  Find method taking a predicate :
        public T? FindPredicate(Predicate<T> predicate)
        {
            return item.Find(predicate);
        }


    }
}
