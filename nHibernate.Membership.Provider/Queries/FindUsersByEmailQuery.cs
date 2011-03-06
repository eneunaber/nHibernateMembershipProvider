using System;
using System.Linq.Expressions;
using nHibernate.Membership.Provider.Entities;

namespace nHibernate.Membership.Provider.Queries
{
    public class FindUsersByEmailQuery : QueryBase<User>
    {
        private readonly string _emailAddress;
        private readonly string _applicationName;

        public FindUsersByEmailQuery(string emailAddress, string applicationName)
        {
            _emailAddress = emailAddress;
            _applicationName = applicationName;
        }

        public override Expression<Func<User, bool>> MatchingCriteria
        {
            get { return user => user.Email == _emailAddress && user.ApplicationName == _applicationName; }
        }
    }
}                                                                                           