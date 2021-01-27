using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;

namespace DutchTreatAdvanced.Data
{
    // Another layer between DbContext and its operations
    // Expose the different calls to the database that we want
    // We don't want to use the context directly, we want to make the call through the repository
    public class DutchRepository : IDutchRepository
    {
        private readonly Dutchcontext _context;

        public DutchRepository(Dutchcontext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products
                .OrderBy(x => x.Title)
                .ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _context.Products
                .Where(x => x.Category == category)
                .ToList();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
