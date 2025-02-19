using FluentValidation;
using InventarioVentas.DTOs.ProductoDtos;

namespace InventarioVentas.Validators.ProductoValidators
{
    public class ProductoInsertValidators : AbstractValidator<ProductInsertDto>
    {
        public ProductoInsertValidators()
        {
            RuleFor(producto => producto.Nombre).NotEmpty().Length(3, 50).WithMessage("El nombre debe contener mas de tres letras y no debe ser nulo");
            RuleFor(producto => producto.FechaCreacion).NotNull().WithMessage("{PropertyName} no debe ser null ");
            RuleFor(producto => producto.Descripcion).NotNull().Length(3, 50).WithMessage("{PropertyName} no debe ser null y debe ser mayor a 3 letras");
            RuleFor(producto => producto.Precio).NotNull().WithMessage("{PropertyName} no debe ser null  ");
            RuleFor(producto => producto.Stock).NotNull().GreaterThan(0).WithMessage("{PropertyName} no debe ser null y debe ser mayor a 0");

        }
    }

}
