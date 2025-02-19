using InventarioVentas.DTOs.AuthDtos;
using InventarioVentas.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistencia.Inventario.Models;

namespace InventarioVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService<Usuario, UserDto> _authService;   
        public AuthController(IAuthService<Usuario, UserDto> authService)
        {
            _authService = authService;
        }

        public static Usuario user = new();
        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register(UserDto request)
        {
            var user = await _authService.RegisterAsync(request);
            if (user is null)
            {
                return BadRequest("User already exist");
            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResposeDto>> Login(UserDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result is null)
            {
                return BadRequest("Invalid Username or Pssword");
            }

            return Ok(result);
        }

    }
}
