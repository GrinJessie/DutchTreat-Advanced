using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;

namespace DutchTreatAdvanced.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }

        // Used convention of AutoMapping
        // Referring to the sub data type, prefix the attribute names with th sub data type name
        // Not returning the data stored in the DB actually, do collapsing to simplified the data structure on the server versus what we expose to the API
        [Required]
        public string ProductId { get; set; }

        public string ProductCategory { get; set; }
        public string ProductSize { get; set; }
        public string ProductTitle { get; set; }
        public string ProductArtId { get; set; }
        public string ProductArtist { get; set; }
    }
}
