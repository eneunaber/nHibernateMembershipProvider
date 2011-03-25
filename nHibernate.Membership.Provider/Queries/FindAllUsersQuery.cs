using System;
using System.Linq.Expressions;
using nHibernate.Membership.Provider.Entities;

namespace nHibernate.Membership.Provider.Queries
{
    public class FindAllUsersQuery : QueryBase<User>
    {
        private readonly string _applicationName;

        public FindAllUsersQuery(string applicationName)
        {
            _applicationName = applicationName;
        }
        public override Expression<Func<User, bool>> MatchingCriteria
        {
            get { return user => user.ApplicationName == _applicationName; }
        }
    }
}
