using System.Linq.Expressions;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test.Queries
{
    public class FindUsersWithEmailLikeQueryTest
    {
        [Fact]
        public void FindUsersByEmailQuery_Correctly_Builds_Expression()
        {
            var testObject = new FindUsersWithEmailLikeQuery("a@b.com", "myApp");

            var exp = testObject.MatchingCriteria;
            Expression body = exp.Body;

            Assert.Equal(ExpressionType.AndAlso, body.NodeType);
            Assert.Contains("(user.Email.StartsWith(value(nHibernate.Membership.Provider.Queries.FindUsersWithEmailLikeQuery)._emailAddress)", body.ToString());
            Assert.Contains("(user.ApplicationName == value(nHibernate.Membership.Provider.Queries.FindUsersWithEmailLikeQuery)._applicationName)", body.ToString());
        }

        [Fact]
        public void QueryFactory_Builds_FindUsersByEmailQuery()
        {
            var testObject = new QueryFactory();
            var email = "myEmail@you.com";
            var appName = "myApp";

            var query = testObject.createFindUsersWithEmailLikeQuery(email, appName);

            Assert.IsType<FindUsersWithEmailLikeQuery>(query);
        }
    }
}
