namespace InventarioVentas.DTOs.DetalleVentasDtos
{
    public class DetalleVentaInsertDto
    {
      

        public int IdVenta { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal Subtotal { get; set; }
    }
}
