using System;
using System.Linq.Expressions;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test.Queries
{
    public class FindAllUsersQueryTest
    {
        [Fact]
        public void FindAllUsersQuery_Correctly_Builds_Expression()
        {
            var testObject = new FindAllUsersQuery("myApp");

            var exp = testObject.MatchingCriteria;
            Expression body = exp.Body;

            Assert.Equal(ExpressionType.Equal, body.NodeType);
            Assert.Contains("(user.ApplicationName == value(nHibernate.Membership.Provider.Queries.FindAllUsersQuery)._applicationName)", body.ToString());
        }

        [Fact]
        public void QueryFactory_Builds_FindAllUsersQuery() {
            var testObject = new QueryFactory();
            var appName = "myApp";

            var query = testObject.createFindAllUsersQuery(appName);

            Assert.IsType<FindAllUsersQuery>(query);
        }
    }
}
