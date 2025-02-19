
using Dominio.Productos;
using InventarioVentas.DTOs.AuthDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistencia.Inventario.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InventarioVentas.Services.AuthService
{
    public class AuthService : IAuthService<Usuario, UserDto>
    {
        private readonly EntitiesDomainUsuarios _entitiesDomainUsuarios;
        private InventarioVentasContext _context;
        private IConfiguration _configuration;
        public AuthService(EntitiesDomainUsuarios entitiesDomainUsuarios, IConfiguration configuration) 
        {
            _configuration = configuration;
            _entitiesDomainUsuarios = entitiesDomainUsuarios;
        }
        public async Task<TokenResposeDto?> LoginAsync(UserDto request)
        {
            // Buscar en la base de datos un usuario con el mismo Username del request

            var user = await _entitiesDomainUsuarios.UsuariosRepository.GetByName(u => u.UserName == request.UserName);




            // Si el usuario no existe en la base de datos, retornamos null (inicio de sesión fallido)
            if (user is null)
            {
                return null;
            }
            // Verificar si la contraseña proporcionada coincide con la contraseña almacenada (hasheada)
            // Se usa la clase PasswordHasher<User> para comparar la contraseña ingresada con la guardada en la BD
            var result = new PasswordHasher<Usuario>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password);
            //  Si la verificación de la contraseña falla, retornamos null (credenciales incorrectas)
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }
            // Crear una nueva instancia del objeto TokenResponseDto para almacenar los tokens
            var response = new TokenResposeDto()
            {
                // Generar el Access Token llamando al método CreateToken y asignarlo a AccesToken
                AccesToken = CreateToken(user),
                // Generar un Refresh Token de manera asíncrona y guardarlo en la base de datos
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
            // Retornar el objeto TokenResponseDto que contiene el Access Token y el Refresh Token
            return response;

        }

        public async Task<Usuario?> RegisterAsync(UserDto request)
        {
            // Verificar si el usuario ya existe en la base de datos
            // if (await context.Users.AnyAsync(u => u.Username == request.Username))
            if (await  _entitiesDomainUsuarios.UsuariosRepository.AnyAsyncFast(u => u.UserName == request.UserName))
            {
                return null; // Si el usuario ya existe, retornamos null (registro fallido)
            }
            // Crear una nueva instancia de User
            var user = new Usuario();
            // Hashear la contraseña antes de guardarla en la base de datos
            var HashedPassword = new PasswordHasher<Usuario>().HashPassword(user, request.Password);
            // Asignar los valores del usuario a la entidad User
            user.UserName = request.UserName;
            user.PasswordHash = HashedPassword; // Guardamos la contraseña hasheada por seguridad
            user.Correo = request.Correo;
            user.Rol = request.Rol;
            // Agregar el nuevo usuario al contexto de la base de datos
            _entitiesDomainUsuarios.UsuariosRepository.Add(user);

            //Guardar los cambios en la base de datos de manera asíncrona
            await _entitiesDomainUsuarios.UsuariosRepository.GuardarTransaccionesAsincronas();
            //Retornar el usuario registrado
            return user;
        }


        // Método público y asíncrono que maneja la renovación del token
        public async Task<TokenResposeDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            // Validar si el Refresh Token y el UserId son correctos
            var user = await ValidateRefreshTokenAsync(request.IdUser , request.RefreshToken);
            // Si el usuario no es válido o el Refresh Token es incorrecto, retorna null
            if (user is null)
            {
                return null;
            }
            // Si la validación es exitosa, genera y devuelve una nueva respuesta con tokens
            return await CreateTokenRespoce(user);
        }

        // Método privado que genera la respuesta con un nuevo Access Token y Refresh Token
        private async Task<TokenResposeDto> CreateTokenRespoce(Usuario? user)
        {
            return new TokenResposeDto()
            {
                // Genera un nuevo Access Token para el usuario autenticado
                AccesToken = CreateToken(user),

                //Genera y guarda un nuevo Refresh Token en la base de datos
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        // 1️⃣ Método privado y asíncrono que valida si un Refresh Token es válido
        private async Task<Usuario?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            // 2️⃣ Buscar en la base de datos el usuario correspondiente al userId
            var user = await _entitiesDomainUsuarios.UsuariosRepository.GetById(userId);// context.Users.FindAsync(userId);

            // 3️⃣ Si el usuario no existe, el Refresh Token no coincide o ha expirado, retorna null
            if (user is null || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            // 4️⃣ Si el Refresh Token es válido, retorna el usuario para generar nuevos tokens
            return user;
        }


        //refresf token
        private string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        //Método privado y asíncrono que genera y guarda un Refresh Token para un usuario
        private async Task<string> GenerateAndSaveRefreshTokenAsync(Usuario user)
        {
            //Generar un nuevo Refresh Token
            var refreshToken = CreateRefreshToken();
            user.RefreshToken = refreshToken; //Asignar el token generado al usuario actual
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);//Definir la fecha de expiración del Refresh Token (en este caso, 7 días)
            await _entitiesDomainUsuarios.UsuariosRepository.GuardarTransaccionesAsincronas();  //Guardar los cambios en la base de datos (persistir el Refresh Token del usuario)
            return refreshToken; //Retornar el Refresh Token generado
        }

        private string CreateToken(Usuario user)
        {
            // Crear una lista de claims (información del usuario incluida en el token)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                 new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                 new Claim(ClaimTypes.Role, user.Rol)
            };
            // Obtener la clave secreta desde la configuración (AppSettings)
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));
            // Crear las credenciales de firma con el algoritmo HMAC-SHA256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<bool> RevokeRefreshTokenAsync(int userId)
        {
            // 1️⃣ Buscar al usuario en la base de datos por su ID
            var user = await _entitiesDomainUsuarios.UsuariosRepository.GetById(userId);
            if (user is null)
            {
                return false; // Usuario no encontrado
            }

            // 2️⃣ Invalidar el Refresh Token (puede ser eliminándolo o sobrescribiéndolo con un valor nulo/vacío)
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow; // O ponerlo en el pasado para asegurar expiración inmediata

            // 3️⃣ Guardar los cambios en la base de datos
            await _entitiesDomainUsuarios.UsuariosRepository.GuardarTransaccionesAsincronas();

            return true; // Éxito
        }
    }
}
