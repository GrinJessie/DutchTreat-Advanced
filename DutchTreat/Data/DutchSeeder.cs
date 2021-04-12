using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using DutchTreatAdvanced.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DutchTreatAdvanced.Data
{
    public class DutchSeeder
    {
        private readonly Dutchcontext _context;
        private readonly IWebHostEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(Dutchcontext context, IWebHostEnvironment hosting, UserManager<StoreUser> userManager)
        {
            _context = context;
            _hosting = hosting;
            _userManager = userManager;
        }

        // naming is a good indication when the operation is async
        public async Task SeedAsync()
        {
            // to avoid exception
            await _context.Database.EnsureCreatedAsync();

            var user = await _userManager.FindByEmailAsync("yimiao@dutchtreat.com");

            // seed the user if doesn't exist
            if (user == null)
            {
                user = new StoreUser
                {
                    FirstName = "Yimiao",
                    LastName = "He",
                    Email = "yimiao@dutchtreat.com",
                    UserName = "yimiao@dutchtreat.com"

                };

                var result = await _userManager.CreateAsync(user, "P@ssW0rd!");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Cannot create new user in seeder");
                }
            }

            if (!_context.Products.Any())
            {
                // Create sample data for lookup tables like countries that won't change over time
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                // read the whole file as a json string
                var json = await File.ReadAllTextAsync(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                await _context.Products.AddRangeAsync(products);

                // query for the user created above instead of referring to the variable to avoid duplicate primary key issue
                var order = _context.Orders.Where(x => x.Id == 1)?.FirstOrDefault();
                if (order != null)
                {
                    order.User = _context.StoreUsers.FirstOrDefault(x => x.FirstName == "Yimiao");
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


                await _context.SaveChangesAsync();
            }

        }
    }
}
