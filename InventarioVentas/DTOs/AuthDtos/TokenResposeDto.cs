namespace InventarioVentas.DTOs.AuthDtos
{
    public class TokenResposeDto
    {
        public required string AccesToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
