﻿using System.Collections.Generic;
using DutchTreat.Data.Entities;

namespace DutchTreatAdvanced.Data
{
    // By having the interface, we can mock it in testing
    public interface IDutchRepository
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveAll();
    }
}