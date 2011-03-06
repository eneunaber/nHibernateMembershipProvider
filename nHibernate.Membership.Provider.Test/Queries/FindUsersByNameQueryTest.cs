using nHibernate.Membership.Provider.Queries;
using System.Linq.Expressions;
using Xunit;
using System;

namespace nHibernate.Membership.Provider.Test.Queries
{
    public class FindUsersByNameQueryTest
    {

        [Fact]
        public void FindUsersByEmailQuery_Correctly_Builds_Expression()
        {
            var userName = "Fred";
            var applicationName = "myApp";

            var testObject = new FindUsersByNameQuery(userName, applicationName);

            var exp = testObject.MatchingCriteria;
            Expression body = exp.Body;

            Assert.Equal(ExpressionType.AndAlso, body.NodeType);
            Assert.Contains("(user.Username == value(nHibernate.Membership.Provider.Queries.FindUsersByNameQuery)._userName)", body.ToString());
            Assert.Contains("(user.ApplicationName == value(nHibernate.Membership.Provider.Queries.FindUsersByNameQuery)._applicationName)", body.ToString());
        }
    }
}
