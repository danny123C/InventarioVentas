                                                       using AutoMapper;
using Dominio.Productos;
using InventarioVentas.DTOs.VentasDtos;
using Persistencia.Inventario.Models;

namespace InventarioVentas.Services
{
    public class VentasService : ICommonService<VentaDto, VentaInsertDto, VentaUpdateDto>
    {
        private readonly IMapper _mapper;
        private readonly EntitiesDomainVentas _entities;
        public List<string> Errors { get; }
        public VentasService(IMapper mapper, EntitiesDomainVentas entitiesDomainDetalleVentas)
        {
            _entities = entitiesDomainDetalleVentas;
            _mapper = mapper;
            Errors = new List<string>();
        }
        public async Task<IEnumerable<VentaDto>> GetAll()
        {
            var ventas = await _entities.VentasRepository.GetAll();
            return ventas.Select(v => _mapper.Map<VentaDto>(v));
        }
        public async Task<VentaDto> GetId(int id)
        {
            var venta = await _entities.VentasRepository.GetById(id);
            if (venta != null)
            {
                var responce = _mapper.Map<VentaDto>(venta);
                return responce;
            }
            return null;
        }
        public async Task<VentaDto> Add(VentaInsertDto ventaInsertDto)
        {
            var venta = _mapper.Map<Venta>(ventaInsertDto);
            await _entities.VentasRepository.Add(venta);
            _entities.GuardarTransacciones();
            var ventaDto = _mapper.Map<VentaDto>(venta);
            return ventaDto;
        }
        public async Task<VentaDto> Update(int id, VentaUpdateDto ventaUpdateDto)
        {
            var venta = await _entities.VentasRepository.GetById(id);
            if (venta != null)
            {
                _mapper.Map(ventaUpdateDto, venta);
                _entities.VentasRepository.Actualizar(venta);
                _entities.GuardarTransacciones();
                var ventaDto = _mapper.Map<VentaDto>(venta);
                return ventaDto;
            }
            return null;
        }
        public async Task<VentaDto> Delete(int id)
        {
            var venta = await _entities.VentasRepository.GetById(id);
            if (venta != null)
            {
              var ventaDto = _mapper.Map<VentaDto>(venta);   
                _entities.VentasRepository.Delete(venta);
                _entities.GuardarTransacciones();
                return ventaDto;
            }
            return null;
        }
        public bool Validate(VentaInsertDto ventaInsertDto)
        {
            return true;
        }
        public bool Validate(VentaUpdateDto ventaUpdateDto)
        {
            return true;
        }

       
    }
}
