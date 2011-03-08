using System;
using nHibernate.Membership.Provider.Entities;
using System.Linq.Expressions;

namespace nHibernate.Membership.Provider.Queries
{
    public class UsersLastActivityQuery : QueryBase<User>
    {
        private DateTime _lastActivityDate;
        private string _applicationName;

        public UsersLastActivityQuery(DateTime lastActivityDate, string applicationName)
        {
            _lastActivityDate = lastActivityDate;
            _applicationName = applicationName;
        }

        public override Expression<Func<User, bool>> MatchingCriteria
        {
            get { return user => user.LastActivityDate > _lastActivityDate && user.ApplicationName == _applicationName;  }
        }
    }
}
