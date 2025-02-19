using InventarioVentas.DTOs.AuthDtos;
namespace InventarioVentas.Services.AuthService
{
    public interface IAuthService<TUser,TUserDto> 
        where TUser : class
        where TUserDto : class
    {
        // Registra un nuevo usuario en el sistema y devuelve el usuario creado o null si el registro falla.
        Task<TUser?> RegisterAsync(TUserDto request);

        // Permite que un usuario inicie sesión. Recibe las credenciales del usuario (UserDto) 
        // y devuelve un objeto TokenResponseDto con el Access Token y el Refresh Token si la autenticación es exitosa.
        Task<TokenResposeDto?> LoginAsync(TUserDto request);

        // Permite renovar el Access Token utilizando un Refresh Token válido. 
        // Recibe un RefreshTokenRequestDto (que contiene el refresh token actual) y devuelve un nuevo TokenResponseDto 
        // con un nuevo Access Token si el Refresh Token es válido.
        Task<TokenResposeDto?> RefreshTokenAsync(RefreshTokenRequestDto request);

        Task<bool> RevokeRefreshTokenAsync(int IdUser);

    }
}
