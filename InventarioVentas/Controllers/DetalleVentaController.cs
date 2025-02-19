using FluentValidation;
using InventarioVentas.DTOs.DetalleVentasDtos;
using InventarioVentas.DTOs.ProductoDtos;
using InventarioVentas.Services;
using InventarioVentas.Validators.DetalleVentaValidators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventarioVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleVentaController : ControllerBase
    {
        private readonly ICommonService<DetalleVentaDto, DetalleVentaInsertDto, DetalleVentaUpdateDto> _detalleVentaService;
       private readonly IValidator<DetalleVentaInsertDto> _validatorInsert;
        private readonly IValidator<DetalleVentaUpdateDto> _validatorUpdate;
        public DetalleVentaController([FromKeyedServices("DetalleVentaService")] ICommonService<DetalleVentaDto, DetalleVentaInsertDto, DetalleVentaUpdateDto> detalleVentaService
            ,IValidator<DetalleVentaInsertDto> validatorInsert, IValidator<DetalleVentaUpdateDto> validatorUpdate)
        {
            _detalleVentaService = detalleVentaService;
           _validatorInsert = validatorInsert;
           _validatorUpdate = validatorUpdate;
        }
        [HttpGet]
        public async Task<IEnumerable<DetalleVentaDto>> GetAll() =>  await _detalleVentaService.GetAll();
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var producto = await _detalleVentaService.GetId(id);
            return producto == null ? NotFound() : Ok(producto);
        }
        [HttpPost]
        public async Task<ActionResult<DetalleVentaInsertDto>> Add(DetalleVentaInsertDto detalleVentaInsertDto)
        {
            var validatorResult = _validatorInsert.Validate(detalleVentaInsertDto);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }
            if (!_detalleVentaService.Validate(detalleVentaInsertDto))
            {
                return BadRequest(_detalleVentaService.Errors);
            }
            var producto = await _detalleVentaService.Add(detalleVentaInsertDto);
            return CreatedAtAction(nameof(GetById), new { id = producto.IdDetalleVenta }, producto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DetalleVentaUpdateDto>> Update(int id, DetalleVentaUpdateDto detalleVentaUpdateDto)
        {
            var validatorResult = _validatorUpdate.Validate(detalleVentaUpdateDto);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }
            if (!_detalleVentaService.Validate(detalleVentaUpdateDto))
            {
                return BadRequest(_detalleVentaService.Errors);
            }
            var result = await _detalleVentaService.Update(id, detalleVentaUpdateDto);
            return result == null ? BadRequest() : Ok(result);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDto>> Delete(int id)
        {
            var result = await _detalleVentaService.Delete(id);
            return result == null ? BadRequest() : Ok(result);
        }

    }
}
