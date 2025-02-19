using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Productos
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        //Task<T> GetByName(string name);
        Task<T?> GetByName(Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        void Delete(T entity);
        IEnumerable<T> Search(Func<T, bool> filter);
   
        void Actualizar(T entidadActualizar);
    }
}
