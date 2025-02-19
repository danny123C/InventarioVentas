using Microsoft.EntityFrameworkCore;
using Persistencia.Inventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Productos
{
    public class EntitiesDomainVentas
    {
        private readonly InventarioVentasContext contexto;
      
        public EntitiesDomainVentas(DbContextOptions<InventarioVentasContext> options)
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


        private Repository<Venta> ventasRepository;

        public Repository<Venta> VentasRepository
        {
            get
            {
                if (ventasRepository == null)
                {
                    ventasRepository = new Repository<Venta>(contexto);
                }
                return ventasRepository;
            }
        }
    }
}
