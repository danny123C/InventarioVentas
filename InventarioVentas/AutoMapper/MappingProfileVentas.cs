using Persistencia.Inventario.Models;
using AutoMapper;

namespace InventarioVentas.AutoMapper
{
    public class MappingProfileVentas:Profile
    {
        public MappingProfileVentas() 
        {
        //primero el origen lueo el desito
            CreateMap<Persistencia.Inventario.Models.Venta,DTOs.VentasDtos.VentaDto>();
            CreateMap<DTOs.VentasDtos.VentaInsertDto,Persistencia.Inventario.Models.Venta>();
            CreateMap<DTOs.VentasDtos.VentaUpdateDto, Persistencia.Inventario.Models.Venta>();
        }
    }
}
