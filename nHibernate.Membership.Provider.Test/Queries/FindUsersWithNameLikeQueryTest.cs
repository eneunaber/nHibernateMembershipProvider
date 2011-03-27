using System.Linq.Expressions;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test.Queries
{
    public class FindUsersWithNameLikeQueryTest
    {

        [Fact]
        public void FindUsersByEmailQuery_Correctly_Builds_Expression()
        {
            var testObject = new FindUsersWithNameLikeQuery("Fred", "myApp");

            var exp = testObject.MatchingCriteria;
            Expression body = exp.Body;

            Assert.Equal(ExpressionType.AndAlso, body.NodeType);
            Assert.Contains("(user.Username.StartsWith(value(nHibernate.Membership.Provider.Queries.FindUsersWithNameLikeQuery)._userName) ", body.ToString());
            Assert.Contains("(user.ApplicationName == value(nHibernate.Membership.Provider.Queries.FindUsersWithNameLikeQuery)._applicationName)", body.ToString());
        }

        [Fact]
        public void QueryFactory_Builds_FindUsersByNameQuery()
        {
            var testObject = new QueryFactory();
            var name = "my name";
            var appName = "myApp";

            var query = testObject.createFindUsersWithNameLikeQuery(name, appName);

            Assert.IsType<FindUsersWithNameLikeQuery>(query);
        }
    }
}
