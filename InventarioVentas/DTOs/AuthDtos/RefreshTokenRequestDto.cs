namespace InventarioVentas.DTOs.AuthDtos
{
    public class RefreshTokenRequestDto
    {
        public int IdUser { get; set; }
        public required string RefreshToken { get; set; }
    }
}
