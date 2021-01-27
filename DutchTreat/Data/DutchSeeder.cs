using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace DutchTreatAdvanced.Data
{
    public class DutchSeeder
    {
        private readonly Dutchcontext _context;
        private readonly IWebHostEnvironment _hosting;

        public DutchSeeder(Dutchcontext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        public void Seed()
        {
            // to avoid exception
            _context.Database.EnsureCreated();

            if (!_context.Products.Any())
            {
                // Create sample data for lookup tables like countries that won't change over time
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                // read the whole file as a json string
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _context.Products.AddRange(products);

                var order = _context.Orders.Where(x => x.Id == 1)?.FirstOrDefault();
                if (order != null)
                {
                    order.Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }

                _context.SaveChanges();
            }

        }
    }
}
