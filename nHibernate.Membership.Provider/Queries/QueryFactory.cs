using System;
using nHibernate.Membership.Provider.Entities;

namespace nHibernate.Membership.Provider.Queries
{
    public class QueryFactory : IQueryFactory
    {
        public QueryBase<User> createFindAllUsersQuery(string appName)
        {
            return new FindAllUsersQuery(appName);
        }

        public QueryBase<User> createFindUsersByEmailQuery(string email, string appName)
        {
            return new FindUsersByEmailQuery(email, appName);
        }

        public QueryBase<User> createFindUsersByNameQuery(string name, string appName)
        {
            return new FindUsersByNameQuery(name, appName);
        }

        public QueryBase<User> createUsersLastActivityQuery(DateTime time, string appName)
        {
            return new UsersLastActivityQuery(time, appName);
        }
    }
}
