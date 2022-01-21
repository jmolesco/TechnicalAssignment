using Domain;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Interface
{
    public interface IBaseRepository<T> where T : BaseEntity
    {

        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);

        T Get(object id);

        T Get(Expression<Func<T, bool>> predicate);
        void Insert(T entity, bool status = true);
        void Update(T entity);
        void Delete(T entity, bool isDelete = false);
        void Remove(T entity);
        void SaveChanges();
        IQueryable<T> Queryable();
        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);


    }
}
