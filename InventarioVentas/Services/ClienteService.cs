
using AutoMapper;
using Dominio.Productos;
using InventarioVentas.DTOs.ClientesDtos;
using InventarioVentas.DTOs.DetalleVentasDtos;
using Persistencia.Inventario.Models;

namespace InventarioVentas.Services
{
    public class ClienteService : ICommonService<ClienteDto, ClienteInsertDto, ClienteUpdateDto>
    {
        //private IRepository<Cliente> _clienteRepository;
        private IMapper _mapper;
        private EntitiesDomainClientes _entities;

        public List<string> Errors { get; }
        public ClienteService(IMapper mapper, EntitiesDomainClientes entitiesDomainClientes)
        {
            _mapper = mapper;
            Errors = new List<string>();
            _entities = entitiesDomainClientes;
        }
        public async Task<IEnumerable<ClienteDto>> GetAll()
        {
            var cliente = await _entities.ClienteRepository.GetAll();
            return cliente.Select(dv => _mapper.Map<ClienteDto>(dv));
        }
        public async Task<ClienteDto> GetId(int id)
        {
            var cliente = await _entities.ClienteRepository.GetById(id);
            if (cliente != null)
            {
                var responce = _mapper.Map<ClienteDto>(cliente);
                return responce;
            }
            return null;
        }

        public async Task<ClienteDto> Add(ClienteInsertDto clienteInsertDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteInsertDto); //necesito un beer a partir de (beerInsertDto)
            await _entities.ClienteRepository.Add(cliente);
            _entities.GuardarTransacciones();
            //necestio un beerDto a partir de (beer)
            var clienteDto = _mapper.Map<ClienteDto>(cliente);

            return clienteDto;
        }
        public async Task<ClienteDto> Update(int id, ClienteUpdateDto clienteUpdateDto)
        {
            var cliente = await _entities.ClienteRepository.GetById(id);
            if (cliente != null)
            {
                // Mapear el DTO al cliente actual se pasa dto a cliente
                _mapper.Map(clienteUpdateDto, cliente);  // Esto actualizará las propiedades del cliente
                //cliente = _mapper.Map<Cliente>(clienteUpdateDto);
                _entities.ClienteRepository.Actualizar(cliente);
                _entities.GuardarTransacciones();  // Guarda los cambios
                var clienteDto = _mapper.Map<ClienteDto>(cliente);  // Mapea la entidad actualizada al DTO
                return clienteDto;
            }
            return null;

          
        }

        public async Task<ClienteDto> Delete(int id)
        {
            var cliente = await _entities.ClienteRepository.GetById(id);
            if (cliente != null)
            {
                var clienteDto = _mapper.Map<ClienteDto>(cliente);
                _entities.ClienteRepository.Delete(cliente);
                _entities.GuardarTransacciones();

                return clienteDto;

            }
            return null;

        }

        public bool Validate(ClienteInsertDto dto)
        {
           // if (_entities.ClienteRepository.Search(b => b. == dto.IdCliente).Count() > 0)
            //{
              //  Errors.Add("Ya existe un Cliente con el mismo id");
                ////return false;
            //}
            return true;
        }

        public bool Validate(ClienteUpdateDto dto)
        {

            // Cambiar esta validación, suponiendo que el email es único
            var existe = _entities.ClienteRepository.Search(b =>
                b.IdCliente != dto.IdCliente && b.Correo == dto.Correo).Any();

            if (existe)
            {
                Errors.Add("Ya existe un Cliente con ese email");
                return false;
            }

            return true;
        }
    }
}
