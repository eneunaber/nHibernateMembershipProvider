using nHibernate.Membership.Provider.Queries;
using System.Linq.Expressions;
using Xunit;

namespace nHibernate.Membership.Provider.Test.Queries
{
    public class FindUsersByEmailQueryTest
    {

        [Fact]
        public void FindUsersByEmailQuery_Correctly_Builds_Expression()
        {
            var emailAddress = "a@b.com";
            var applicationName = "myApp";

            var testObject = new FindUsersByEmailQuery(emailAddress, applicationName);

            var exp = testObject.MatchingCriteria;
            Expression body = exp.Body;

            Assert.Equal(ExpressionType.AndAlso, body.NodeType);
            Assert.Contains("(user.Email == value(nHibernate.Membership.Provider.Queries.FindUsersByEmailQuery)._emailAddress)", body.ToString());
            Assert.Contains("(user.ApplicationName == value(nHibernate.Membership.Provider.Queries.FindUsersByEmailQuery)._applicationName)", body.ToString());
        }
    }
}
