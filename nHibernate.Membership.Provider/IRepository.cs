using System.Collections.Generic;
using System.Linq;
using System;

namespace nHibernate.Membership.Provider
{
    public interface IRepository
    {
        IQueryable<T> GetQueryableList<T>();
        List<T> GetList<T>();
        T GetById<T>(int id);
        T GetById<T>(Guid id);
        void Save<T>(T entity);
        T GetOne<T>(QueryBase<T> query);
        IQueryable<T> GetQueryableList<T>(QueryBase<T> query);
        void Delete<T>(int id);
        void Delete<T>(Guid guid);
    }
}