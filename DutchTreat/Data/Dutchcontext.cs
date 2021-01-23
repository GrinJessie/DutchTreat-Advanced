using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DutchTreatAdvanced.Data
{
    // Need to have DbContext and Entities defined before creating the database using "dotnet ef database"
    // DbContext is not tied to a single database provider, same context can tie to SQL server, mySql, etc 
    public class Dutchcontext : DbContext
    {
        // Take the options we specified in the start up and pass them into the context so it know what connection string to actually use
        public Dutchcontext(DbContextOptions<Dutchcontext> options) : base(options)
        {
            
        }

        // Allows to query and add products/orders to the database through the context
        // since orders have relationship to order items, unless to query order items across orders,  we don't need it. Only worry about the order items as a child of orders.
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}
