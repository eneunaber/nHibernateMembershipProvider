using System.Collections.Generic;
using System.Linq;
using Moq;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;
using Xunit;
using System.Web.Security;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_GetUserNameByEmail : nHibernateMembershipProviderTestBase
    {
        [Fact]
        public void GetUserNameByEmail_Creates_a_FindUserByEmailQuery_and_Passes_it_to_Repository()
        {
            var email = "a@b.com";
            var appName = "myApp";
            var findUsersByEmailQuery = new FindUserByEmailQuery(email, appName);
            _queryFactory.Setup(qf => qf.createFindUserByEmailQuery(email, appName)).Returns(findUsersByEmailQuery);

            testObject.GetUserNameByEmail(email);

            _repository.Verify(r => r.GetOne(findUsersByEmailQuery));
        }

        [Fact]
        public void GetUserNameByEmail_Returns_Username_From_Repository()
        {
            var email = "a@b.com";
            var username = "fred";
            var user = new User { Username = username };
            _queryFactory.Setup(qf => qf.createFindUserByEmailQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(new FindUserByEmailQuery("",""));
            _repository.Setup(r => r.GetOne(It.IsAny<FindUserByEmailQuery>())).Returns(user);

            var result = testObject.GetUserNameByEmail(email);

            Assert.Equal(result, username);
        }

        [Fact]
        public void GetUserNameByEmail_Returns_EmptyString_When_User_is_Not_Found()
        {
            var email = "a@b.com";
            var username = "";
            _queryFactory.Setup(qf => qf.createFindUserByEmailQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(new FindUserByEmailQuery("", ""));
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUserByEmailQuery>())).Returns((new List<User>()).AsQueryable<User>());

            var result = testObject.GetUserNameByEmail(email);

            Assert.Equal(result, username);
        }

    }
}