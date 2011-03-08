using System.Collections.Generic;
using System.Linq;
using Moq;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;
using NHibernate;
using Xunit;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_FindUsersByNameTest : InMemoryDatabaseTest
    {
        private Mock<IRepository> _repository;
        private Mock<ISession> _session;
        private nHibernateMembershipProvider testObject;


        public nHibernateMembershipProvider_FindUsersByNameTest()
            : base(typeof (nHibernateMembershipProvider).Assembly)
        {
            _repository = new Mock<IRepository>();
            _session = new Mock<ISession>();
            testObject  = new nHibernateMembershipProvider(_repository.Object);

        }

        [Fact] //Would like test to prove exact object
        public void FindUsersByName_Creates_a_FindUsersByNameQuery_and_Passes_it_to_Repository()
        {
            var totalRecords = 0;

            testObject.FindUsersByName("foo", 0, 0, out totalRecords);

            _repository.Verify(r => r.GetQueryableList(It.IsAny<FindUsersByNameQuery>()));
        }

        [Fact]
        public void FindUsersByName_Transforms_Results_Into_a_MembershipCollection()
        {
            var totalRecords = 0;
            IQueryable<User> valueFunction = TestDataHelper.CreateUserListForNameSearch();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersByNameQuery>())).Returns(valueFunction);

            var result = testObject.FindUsersByName("a", 0, 0, out totalRecords);

            Assert.Equal<int>(3, result.Count);
            Assert.Equal<string>("a", result["a"].UserName);
            Assert.Equal<string>("aa", result["aa"].UserName);
            Assert.Equal<string>("aaa", result["aaa"].UserName);
        }

        [Fact]
        public void FindUsersByName_Returns_an_Empty_Collection_if_No_Matches_are_Found() {
            var totalRecords = 0;
            IQueryable<User> valueFunction = (new List<User>()).AsQueryable();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersByNameQuery>())).Returns(valueFunction);

            var result = testObject.FindUsersByName("bar", 0, 0, out totalRecords);

            Assert.Equal<int>(0, result.Count);            
        }

        [Fact]
        public void FindUsersByName_Sets_TotalRecords_Equal_to_Number_of_Records_Returned()
        {
            var totalRecords = 0;
            IQueryable<User> valueFunction = TestDataHelper.CreateUserListForEmailSearch();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersByNameQuery>())).Returns(valueFunction);

            var result = testObject.FindUsersByName("a", 0, 0, out totalRecords);

            Assert.Equal<int>(3, totalRecords);
            Assert.Equal<int>(result.Count, totalRecords);
        }
    }
}