﻿using System;
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
            var lastActivityDate = DateTime.Now;
            var applicationName = "myApp";

            var testObject = new UsersLastActivityQuery(lastActivityDate, applicationName);

            var exp = testObject.MatchingCriteria;
            Expression body = exp.Body;

            Assert.Equal(ExpressionType.AndAlso, body.NodeType);
            Assert.Contains("(user.LastActivityDate > value(nHibernate.Membership.Provider.Queries.UsersLastActivityQuery)._lastActivityDate)", body.ToString());
            Assert.Contains("(user.ApplicationName == value(nHibernate.Membership.Provider.Queries.UsersLastActivityQuery)._applicationName)", body.ToString());
        }
    }
}
