using FluentValidation;
using InventarioVentas.DTOs.DetalleVentasDtos;
using Persistencia.Inventario.Models;

namespace InventarioVentas.Validators.DetalleVentaValidators
{
    public class DetalleVentaInsertValidators:AbstractValidator<DetalleVentaInsertDto>
    {
        public DetalleVentaInsertValidators()
        {
            RuleFor(detalleVenta => detalleVenta.IdVenta).NotEmpty().WithMessage("El nombre debe contener mas de tres letras y no debe ser nulo");
            RuleFor(detalleVenta => detalleVenta.IdProducto).NotNull().WithMessage("{PropertyName} no debe ser null ");
            RuleFor(detalleVenta => detalleVenta.Cantidad).NotNull().WithMessage("{PropertyName} no debe ser null y debe ser mayor a 3 letras");
            RuleFor(detalleVenta => detalleVenta.Subtotal).NotNull().GreaterThan(0).WithMessage("{PropertyName} no debe ser null  ni 0");
         
        }
    }
}

