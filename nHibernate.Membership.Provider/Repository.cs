using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using System;

namespace nHibernate.Membership.Provider
{
    public class Repository : IRepository
    {
        public Repository(ISession session)
        {
            Session = session;
        }

        private ISession Session { get; set; }

        public IQueryable<T> GetQueryableList<T>()
        {
            return (from entity in Session.Query<T>() select entity);
        }

        public List<T> GetList<T>()
        {
            return (from entity in Session.Query<T>() select entity).ToList();
        }

        public T GetById<T>(int id)
        {
            return Session.Get<T>(id);
        }

        public T GetById<T>(Object id)
        {
            return Session.Get<T>(id);
        }

        public void Save<T>(T entity)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Session.SaveOrUpdate(entity);
                transaction.Commit();
            }
        }

        public void Delete<T>(int id)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Session.Delete(GetById<T>(id));
                transaction.Commit();
            }
        }

        public void Delete<T>(Guid id)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Session.Delete(GetById<T>(id));
                transaction.Commit();
            }
        }

        public T GetOne<T>(QueryBase<T> query)
        {
            return query.SatisfyingElementFrom(Session.Query<T>());
        }

        public IQueryable<T> GetQueryableList<T>(QueryBase<T> query)
        {
            return query.SatisfyingElementsFrom(Session.Query<T>());
        }
    }
}