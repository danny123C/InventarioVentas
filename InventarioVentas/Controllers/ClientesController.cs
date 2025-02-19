using FluentValidation;
using InventarioVentas.DTOs.ClientesDtos;
using InventarioVentas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventarioVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ICommonService<ClienteDto,ClienteInsertDto,ClienteUpdateDto> _clienteService;
        private readonly IValidator<ClienteInsertDto> _validatorInsert;
        private readonly IValidator<ClienteUpdateDto> _validatorUpdate;
        public ClientesController([FromKeyedServices("ClienteService")] ICommonService<ClienteDto, ClienteInsertDto, ClienteUpdateDto> clienteService
            , IValidator<ClienteInsertDto> validatorInsert, IValidator<ClienteUpdateDto> validatorUpdate)
        {
            _clienteService = clienteService;
            _validatorInsert = validatorInsert;
            _validatorUpdate = validatorUpdate;
        }
        [HttpGet]
        public async Task<IEnumerable<ClienteDto>> GetAll() => await _clienteService.GetAll();

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetById(int id)
        {
            var producto = await _clienteService.GetId(id);
            return producto == null ? NotFound() : Ok(producto);
        }
        [HttpPost]
        public async Task<ActionResult<ClienteInsertDto>> Add(ClienteInsertDto clienteInsertDto)
        {
            var validatorResult = _validatorInsert.Validate(clienteInsertDto);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }
            if (!_clienteService.Validate(clienteInsertDto))
            {
                return BadRequest(_clienteService.Errors);
            }
            var producto = await _clienteService.Add(clienteInsertDto);
            return CreatedAtAction(nameof(GetById), new { id = producto.IdCliente }, producto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteUpdateDto>> Update(int id, ClienteUpdateDto clienteUpdateDto)
        {
            var validatorResult = _validatorUpdate.Validate(clienteUpdateDto);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }
            if (!_clienteService.Validate(clienteUpdateDto))
            {
                return BadRequest(_clienteService.Errors);
            }
            var result = await _clienteService.Update(id, clienteUpdateDto);
            return result == null ? BadRequest() : Ok(result);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClienteDto>> Delete(int id)
        {
            var result = await _clienteService.Delete(id);
            return result == null ? BadRequest() : Ok(result);
        }

    }
}
