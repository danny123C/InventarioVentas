using Microsoft.EntityFrameworkCore;
using InventarioVentas.AutoMapper;
using Persistencia.Inventario.Models;
using Dominio.Productos;
using InventarioVentas.Services;
using InventarioVentas.DTOs.ProductoDtos;
using InventarioVentas.Validators.ProductoValidators;
using FluentValidation;
using InventarioVentas.DTOs.DetalleVentasDtos;
using InventarioVentas.Validators.DetalleVentaValidators;
using InventarioVentas.DTOs.ClientesDtos;
using InventarioVentas.Validators.ClientesValidators;
using InventarioVentas.DTOs.VentasDtos;
using InventarioVentas.Validators.VentasValidators;
using InventarioVentas.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using InventarioVentas.DTOs.AuthDtos; // Add this line
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddKeyedScoped<ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto>, ProductosService>("ProductosService"); //servicio de reglas de negocio
builder.Services.AddKeyedScoped<ICommonService<DetalleVentaDto, DetalleVentaInsertDto, DetalleVentaUpdateDto>, DetalleVentaService>("DetalleVentaService"); //servicio de reglas de negocio
builder.Services.AddKeyedScoped<ICommonService<ClienteDto, ClienteInsertDto, ClienteUpdateDto>, ClienteService>("ClienteService"); //servicio de reglas de negocio
builder.Services.AddKeyedScoped<ICommonService<VentaDto, VentaInsertDto, VentaUpdateDto>, VentasService>("VentasService"); //servicio de reglas de negocio
// Registrar el DbContext
builder.Services.AddDbContext<InventarioVentasContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreConnection"));
});

///Authorizationnn


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"])),
            ValidateIssuerSigningKey = true,
        };
    });
///Auth servicios
builder.Services.AddScoped<IAuthService<Usuario,UserDto>, AuthService>();
//servicios de repositorio bilbioteca d eclases 
builder.Services.AddScoped<EntitiesDomainProductos>();
builder.Services.AddScoped<EntitiesDomainDetalleVentas>();
builder.Services.AddScoped<EntitiesDomainClientes>();
builder.Services.AddScoped<EntitiesDomainVentas>();
builder.Services.AddScoped<EntitiesDomainUsuarios>();
// Registrar el repositorio genérico
//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); servicios del repositorio generico
//////////Inyeccion de validators 
builder.Services.AddScoped<IValidator<ProductInsertDto>, ProductoInsertValidators>();
builder.Services.AddScoped<IValidator<ProductUpdateDto>, ProductoUpdateValidators>();

builder.Services.AddScoped<IValidator<DetalleVentaInsertDto>, DetalleVentaInsertValidators>();
builder.Services.AddScoped<IValidator<DetalleVentaUpdateDto>, DetalleVentaUpdateValidators>();

builder.Services.AddScoped<IValidator<ClienteInsertDto>, ClienteInsertValidators>();
builder.Services.AddScoped<IValidator<ClienteUpdateDto>, ClienteUpdateValidators>();

builder.Services.AddScoped<IValidator<VentaInsertDto>, VentaInsertValidators>();
builder.Services.AddScoped<IValidator<VentaUpdateDto>, VentaUpdateValidators>();
//Mappers
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
// Add services to the container.
builder.Services.AddControllers();

// Configuración para Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
