using System.Collections.Generic;
using System.Linq;

namespace nHibernate.Membership.Provider
{
    public interface IRepository
    {
        IQueryable<T> GetQueryableList<T>();
        List<T> GetList<T>();
        T GetById<T>(int id);
        void Save<T>(T entity);
        T GetOne<T>(QueryBase<T> query);
        IQueryable<T> GetQueryableList<T>(QueryBase<T> query);
        void Delete<T>(int id);
    }
}