using System.Collections.Generic;
using System.Linq;
using Moq;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_FindUsersByNameTest : nHibernateMembershipProviderTestBase
    {
        [Fact] //Would like test to prove exact object
        public void FindUsersWithNameLike_Creates_a_FindUsersByNameQuery_and_Passes_it_to_Repository()
        {
            var totalRecords = 0;
            var name = "foo";
            var appName = "myApp";
            var findUsersByNameQuery = new FindUsersWithNameLikeQuery(name, appName);

            _queryFactory.Setup(qf => qf.createFindUsersWithNameLikeQuery(name, appName)).Returns(findUsersByNameQuery);


            testObject.FindUsersByName(name, 0, 0, out totalRecords);

            _repository.Verify(r => r.GetQueryableList(findUsersByNameQuery));
        }

        [Fact]
        public void FindUsersWithNameLike_Transforms_Results_Into_a_MembershipCollection()
        {
            var totalRecords = 0;
            IQueryable<User> userList = TestDataHelper.CreateUserListForNameSearch();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersWithNameLikeQuery>())).Returns(userList);

            var result = testObject.FindUsersByName("a", 0, 0, out totalRecords);

            Assert.Equal<int>(3, result.Count);
            Assert.Equal<string>("a", result["a"].UserName);
            Assert.Equal<string>("aa", result["aa"].UserName);
            Assert.Equal<string>("aaa", result["aaa"].UserName);
        }

        [Fact]
        public void FindUsersWithNameLike_Returns_an_Empty_Collection_if_No_Matches_are_Found() {
            var totalRecords = 0;
            IQueryable<User> userList = (new List<User>()).AsQueryable();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersWithNameLikeQuery>())).Returns(userList);

            var result = testObject.FindUsersByName("bar", 0, 0, out totalRecords);

            Assert.Equal<int>(0, result.Count);            
        }

        [Fact]
        public void FindUsersWithNameLike_Sets_TotalRecords_Equal_to_Number_of_Records_Returned()
        {
            var totalRecords = 0;
            IQueryable<User> userList = TestDataHelper.CreateUserListForEmailSearch();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindUsersWithNameLikeQuery>())).Returns(userList);

            var result = testObject.FindUsersByName("a", 0, 0, out totalRecords);

            Assert.Equal<int>(3, totalRecords);
            Assert.Equal<int>(result.Count, totalRecords);
        }
    }
}