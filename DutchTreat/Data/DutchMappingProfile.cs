using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreatAdvanced.ViewModels;

namespace DutchTreatAdvanced.Data
{
    public class DutchMappingProfile : Profile
    {
        public DutchMappingProfile()
        {
            // convention based that it matches property by property, type by type
            CreateMap<Order, OrderViewModel>()
                // when property names don't match
                // provide the source destination relationship
                .ForMember( o => o.OrderId, ex => ex.MapFrom(o => o.Id))
                // support mapping above in the opposite order
                .ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>()
                .ReverseMap();
        }
    }
}
