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

        public QueryBase<User> createFindUsersWithEmailLikeQuery(string email, string appName)
        {
            return new FindUsersWithEmailLikeQuery(email, appName);
        }

        public QueryBase<User> createFindUsersWithNameLikeQuery(string name, string appName)
        {
            return new FindUsersWithNameLikeQuery(name, appName);
        }

        public QueryBase<User> createUsersLastActivityQuery(DateTime time, string appName)
        {
            return new UsersLastActivityQuery(time, appName);
        }

        public QueryBase<User> createFindUserByEmailQuery(string email, string appName)
        {
            return new FindUserByEmailQuery(email, appName);
        }

        public QueryBase<User> createFindUserByUsernameQuery(string username, string appName)
        {
            return new FindUserByUsernameQuery(username, appName);
        }
    }
}
