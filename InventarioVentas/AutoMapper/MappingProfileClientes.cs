using Persistencia.Inventario.Models;
using AutoMapper;
using System.Runtime.CompilerServices;
namespace InventarioVentas.AutoMapper
{
    public class MappingProfileClientes:Profile
    {
        public MappingProfileClientes() 
        {
            CreateMap<Persistencia.Inventario.Models.Cliente,DTOs.ClientesDtos.ClienteDto>();//primero eligo el origen y luego el destino
            CreateMap<DTOs.ClientesDtos.ClienteInsertDto,Persistencia.Inventario.Models.Cliente>();
            CreateMap<DTOs.ClientesDtos.ClienteUpdateDto, Persistencia.Inventario.Models.Cliente>();
        }
    }   
}
