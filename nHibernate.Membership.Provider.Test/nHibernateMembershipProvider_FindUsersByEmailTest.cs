using System.Collections.Generic;
using System.Linq;
using Moq;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_FindUsersByEmailTest : nHibernateMembershipProviderTestBase
    {
        [Fact]
        public void FindUsersByEmail_Creates_a_FindUsersByEmailQuery_and_Passes_it_to_Repository()
        {
            var totalRecords = 0;
            var email = "a@b.com";
            var appName = "myApp";
            var findUsersByEmailQuery = new FindUsersByEmailQuery(email, appName);
            
            _queryFactory.Setup(qf => qf.createFindUsersByEmailQuery(email, appName)).Returns(findUsersByEmailQuery);


            testObject.FindUsersByEmail(email, 0, 0, out totalRecords);

            _repository.Verify(r => r.GetQueryableList(findUsersByEmailQuery));
        }

        [Fact]
        public void FindUsersByEmail_Transforms_Results_Into_a_MembershipCollection()
        {
            var totalRecords = 0;
            IQueryable<User> userList = TestDataHelper.CreateUserListForEmailSearch();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersByEmailQuery>())).Returns(userList);

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
            IQueryable<User> userList = (new List<User>()).AsQueryable();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersByEmailQuery>())).Returns(userList);

            var result = testObject.FindUsersByEmail("asdfasf@b.com", 0, 0, out totalRecords);

            Assert.Equal<int>(0, result.Count);
        }

        [Fact]
        public void FindUsersByEmail_Sets_TotalRecords_Equal_to_Number_of_Records_Returned()
        {
            var totalRecords = 0;
            IQueryable<User> userList = TestDataHelper.CreateUserListForEmailSearch();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersByEmailQuery>())).Returns(userList);

            var result = testObject.FindUsersByEmail("a@b.com", 0, 0, out totalRecords);

            Assert.Equal<int>(3, totalRecords);
            Assert.Equal<int>(result.Count, totalRecords);
        }
    }
}