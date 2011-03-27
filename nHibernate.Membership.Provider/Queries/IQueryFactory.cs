using System;
using nHibernate.Membership.Provider.Entities;

namespace nHibernate.Membership.Provider.Queries
{
    public interface IQueryFactory
    {
        QueryBase<User> createFindAllUsersQuery(string appName);
        QueryBase<User> createFindUsersWithEmailLikeQuery(string email, string appName);
        QueryBase<User> createFindUsersWithNameLikeQuery(string name, string appName);
        QueryBase<User> createUsersLastActivityQuery(DateTime time, string appName);
        QueryBase<User> createFindUserByEmailQuery(string email, string appName);
    }
}
