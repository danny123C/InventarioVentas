using FluentValidation;
using InventarioVentas.DTOs.ClientesDtos;
using Persistencia.Inventario.Models;
namespace InventarioVentas.Validators.ClientesValidators
{
    public class ClienteInsertValidators:AbstractValidator<ClienteInsertDto>
    {
        public  ClienteInsertValidators()
        {
            RuleFor(cliente => cliente.Nombre).NotEmpty().WithMessage("El nombre debe contener mas de tres letras y no debe ser nulo");
            RuleFor(cliente => cliente.Correo).NotNull().WithMessage("{PropertyName} no debe ser null ");
            //RuleFor(cliente => cliente.Telefono).NotNull().GreaterThan(9).WithMessage("{PropertyName} no debe ser null y debe ser mayor a 9 numeros");
            RuleFor(cliente => cliente.FechaCreacion).NotNull().NotEmpty().WithMessage("{PropertyName} no debe ser null  ni 0");

            RuleFor(c => c.Telefono)
             .NotEmpty().WithMessage("El número de teléfono es obligatorio.")
             .Matches(@"^\d{9}$").WithMessage("El número de teléfono debe tener exactamente 9 dígitos.")
             .Custom((telefono, context) =>
             {
                 // Agregar +593 si el número no lo tiene
                 if (!telefono.StartsWith("+593"))
                 {
                     context.InstanceToValidate.Telefono = "+593" + telefono;
                 }
             });
        }
    }
}

