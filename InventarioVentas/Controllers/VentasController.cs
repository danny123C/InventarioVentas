using FluentValidation;
using InventarioVentas.DTOs.VentasDtos;
using InventarioVentas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventarioVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly ICommonService<VentaDto, VentaInsertDto, VentaUpdateDto> _ventaService;
        private readonly IValidator<VentaInsertDto> _validatorInsert;
        private readonly IValidator<VentaUpdateDto> _validatorUpdate;
        public VentasController([FromKeyedServices("VentasService")] ICommonService<VentaDto, VentaInsertDto, VentaUpdateDto> ventaService
            , IValidator<VentaInsertDto> validatorInsert, IValidator<VentaUpdateDto> validatorUpdate)
        {
            _ventaService = ventaService;
            _validatorInsert = validatorInsert;
            _validatorUpdate = validatorUpdate;
        }
        [HttpGet]
        public async Task<IEnumerable<VentaDto>> GetAll() => await _ventaService.GetAll();

        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDto>> GetById(int id)
        {
            var producto = await _ventaService.GetId(id);
            return producto == null ? NotFound() : Ok(producto);
        }
        [HttpPost]
        public async Task<ActionResult<VentaInsertDto>> Add(VentaInsertDto ventaInsertDto)
        {
            var validatorResult = _validatorInsert.Validate(ventaInsertDto);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }
            if (!_ventaService.Validate(ventaInsertDto))
            {
                return BadRequest(_ventaService.Errors);
            }
            var producto = await _ventaService.Add(ventaInsertDto);
            return CreatedAtAction(nameof(GetById), new { id = producto.IdCliente }, producto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VentaUpdateDto>> Update(int id, VentaUpdateDto ventaUpdateDto)
        {
            var validatorResult = _validatorUpdate.Validate(ventaUpdateDto);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }
            if (!_ventaService.Validate(ventaUpdateDto))
            {
                return BadRequest(_ventaService.Errors);
            }
            var result = await _ventaService.Update(id, ventaUpdateDto);
            return result == null ? BadRequest() : Ok(result);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<VentaDto>> Delete(int id)
        {
            var result = await _ventaService.Delete(id);
            return result == null ? BadRequest() : Ok(result);
        }

    }
}
