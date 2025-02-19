﻿namespace InventarioVentas.DTOs.ProductoDtos
{
    public class ProductInsertDto
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public DateTime? FechaCreacion { get; set; }

    }
}
