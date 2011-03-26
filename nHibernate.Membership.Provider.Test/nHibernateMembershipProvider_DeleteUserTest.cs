using System;
using Moq;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_DeleteUser : nHibernateMembershipProviderTestBase
    {
        [Fact]
        public void DeleteUser_Creates_a_FindUsersByName_and_Passes_UserId_to_Delete_in_Repository()
        {
            var name = "foo";
            var appName = "myApp";
            var findUsersByNameQuery = new FindUsersByNameQuery(name, appName);
            _queryFactory.Setup(qf => qf.createFindUsersByNameQuery(name, appName)).Returns(findUsersByNameQuery);
            User user = new User { Id =  Guid.NewGuid() };
            _repository.Setup(r => r.GetOne(findUsersByNameQuery)).Returns(user);

            testObject.DeleteUser(name, false);

            _repository.Verify(r => r.Delete<User>(user.Id));
        }

        [Fact] 
        public void DeleteUser_Does_Not_Call_Delete_in_Repository_if_No_User_is_Found()
        {
            User user = null;
            _repository.Setup(r => r.GetOne(It.IsAny<FindUsersByNameQuery>())).Returns(user);

            testObject.DeleteUser("foo", false);

            _repository.Verify(r => r.Delete<User>(It.IsAny<Guid>()), Times.Never());
        }

        [Fact]
        public void DeleteUser_Returns_False_if_No_User_is_Found()
        {
            User user = null;
            _repository.Setup(r => r.GetOne(It.IsAny<FindUsersByNameQuery>())).Returns(user);

            var returnValue = testObject.DeleteUser("foo", false);

            Assert.False(returnValue);
        }

        [Fact]
        public void DeleteUser_Returns_True_if_User_is_Deleted()
        {
            User user = new User { Id = Guid.NewGuid() };
            _repository.Setup(r => r.GetOne(It.IsAny<FindUsersByNameQuery>())).Returns(user);

            var returnValue = testObject.DeleteUser("foo", false);

            Assert.True(returnValue);
        }
    }
}