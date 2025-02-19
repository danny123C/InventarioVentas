using Persistencia.Inventario.Models;
using AutoMapper;

namespace InventarioVentas.AutoMapper
{
   
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DTOs.ProductoDtos.ProductUpdateDto, Persistencia.Inventario.Models.Producto>()
                .ForMember(dto => dto.IdProducto,
              m => m.MapFrom(b => b.Id));
           CreateMap<DTOs.ProductoDtos.ProductInsertDto,Persistencia.Inventario.Models.Producto>(); //primero eligo el origen y luego el destino si tienen los mismos campos
           CreateMap<DTOs.ProductoDtos.ProductDto, Persistencia.Inventario.Models.Producto>();
            CreateMap<Persistencia.Inventario.Models.Producto, DTOs.ProductoDtos.ProductDto>();

        }
    }
}
