using FluentValidation;
using InventarioVentas.DTOs.ClientesDtos;
using Persistencia.Inventario.Models;
namespace InventarioVentas.Validators.ClientesValidators
{
    public class ClienteUpdateValidators:AbstractValidator<ClienteUpdateDto>
    {
        public  ClienteUpdateValidators()
        {
            RuleFor(cliente => cliente.IdCliente).NotEmpty().WithMessage("El {PropertyName} no debe ser nulo");
            RuleFor(cliente => cliente.Nombre).NotEmpty().WithMessage("El nombre debe contener mas de tres letras y no debe ser nulo");
            RuleFor(cliente => cliente.Correo).NotNull().WithMessage("{PropertyName} no debe ser null ");
          //  RuleFor(cliente => cliente.Telefono).NotNull().GreaterThan(9).WithMessage("{PropertyName} no debe ser null y debe ser mayor a 9 numeros");
            RuleFor(cliente => cliente.FechaCreacion).NotNull().NotEmpty().WithMessage("{PropertyName} no debe ser null  ni 0");
        }
    }
}
