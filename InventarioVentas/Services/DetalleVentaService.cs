using AutoMapper;
using Dominio.Productos;
using InventarioVentas.DTOs.ProductoDtos;
using InventarioVentas.DTOs.DetalleVentasDtos;
using Persistencia.Inventario.Models;

namespace InventarioVentas.Services
{
    public class DetalleVentaService : ICommonService<DetalleVentaDto, DetalleVentaInsertDto, DetalleVentaUpdateDto>
    {
        private IRepository<Producto> _productopository;
        private IMapper _mapper;
        private EntitiesDomainDetalleVentas _entities;

        public List<string> Errors { get; }
        public DetalleVentaService(IMapper mapper, EntitiesDomainDetalleVentas entitiDomainDetalleVentas)
        {
            _mapper = mapper;
            Errors = new List<string>();
            _entities = entitiDomainDetalleVentas;
        }
        public async Task<IEnumerable<DetalleVentaDto>> GetAll()
        {
            var detalleVenta = await _entities.DetalleVentaRepository.GetAll(); 
            return detalleVenta.Select(dv => new DetalleVentaDto
            {
                IdDetalleVenta = dv.IdDetalleVenta,
                IdVenta = dv.IdVenta,
                IdProducto = dv.IdProducto,
                Cantidad = dv.Cantidad,
                Subtotal = dv.Subtotal,
            });
        }


        public async Task<DetalleVentaDto> GetId(int id)
        {
            var detalleVenta = await _entities.DetalleVentaRepository.GetById(id);
            if (detalleVenta != null)
            {
                var responce = _mapper.Map<DetalleVentaDto>(detalleVenta);
                return responce;
            }
            return null;
        }

        public async Task<DetalleVentaDto> Add(DetalleVentaInsertDto detalleVentaInsertDto)
        {
            var detalleventa = _mapper.Map<DetalleVenta>(detalleVentaInsertDto); //necesito un beer a partir de (beerInsertDto)
            await _entities.DetalleVentaRepository.Add(detalleventa);
            _entities.GuardarTransacciones();
            //necestio un beerDto a partir de (beer)
            var detalleventaDto = _mapper.Map<DetalleVentaDto>(detalleventa);

            return detalleventaDto;
        }
        public async Task<DetalleVentaDto> Update(int id, DetalleVentaUpdateDto detalleVentaUpdateDto)
        {
            var detalleventa = await _entities.DetalleVentaRepository.GetById(id);
            if (detalleventa != null)
            {
                //apartir de beerUpdateDto necesito un beer
                detalleventa = _mapper.Map< DetalleVenta>(detalleVentaUpdateDto);
                _entities.DetalleVentaRepository.Actualizar(detalleventa);
                _entities.GuardarTransacciones();
                var detalleVentaDto = _mapper.Map<DetalleVentaDto>(detalleventa);
                return detalleVentaDto;

            }
            return null;
        }

        public async Task<DetalleVentaDto> Delete(int id)
        {
            var detalleVenta = await _entities.DetalleVentaRepository.GetById(id);
            if (detalleVenta != null)
            {
                var detalleVentaDto = _mapper.Map<DetalleVentaDto>(detalleVenta);
                    
                _entities.DetalleVentaRepository.Delete(detalleVenta);
                _entities.GuardarTransacciones();

                return detalleVentaDto;

            }
            return null;

        }


        public bool Validate(DetalleVentaInsertDto dto)
        {
            if (_entities.DetalleVentaRepository.Search(b => b.IdVenta == dto.IdVenta).Count() > 0)
            {
                Errors.Add("Ya existe una venta con el mismo id");
                return false;
            }
            return true;
        }

        public bool Validate(DetalleVentaUpdateDto dto)
        {
            if (_entities.DetalleVentaRepository.Search(b => b.IdDetalleVenta == dto.IdDetalleVenta
                && dto.IdVenta != b.IdVenta).Count() > 0)

            {
                Errors.Add("Ya existe una vwnta con ese nombre");
                return false;
            }
            return true;
        }
    }
}

