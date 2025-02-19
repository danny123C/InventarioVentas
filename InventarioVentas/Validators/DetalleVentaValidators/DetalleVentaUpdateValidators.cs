using InventarioVentas.DTOs.DetalleVentasDtos;
using FluentValidation;
using Persistencia.Inventario.Models;
namespace InventarioVentas.Validators.DetalleVentaValidators

{
    public class DetalleVentaUpdateValidators:AbstractValidator<DetalleVentaUpdateDto>
    {
        public DetalleVentaUpdateValidators()
        {
            
                RuleFor(detalleVenta => detalleVenta.IdDetalleVenta).NotEmpty().WithMessage("El {PropertyName} no debe ser nulo");
            RuleFor(detalleVenta => detalleVenta.IdVenta).NotEmpty().WithMessage("El {PropertyName} no debe ser nulo");
            RuleFor(detalleVenta => detalleVenta.IdProducto).NotNull().WithMessage("{PropertyName} no debe ser null ");
            RuleFor(detalleVenta => detalleVenta.Cantidad).NotNull().WithMessage("{PropertyName} no debe ser null y debe ser mayor a 3 letras");
            RuleFor(detalleVenta => detalleVenta.Subtotal).NotNull().GreaterThan(0).WithMessage("{PropertyName} no debe ser null  ni 0");

        }
    }
}
