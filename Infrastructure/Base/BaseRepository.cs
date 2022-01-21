using Domain;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly DataContext context;
        private DbSet<T> entities;
        //private IDbContextTransaction transaction;
        //string errorMessage = string.Empty;

        public BaseRepository(DataContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return entities.AsNoTracking();
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return entities.AsNoTracking().Where(predicate);
        }

        public virtual T Get(object id)
        {
            return entities.Find(id);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return entities.AsNoTracking().Where(predicate).FirstOrDefault();
        }

        public virtual void Insert(T entity, bool status = true)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.Status = status;
            entity.DateCreated = DateTime.Now;
            //entity.DateModified = default(DateTime).DefaultSqlDateTime();
            entities.Add(entity);
            context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            entity.DateModified = DateTime.Now;
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public virtual void Delete(T entity, bool isDelete = false)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (isDelete == true)
            {
                context.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                entity.Status = false;
                entity.DateModified = DateTime.Now;
            }
            context.SaveChanges();
        }

        public virtual void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
        }

        public virtual void SaveChanges()
        {
            context.SaveChanges();
        }

        public virtual IQueryable<T> Queryable() => entities;


        public virtual IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            return context.Database.BeginTransaction();
        }

        public virtual bool Commit(IDbContextTransaction transaction = null)
        {
            if (transaction == null)
            {
                return false;
            }
            transaction.Commit();
            return true;
        }

        public virtual void Rollback(IDbContextTransaction transaction = null)
        {
            transaction.Rollback();
        }
    }
}
