using System.Collections.Generic;
using System.Linq;
using Moq;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;
using Xunit;
using System.Web.Security;
using System;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_GetUserTest : nHibernateMembershipProviderTestBase
    {
        [Fact]
        public void GetUser_Creates_a_FindUserByUsernameQuery_and_Passes_it_to_Repository()
        {
            var username = "foo";
            var appName = "myApp";
            var findUserByUsernameQuery = new FindUserByUsernameQuery(username, appName);
            _queryFactory.Setup(qf => qf.createFindUserByUsernameQuery(username, appName)).Returns(findUserByUsernameQuery);

            testObject.GetUser(username, false);

            _repository.Verify(r => r.GetQueryableList(findUserByUsernameQuery));
        }

        [Fact]
        public void GetUser_Returns_User_From_Repository()
        {
            var username = "fred";
            IQueryable<User> userCollection = (new List<User>() { new User { Username = username } }).AsQueryable<User>();
            _queryFactory.Setup(qf => qf.createFindUserByUsernameQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(new FindUserByUsernameQuery("", ""));
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUserByUsernameQuery>())).Returns(userCollection);

            var result = testObject.GetUser(username, false);

            Assert.IsType<MembershipUser>(result);
            Assert.Equal(result.UserName, username);
        }

        [Fact]
        public void GetUser_Returns_Null_if_No_User_is_Returned_From_Repository()
        {
            var username = "fred";
            _queryFactory.Setup(qf => qf.createFindUserByUsernameQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(new FindUserByUsernameQuery("", ""));
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUserByUsernameQuery>())).Returns((new List<User>()).AsQueryable<User>());

            var result = testObject.GetUser(username, false);

            Assert.Null(result);
        }

        [Fact]
        public void GetUser_Updates_User_LastActivityDate_When_userIsOnline_is_True()
        {
            var startTime = DateTime.Now;
            var username = "fred";
            var user = new User { Username = username };
            IQueryable<User> userCollection = (new List<User>() { user }).AsQueryable<User>();
            _queryFactory.Setup(qf => qf.createFindUserByUsernameQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(new FindUserByUsernameQuery("", ""));
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUserByUsernameQuery>())).Returns(userCollection);

            var result = testObject.GetUser(username, true);

            var stopTime = DateTime.Now;
            Assert.True(result.LastActivityDate >= startTime && result.LastActivityDate <= stopTime);
            _repository.Verify(r => r.Save<User>(user));
        }

        [Fact]
        public void GetUser_Does_NOT_Update_User_LastActivityDate_When_userIsOnline_is_False()
        {
            var startTime = DateTime.Now;
            var username = "fred";
            var user = new User { Username = username, LastActivityDate = startTime };
            IQueryable<User> userCollection = (new List<User>() { user }).AsQueryable<User>();
            _queryFactory.Setup(qf => qf.createFindUserByUsernameQuery(It.IsAny<string>(), It.IsAny<string>())).Returns(new FindUserByUsernameQuery("", ""));
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUserByUsernameQuery>())).Returns(userCollection);

            var result = testObject.GetUser(username, false);

            var stopTime = DateTime.Now;
            Assert.Equal(result.LastActivityDate, startTime);
            _repository.Verify(r => r.Save<User>(user), Times.Never());
        }

        //GetUser when provided a User Id

        [Fact]
        public void GetUser_Passes_UserId_to_Repository_and_Passes_Matching_User_Back()
        {
            var id = Guid.NewGuid();
            var userName = "fred";
            var user = new User { Username = userName };
            _repository.Setup(r => r.GetById<User>(id)).Returns(user);

            var result = testObject.GetUser(id, false);

            Assert.IsType<MembershipUser>(result);
            Assert.Equal(userName, result.UserName);
        }

        [Fact]
        public void GetUser_Returns_Null_When_No_Match_is_Found_Back()
        {
            var id = Guid.NewGuid();
            var userName = "fred";
            _repository.Setup(r => r.GetById<User>(id)).Returns<User>(null);

            var result = testObject.GetUser(id, false);

            Assert.Null(result);
        }

        [Fact]
        public void GetUser_With_UserId_Updates_User_LastActivityDate_When_userIsOnline_is_True()
        {
            var startTime = DateTime.Now;
            var id = Guid.NewGuid();
            var userName = "fred";
            var user = new User { Username = userName };
            _repository.Setup(r => r.GetById<User>(id)).Returns(user);

            var result = testObject.GetUser(id, true);

            var stopTime = DateTime.Now;
            Assert.True(result.LastActivityDate >= startTime && result.LastActivityDate <= stopTime);
            _repository.Verify(r => r.Save<User>(user));
        }

        [Fact]
        public void GetUser_With_UserId_Does_NOT_Update_User_LastActivityDate_When_userIsOnline_is_False()
        {
            var startTime = DateTime.Now;
            var id = Guid.NewGuid();
            var user = new User { LastActivityDate = startTime };
            _repository.Setup(r => r.GetById<User>(id)).Returns(user);

            var result = testObject.GetUser(id, false);

            var stopTime = DateTime.Now;
            Assert.Equal(result.LastActivityDate, startTime);
            _repository.Verify(r => r.Save<User>(user), Times.Never());
        }
    }
}