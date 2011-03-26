using System;
using nHibernate.Membership.Provider.Entities;

namespace nHibernate.Membership.Provider.Queries
{
    public interface IQueryFactory
    {
        QueryBase<User> createFindAllUsersQuery(string appName);
        QueryBase<User> createFindUsersByEmailQuery(string email, string appName);
        QueryBase<User> createFindUsersByNameQuery(string name, string appName);
        QueryBase<User> createUsersLastActivityQuery(DateTime time, string appName);
    }
}
