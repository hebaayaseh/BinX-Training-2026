using MyFirstApi.Interface;
using MyFirstApi.Models;

namespace MyFirstApi.Service
{
    public class GetItems : IItem
    {
         List<OrderItem> items = new()
        {
            new OrderItem{Id=1,name = "Laptop",price = 2000 , quantity = 5},
            new OrderItem{Id=2,name = "Caple",price = 20 , quantity = 10},
            new OrderItem{Id=3,name = "Mouse",price = 50 , quantity = 8}
        };

        public async Task<string> GetItemsAsync()
        {
            if (items.Count() == 0)
                throw new Exception("item from dependency injection method empty");

            return "item from dependency injection method not empty";
        }
    }
}
