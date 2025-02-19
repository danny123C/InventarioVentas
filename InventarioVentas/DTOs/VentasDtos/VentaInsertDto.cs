namespace InventarioVentas.DTOs.VentasDtos
{
    public class VentaInsertDto
    {
        

        public int IdCliente { get; set; }

        public decimal Total { get; set; }

        public DateTime? FechaVenta { get; set; }
    }
}
