namespace InventarioVentas.DTOs.DetalleVentasDtos
{
    public class DetalleVentaUpdateDto
    {
        public int IdDetalleVenta { get; set; }

        public int IdVenta { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal Subtotal { get; set; }
    }
}
