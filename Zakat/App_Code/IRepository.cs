using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Zakat.App_Code
{
    interface IRepository<T> where T : class
    {
        List<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        T Insert(T a);
        T Update(T a);
        T Delete(T a);
    }
    class Implementation<T> : IRepository<T> where T : class
    {
        private ZakatCS con;
        private DbSet<T> entities;
        public Implementation()
        {
            con = new ZakatCS();
            entities = con.Set<T>();
        }
        public T Delete(T a)
        {
            entities.Remove(a);
            con.SaveChanges();
            return a;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = con.Set<T>().Where(predicate);
            return query;
        }

       
        public List<T> GetAll()
        {
            return entities.ToList();
        }

        public T Insert(T a)
        {
            entities.Add(a);
            try
            {
                con.SaveChanges();
                return a;
            }
            
            catch (Exception)
            {
                
                throw new System.Data.Entity.Core.UpdateException();
            }
        }

        public T Update(T a)
        {
            con.Entry(a).State = EntityState.Modified;
            con.SaveChanges();
            return a;
        }
    }
}
