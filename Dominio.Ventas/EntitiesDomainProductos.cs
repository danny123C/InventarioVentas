using Microsoft.EntityFrameworkCore;
using Persistencia.Inventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Productos
{
    public class EntitiesDomainProductos
    {

        InventarioVentasContext contexto;

        public EntitiesDomainProductos(DbContextOptions<InventarioVentasContext> options)
        {
            contexto = new InventarioVentasContext(options);
        }

        public void GuardarTransacciones()
        {
            contexto.SaveChanges();
        }

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        private Repository<Producto> productosRepository;

        public Repository<Producto> ProductosRepository
        {
            get
            {
                if (productosRepository == null)
                {
                    productosRepository = new Repository<Producto>(contexto);
                }
                return productosRepository;
            }
        }

    }
}
