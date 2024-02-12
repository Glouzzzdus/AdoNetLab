using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetLabTests
{
    public class DataSource
    {
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }

        public DataSource()
        {
            Products = GenerateProducts();
            Orders = GenerateOrders(Products);
        }

        private List<Product> GenerateProducts()
        {
            var products = new List<Product>();

            for (int i = 1; i <= 20; i++)
            {
                products.Add(new Product
                {
                    Id = i,
                    Name = $"Product {i}",
                    Description = $"Description for Product {i}",
                    Weight = i * 1.1,
                    Height = i * 2.2,
                    Width = i * 3.3,
                    Length = i * 4.4
                });
            }

            return products;
        }

        private List<Order> GenerateOrders(List<Product> products)
        {
            var orders = new List<Order>();
            Random rnd = new Random();

            for (int i = 1; i <= 20; i++)
            {
                orders.Add(new Order
                {
                    Id = i,
                    Status = (Status)rnd.Next(0, 7),
                    CreateDate = DateTime.Now.AddDays(-rnd.Next(1, 100)),
                    UpdateDate = DateTime.Now.AddDays(-rnd.Next(1, 100)),
                    Product = products[i - 1]
                });
            }

            return orders;
        }
    }
}
