using System;
using nHibernate.Membership.Provider.Entities;
using System.Linq.Expressions;

namespace nHibernate.Membership.Provider.Queries
{
    public class FindUsersByNameQuery : QueryBase<User>
    {
        private string _userName;
        private string _applicationName;

        public FindUsersByNameQuery(string userName, string applicationName)
        {
            _userName = userName;
            _applicationName = applicationName;
        }

        public override Expression<Func<User, bool>> MatchingCriteria
        {
            get { return user => user.Username == _userName && user.ApplicationName == _applicationName;  }
        }
    }
}
