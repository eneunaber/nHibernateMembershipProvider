using System.Linq.Expressions;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test.Queries
{
    public class FindUsersByUsernameQueryTest
    {
        [Fact]
        public void FindUsersByUsernameQuery_Correctly_Builds_Expression()
        {
            var testObject = new FindUserByUsernameQuery("foo", "myApp");

            var exp = testObject.MatchingCriteria;
            Expression body = exp.Body;

            Assert.Equal(ExpressionType.AndAlso, body.NodeType);
            Assert.Contains("(user.Username == value(nHibernate.Membership.Provider.Queries.FindUserByUsernameQuery)._username)", body.ToString());
            Assert.Contains("(user.ApplicationName == value(nHibernate.Membership.Provider.Queries.FindUserByUsernameQuery)._applicationName)", body.ToString());
        }

        [Fact]
        public void QueryFactory_Builds_FindUsersByUsernameQuery()
        {
            var testObject = new QueryFactory();
            var username = "foo";
            var appName = "myApp";

            var query = testObject.createFindUserByUsernameQuery(username, appName);

            Assert.IsType<FindUserByUsernameQuery>(query);
        }
    }
}
