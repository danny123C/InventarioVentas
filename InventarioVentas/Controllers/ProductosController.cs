using FluentValidation;
using InventarioVentas.DTOs.ProductoDtos;
using InventarioVentas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InventarioVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {

        private readonly ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto> _prodctoService;
        private readonly IValidator<ProductInsertDto> _validatorInsert;
        private readonly IValidator<ProductUpdateDto> _validatorUpdate;
        public ProductosController([FromKeyedServices("ProductosService")]ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto> prodctoService,
           IValidator<ProductInsertDto> validatorInsert, IValidator<ProductUpdateDto> validatorUpdate)
        {
            _prodctoService = prodctoService;
            _validatorInsert = validatorInsert;
            _validatorUpdate = validatorUpdate;
        }
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAll() => await  _prodctoService.GetAll();

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var producto = await _prodctoService.GetId(id);
            return producto == null ? NotFound() : Ok(producto);
        }
        [HttpPost]
        public async Task<ActionResult<ProductInsertDto>> Add(ProductInsertDto productInsertDto)
        {
            var validatorResult = _validatorInsert.Validate(productInsertDto);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }   
            if (!_prodctoService.Validate(productInsertDto))
            {
                return BadRequest(_prodctoService.Errors);
            }   
            var producto = await _prodctoService.Add(productInsertDto);
            return CreatedAtAction(nameof(GetById), new { id = producto.IdProducto }, producto);
        }
      
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductUpdateDto>> Update(int id, ProductUpdateDto productUpdateDto)
        {
            var validatorResult = _validatorUpdate.Validate(productUpdateDto);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }
            if (!_prodctoService.Validate(productUpdateDto))
            {
                return BadRequest(_prodctoService.Errors);
            }
            var result = await _prodctoService.Update(id, productUpdateDto);
            return result == null ? BadRequest() : Ok(result);

        }
        [HttpDelete("{id}")]
       public async Task<ActionResult<ProductDto>> Delete(int id)
        {
            var result = await _prodctoService.Delete(id);
            return result == null ? BadRequest() : Ok(result);
        }
    }
}
