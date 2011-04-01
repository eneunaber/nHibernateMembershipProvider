using System;
using Moq;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_UnlockUserTest : nHibernateMembershipProviderTestBase
    {
        [Fact]
        public void UnlockUser_Sets_IsLockedOut_to_False_and_Updates_LastLockedOutDate()
        {
            var userName = "fred";
            var appName = "myApp";
            var startTime = DateTime.Now;
            var lastLockedOutDate = DateTime.Parse("10-03-10");
            var user = new User { IsLockedOut = true, LastLockedOutDate = lastLockedOutDate };
            var query = new FindUserByUsernameQuery("", "");
            _queryFactory.Setup(qf => qf.createFindUserByUsernameQuery(userName, appName)).Returns(query);
            _repository.Setup(r => r.GetOne<User>(query)).Returns(user);

            testObject.UnlockUser(userName);

            _repository.Verify(r => r.Save<User>(user));
            Assert.False(user.IsLockedOut);
            Assert.True(user.LastLockedOutDate > lastLockedOutDate && user.LastLockedOutDate >= startTime);
        }

        [Fact]
        public void UnlockUser_Returns_True_When_User_is_Unlocked()
        {
            _queryFactory.Setup(qf => qf.createFindUserByUsernameQuery(It.IsAny<String>(), It.IsAny<String>())).Returns(new FindUserByUsernameQuery("", ""));
            _repository.Setup(r => r.GetOne<User>(It.IsAny<FindUserByUsernameQuery>())).Returns(new User());

            var result = testObject.UnlockUser("fred");

            Assert.True(result);
        }

        [Fact]
        public void UnlockUser_Returns_False_When_User_is_Unlocked()
        {
            _queryFactory.Setup(qf => qf.createFindUserByUsernameQuery(It.IsAny<String>(), It.IsAny<String>())).Returns(new FindUserByUsernameQuery("", ""));
            _repository.Setup(r => r.GetOne<User>(It.IsAny<FindUserByUsernameQuery>())).Returns(new User());
            _repository.Setup(r => r.Save<User>(It.IsAny<User>())).Throws(new Exception());


            var result = testObject.UnlockUser("fred");

            Assert.False(result);
        }
    }
}