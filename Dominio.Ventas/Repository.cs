using Microsoft.EntityFrameworkCore;
using Persistencia.Inventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Productos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //puente entre tu aplicación y la base de datos Administra ,consulta,coordina Acceso a la base de datos
        private readonly InventarioVentasContext _context;
        //se realiza operaciones crud , son las Tablas de la base de datos
        private readonly DbSet<T> _dbSet;

        public Repository(InventarioVentasContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }


        public async Task<IEnumerable<T>> GetAll()
        => await _dbSet.ToListAsync();
        
        public async Task<T> GetById(int id)
        {
            return  await _dbSet.FindAsync(id);
        }
       
        public async Task<T?> GetByName(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);  // Elimina la entidad del DbSet
        }
        public async Task<int> GuardarTransaccionesAsincronas()
        {
            return await _context.SaveChangesAsync();
        }


        public IEnumerable<T> Search(Func<T, bool> filter) =>
             _dbSet.Where(filter).ToList();

        public void Actualizar(T entidadActualizar)
        {
            _dbSet.Attach(entidadActualizar);
            _dbSet.Entry(entidadActualizar).State = EntityState.Modified;
        }
        public async Task<bool> AnyAsyncFast(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }

    
    }

}