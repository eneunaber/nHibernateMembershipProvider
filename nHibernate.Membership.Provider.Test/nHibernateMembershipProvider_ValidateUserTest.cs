using System;
using System.Web.Security;
using Moq;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_ValidateUser : nHibernateMembershipProviderTestBase
    {
        [Fact]
        public void ValidateUser_Creates_a_FindValidatedUserByUsernameQuery_and_Passes_it_to_Repository()
        {
            var username = "foo";
            var password = "bar";
            var appName = "myApp";
            var findValidatedUserByUsernameQuery = new FindValidatedUserByUsernameQuery(username, appName);
            _queryFactory.Setup(qf => qf.createFindValidatedUserByUsernameQuery(username, appName)).Returns(findValidatedUserByUsernameQuery);

            testObject.ValidateUser(username, password);

            _repository.Verify(r => r.GetOne(findValidatedUserByUsernameQuery));
        }

        [Fact]
        public void GetUser_Returns_False_if_No_User_is_Returned_From_Repository()
        {
            var username = "foo";
            var password = "bar";
            var appName = "myApp";
            _queryFactory.Setup(qf => qf.createFindValidatedUserByUsernameQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(new FindValidatedUserByUsernameQuery("", ""));
            _repository.Setup(r => r.GetOne(It.IsAny<FindValidatedUserByUsernameQuery>())).Returns<User>(null);

            var result = testObject.ValidateUser(username, password);

            Assert.False(result);
        }


    }
}