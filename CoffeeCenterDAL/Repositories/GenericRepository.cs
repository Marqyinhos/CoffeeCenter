using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeCenterDAL.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        public DbContext context;
        DbSet<T> dbSet { get; set; }
        public GenericRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return dbSet;
        }
        public T Get(int id)
        {
            return dbSet.Find(id);
        }
        public void AddOrUpdate(T entity)
        {
            dbSet.AddOrUpdate(entity);
            SaveChanges();
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            SaveChanges();
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
