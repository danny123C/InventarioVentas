using FluentValidation;
using InventarioVentas.DTOs.VentasDtos;
namespace InventarioVentas.Validators.VentasValidators
{
    public class VentaInsertValidators:AbstractValidator<VentaInsertDto>
    {
        public VentaInsertValidators()
        {
            RuleFor(venta => venta.IdCliente).NotNull().WithMessage("{PropertyName} no debe ser null ");
            RuleFor(venta => venta.Total).NotNull().WithMessage("{PropertyName} no debe ser null ");
            RuleFor(venta => venta.FechaVenta).NotNull().WithMessage("{PropertyName} no debe ser null  ");

        }

    }
}
