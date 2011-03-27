using System.Linq.Expressions;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test.Queries
{
    public class FindUsersByEmailQueryTest
    {
        [Fact]
        public void FindUsersByEmailQuery_Correctly_Builds_Expression()
        {
            var testObject = new FindUserByEmailQuery("a@b.com", "myApp");

            var exp = testObject.MatchingCriteria;
            Expression body = exp.Body;

            Assert.Equal(ExpressionType.AndAlso, body.NodeType);
            Assert.Contains("(user.Email == value(nHibernate.Membership.Provider.Queries.FindUserByEmailQuery)._emailAddress)", body.ToString());
            Assert.Contains("(user.ApplicationName == value(nHibernate.Membership.Provider.Queries.FindUserByEmailQuery)._applicationName)", body.ToString());
        }

        [Fact]
        public void QueryFactory_Builds_FindUsersByEmailQuery()
        {
            var testObject = new QueryFactory();
            var email = "myEmail@you.com";
            var appName = "myApp";

            var query = testObject.createFindUserByEmailQuery(email, appName);

            Assert.IsType<FindUserByEmailQuery>(query);
        }
    }
}
