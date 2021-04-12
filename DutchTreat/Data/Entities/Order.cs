using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreatAdvanced.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace DutchTreat.Data.Entities
{
  public class Order
  {
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderNumber { get; set; }

    // relate one entity to another entity - parent child relationship / one to many relationship
    public ICollection<OrderItem> Items { get; set; }
    public StoreUser User { get; set; }
  }
}
