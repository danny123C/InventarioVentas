namespace InventarioVentas.DTOs.ClientesDtos
{
    public class ClienteUpdateDto
    {

        public int IdCliente { get; set; }
        public string Nombre { get; set; }

        public string Correo { get; set; }

        public string Telefono { get; set; }

        public DateTime? FechaCreacion { get; set; }

    }
}
