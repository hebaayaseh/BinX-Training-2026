using Ecommerce.System;
using System.Linq;
using System.Linq.Expressions;

namespace Ecommerce
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Customer> customers = new ()
            {
                new Customer (1,"Heba Hesham","Heba@gmail.com","1234"),
                new Customer (2,"Hesham Ayaseh","Hesham@gmail.com","5678"),
            };

            List<OrderItem> items = new()
            {
                new OrderItem
                {
                    Id = 1,
                    name = "Laptop",
                    price = 1500,
                    quantity = 5

                },
                new OrderItem
                {
                    Id = 2,
                    name = "Mouse",
                    price = 50,
                    quantity = 10

                },
                new OrderItem
                {
                    Id = 3,
                    name = "Keybord",
                    price = 150,
                    quantity = 4

                },
                new OrderItem
                {
                    Id = 4,
                    name = "Cable",
                    price = 70,
                    quantity = 8

                },
                new OrderItem
                {
                    Id = 5,
                    name = "USP",
                    price = 140,
                    quantity = 4

                },
                new OrderItem
                {
                    Id = 6,
                    name = "SSD",
                    price = 200,
                    quantity = 4

                }
            };

            List<Order> orders = new()
            {
                new Order
                {
                    Id = 1,
                    customerId = 1,
                    name = "Electronic Order",
                    TotalAmount = (5*1500)+(4*200),
                    items = new List<OrderItem>
                    {
                        new OrderItem
                    {
                          Id = 1,
                          name = "Laptop",
                          price = 1500,
                          quantity = 5

                    },
                        new OrderItem
                        {
                          Id = 6,
                          name = "SSD",
                          price = 200,
                          quantity = 4

                        },
                    },
                    
                },
                new Order
                {
                    Id = 2,
                    customerId = 2,
                    name = "Electronic Order",
                    TotalAmount = 4*140,
                    items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                           Id = 5,
                           name = "USP",
                           price = 140,
                           quantity = 4

                        },
                    }
                },
                new Order
                {
                    Id = 3,
                    customerId = 1,
                    name = "Electronic Order",
                    TotalAmount = 150*4,
                    items = new List<OrderItem>
                    {
                       new OrderItem
                       {
                         Id = 3,
                         name = "Keybord",
                         price = 150,
                         quantity = 4

                       },
                    }
                },
                new Order
                {
                    Id = 4,
                    customerId = 2,
                    name = "Electronic Order",
                    TotalAmount = 70*8,
                    items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                           Id = 4,
                           name = "Cable",
                           price = 70,
                           quantity = 8

                        },
                    }
                },
                new Order
                {
                    Id = 5,
                    customerId = 2,
                    name = "Electronic Order",
                    TotalAmount = 140*4,
                    items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                           Id = 5,
                           name = "USP",
                           price = 140,
                           quantity = 4

                        },
                    }
                },
                new Order
                {
                    Id = 6,
                    customerId = 1,
                    name = "Electronic Order",
                    TotalAmount = 200*4,
                    items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                           Id = 6,
                           name = "SSD",
                           price = 200,
                           quantity = 4

                        }
                    }
                },
            };

            

            var totalOrders = orders
                .GroupBy(o => o.customerId)
                .Select(o => new { 
                    customerId = o.Key,
                    TotalAmount = o.Sum(x=>x.TotalAmount)
                
                });

            foreach(var order in totalOrders)
            {
                Console.WriteLine($"CustomerId : {order.customerId} Total : {order.TotalAmount}");
            }
            Console.WriteLine();

            var customersResult = customers.Join(
                orders,
                c => c.Id,
                o => o.customerId,
                (c, o) => new
                {
                    cutomerName = c.fullName,
                    orderId = o.Id,
                    Total = o.TotalAmount
                });

            foreach(var customer in customersResult)
            {
                Console.WriteLine($"{customer.cutomerName} - {customer.orderId} - {customer.Total}");
            }

            Console.WriteLine();

            var ordersResult = orders
                .SelectMany(o => o.items);

            foreach(var item in ordersResult)
            {
                Console.WriteLine($"{item.name} - {item.quantity}");
            }

            Console.WriteLine();

            var query = customers.Where(c => c.fullName.StartsWith('A'));

            customers.Add(new Customer(
                 3,
                "Ahmad",
                "Ahmad@gmail.com",
               "12356"
            ));

            foreach(var customer in query)
            {
                Console.WriteLine(customer.fullName);
            }

            Console.WriteLine();

            // to list :
            var query2 = customers.Where(c => c.fullName.StartsWith('A'))
                                  .ToList();

            customers.Add(new Customer(
                4,
                "Adam",
                "Adam@gmail.com",
                "12356"
            ));

            foreach (var customer in query2)
            {
                Console.WriteLine(customer.fullName);
            }

            //Deference betwwen quere1 and query2 :
            // LINQ : dont run the query immediately ,it runs only when we use it like (foreach) , so if we change the data before executing the query , the query will use the updated data
            //  ToList : run the query directly and store the result before use it , so if we change the data before executing the query , the query will doesnt use the updated data

        }
    }
}
