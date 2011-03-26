using System;
using System.Linq.Expressions;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test.Queries
{
    public class UsersLastActivityQueryTest
    {
        [Fact]
        public void UsersLastActivityQuery_Correctly_Builds_Expression()
        {
            var testObject = new UsersLastActivityQuery( DateTime.Now, "myApp");

            var exp = testObject.MatchingCriteria;
            Expression body = exp.Body;

            Assert.Equal(ExpressionType.AndAlso, body.NodeType);
            Assert.Contains("(user.LastActivityDate > value(nHibernate.Membership.Provider.Queries.UsersLastActivityQuery)._lastActivityDate)", body.ToString());
            Assert.Contains("(user.ApplicationName == value(nHibernate.Membership.Provider.Queries.UsersLastActivityQuery)._applicationName)", body.ToString());
        }

        [Fact]
        public void QueryFactory_Builds_UsersLastActivityQuery()
        {
            var testObject = new QueryFactory();
            var time = DateTime.Now;
            var appName = "myApp";

            var query = testObject.createUsersLastActivityQuery(time, appName);

            Assert.IsType<UsersLastActivityQuery>(query);
        }
    }
}
