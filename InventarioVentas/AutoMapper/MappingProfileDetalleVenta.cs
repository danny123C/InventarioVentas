using Persistencia.Inventario.Models;
using AutoMapper;

namespace InventarioVentas.AutoMapper
{
    public class MappingProfileDetalleVenta:Profile
    {
        public MappingProfileDetalleVenta()
        { 
            CreateMap<DTOs.DetalleVentasDtos.DetalleVentaUpdateDto, Persistencia.Inventario.Models.DetalleVenta>()
                .ForMember(dto => dto.IdDetalleVenta,
                m => m.MapFrom(b => b.IdDetalleVenta));
            CreateMap<DTOs.DetalleVentasDtos.DetalleVentaInsertDto, Persistencia.Inventario.Models.DetalleVenta>();
            CreateMap<DTOs.DetalleVentasDtos.DetalleVentaDto, Persistencia.Inventario.Models.DetalleVenta>();
            CreateMap<Persistencia.Inventario.Models.DetalleVenta, DTOs.DetalleVentasDtos.DetalleVentaDto>();   
            CreateMap<Persistencia.Inventario.Models.DetalleVenta, DTOs.DetalleVentasDtos.DetalleVentaUpdateDto>();
            
        }
    }
}
