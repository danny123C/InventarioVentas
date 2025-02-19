using FluentValidation;
using InventarioVentas.DTOs.VentasDtos;

namespace InventarioVentas.Validators.VentasValidators
{
    public class VentaUpdateValidators:AbstractValidator<VentaUpdateDto>
    {
        public VentaUpdateValidators() 
        {
            RuleFor(venta => venta.IdVenta).NotEmpty().WithMessage("{PropertyName} no debe ser nulo");
            RuleFor(venta => venta.IdCliente).NotNull().WithMessage("{PropertyName} no debe ser null ");
            RuleFor(venta => venta.Total).NotNull().WithMessage("{PropertyName} no debe ser null ");
            RuleFor(venta => venta.FechaVenta).NotNull().WithMessage("{PropertyName} no debe ser null  ");
           
        }  
    }
}
