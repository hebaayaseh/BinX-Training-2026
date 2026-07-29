using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using MyFirstApi.Models;
using System.Linq;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("item-controller")]
    public class ItemController : ControllerBase
    {
        static List<OrderItem> items = new()
        {
            new OrderItem{Id=1,name = "Laptop",price = 2000 , quantity = 5},
            new OrderItem{Id=2,name = "Caple",price = 20 , quantity = 10},
            new OrderItem{Id=3,name = "Mouse",price = 50 , quantity = 8}
        };

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
