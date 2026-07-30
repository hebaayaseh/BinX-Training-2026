using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using MyFirstApi.Interface;
using MyFirstApi.Models;
using System.Linq;

namespace MyFirstApi.Controllers
{
//    Hands-On Lab: Middleware & Dependency Injection
//1. Write a small custom middleware that logs each request's method and path to the console, and register it in Program.cs.✔
//2. Deliberately place it in the wrong pipeline order once, observe the effect, then correct the ordering.✔
//3. Create an interface and a service class implementing it, and register it with the appropriate lifetime.
//4. Inject the service into your Day 4 controller via constructor injection and use it in an endpoint.
//5. Assemble a Week 2 summary in Notion: generics/LINQ/async exercises, the scaffolded API, and a link to your GitHub
//repository — ready for the mentor check-in.

    [ApiController]
    [Route("item-controller")]
    public class ItemController : ControllerBase
    {
        private readonly IItem item;
        public ItemController(IItem item)
        {
            this.item = item;
        }

        static List<OrderItem> items = new()
        {
            new OrderItem{Id=1,name = "Laptop",price = 2000 , quantity = 5},
            new OrderItem{Id=2,name = "Caple",price = 20 , quantity = 10},
            new OrderItem{Id=3,name = "Mouse",price = 50 , quantity = 8}
        };
        [HttpGet("dependency_injection")]
        public IActionResult GetItems()
        {
            var result = item.GetItemsAsync();
            return Ok(result);
        }

        [HttpGet("list-of-item")]
        public IActionResult GetListOfItem()
        {
            if(items.Count==0)
            return BadRequest();

            return Ok(items);
        }

        [HttpGet("get-item/{Id}")]
        public IActionResult GetItemById(int Id)
        {
            var item = items.FirstOrDefault(i => i.Id == Id);
            if(item == null)
                return BadRequest();

            return Ok(item);
        }


    }
}
