using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Moq;
using NHibernate;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_FindUsersByEmailTest : InMemoryDatabaseTest
    {
        private Mock<IRepository> _repository;
        private Mock<ISession> _session;
        private nHibernateMembershipProvider testObject;


        public nHibernateMembershipProvider_FindUsersByEmailTest()
            : base(typeof (nHibernateMembershipProvider).Assembly)
        {
            _repository = new Mock<IRepository>();
            _session = new Mock<ISession>();
            testObject  = new nHibernateMembershipProvider(_repository.Object);

        }

        [Fact] //Would like test to prove exact object
        public void FindUsersByEmail_Creates_a_FindUsersByEmailQuery_and_Passes_it_to_Repository()
        {
            var totalRecords = 0;

            testObject.FindUsersByEmail("a@b.com", 0, 0, out totalRecords);

            _repository.Verify(r => r.GetQueryableList(It.IsAny<FindUsersByEmailQuery>()));
        }

        
        [Fact]
        public void FindUsersByEmail_Transforms_Results_Into_a_MembershipCollection()
        {
            var totalRecords = 0;
            IQueryable<User> valueFunction = createUserList();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersByEmailQuery>())).Returns(valueFunction);

            var result = testObject.FindUsersByEmail("a@b.com", 0, 0, out totalRecords);

            Assert.Equal<int>(3, result.Count);
            Assert.Equal<string>("a@b.com", result["a"].Email);
            Assert.Equal<string>("a@b.com", result["b"].Email);
            Assert.Equal<string>("a@b.com", result["c"].Email);
        }

        [Fact]
        public void FindUsersByEmail_Returns_an_Empty_Collection_if_No_Matches_are_Found()
        {
            var totalRecords = 0;
            IQueryable<User> valueFunction = (new List<User>()).AsQueryable();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersByEmailQuery>())).Returns(valueFunction);

            var result = testObject.FindUsersByEmail("asdfasf@b.com", 0, 0, out totalRecords);

            Assert.Equal<int>(0, result.Count);            
        }

        [Fact]
        public void FindUsersByEmail_Sets_TotalRecords_Equal_to_Number_of_Records_Returned()
        {
            var totalRecords = 0;
            IQueryable<User> valueFunction = createUserList();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersByEmailQuery>())).Returns(valueFunction);

            var result = testObject.FindUsersByEmail("a@b.com", 0, 0, out totalRecords);

            Assert.Equal<int>(3, totalRecords);
            Assert.Equal<int>(result.Count, totalRecords);
        }


        private IQueryable<User> createUserList()
        {
            return (new List<User>()
                             {
                                 new User {Username = "a", Email = "a@b.com", Id = Guid.NewGuid(), ApplicationName = "UnitTest", 
                                     Comment = string.Empty, CreationDate = DateTime.Now, FailedPasswordAnswerAttemptCount = 0, 
                                     FailedPasswordAnswerAttemptWindowStart = DateTime.Now, FailedPasswordAttemptCount = 0, 
                                     FailedPasswordAttemptWindowStart = DateTime.Now},

                                 new User {Username = "b", Email = "a@b.com", Id = Guid.NewGuid(), ApplicationName = "UnitTest", 
                                     Comment = string.Empty, CreationDate = DateTime.Now, FailedPasswordAnswerAttemptCount = 0, 
                                     FailedPasswordAnswerAttemptWindowStart = DateTime.Now, FailedPasswordAttemptCount = 0, 
                                     FailedPasswordAttemptWindowStart = DateTime.Now},

                                 new User {Username = "c", Email = "a@b.com", Id = Guid.NewGuid(), ApplicationName = "UnitTest", 
                                     Comment = string.Empty, CreationDate = DateTime.Now, FailedPasswordAnswerAttemptCount = 0, 
                                     FailedPasswordAnswerAttemptWindowStart = DateTime.Now, FailedPasswordAttemptCount = 0, 
                                     FailedPasswordAttemptWindowStart = DateTime.Now}

                            }).AsQueryable();
        }
    }
}