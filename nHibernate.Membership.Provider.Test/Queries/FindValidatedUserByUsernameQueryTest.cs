using System.Linq.Expressions;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test.Queries
{
    public class FindValidatedUserByUsernameQueryTest
    {
        [Fact]
        public void FindValidatedUserByEmailQuery_Correctly_Builds_Expression()
        {
            var testObject = new FindValidatedUserByUsernameQuery("a@b.com", "myApp");

            var exp = testObject.MatchingCriteria;
            Expression body = exp.Body;

            Assert.Equal(ExpressionType.AndAlso, body.NodeType);
            Assert.Contains("(user.Username == value(nHibernate.Membership.Provider.Queries.FindValidatedUserByUsernameQuery)._username)", body.ToString());
            Assert.Contains("(user.ApplicationName == value(nHibernate.Membership.Provider.Queries.FindValidatedUserByUsernameQuery)._applicationName)", body.ToString());
            Assert.Contains("(user.IsLockedOut == False)", body.ToString());
        }

        [Fact]
        public void QueryFactory_Builds_FindValidatedUserByEmailQuery()
        {
            var testObject = new QueryFactory();
            var email = "myEmail@you.com";
            var appName = "myApp";

            var query = testObject.createFindValidatedUserByUsernameQuery(email, appName);

            Assert.IsType<FindValidatedUserByUsernameQuery>(query);
        }
    }
}
