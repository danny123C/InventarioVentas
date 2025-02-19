namespace InventarioVentas.DTOs.VentasDtos
{
    public class VentaDto
    {
        public int IdVenta { get; set; }

        public int IdCliente { get; set; }

        public decimal Total { get; set; }

        public DateTime? FechaVenta { get; set; }
    }
}
