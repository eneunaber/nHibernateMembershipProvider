using System;
using System.Linq.Expressions;
using nHibernate.Membership.Provider.Entities;

namespace nHibernate.Membership.Provider.Queries
{
    public class FindValidatedUserByUsernameQuery : QueryBase<User>
    {
        private readonly string _username;
        private readonly string _applicationName;

        public FindValidatedUserByUsernameQuery(string username, string applicationName)
        {
            _username = username;
            _applicationName = applicationName;
        }

        public override Expression<Func<User, bool>> MatchingCriteria
        {
            get { return user => user.Username == (_username) && user.ApplicationName == _applicationName && user.IsLockedOut == false; }
        }
    }
}                                                                                           