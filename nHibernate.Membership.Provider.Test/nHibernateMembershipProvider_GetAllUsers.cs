using System.Collections.Generic;
using System.Linq;
using Moq;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;
using Xunit;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_GetAllUsers : nHibernateMembershipProviderTestBase
    {
        [Fact]
        public void GetAllUsers_Creates_a_FindAllUsersQuery_and_Passes_it_to_Repository()
        {
            var totalRecords = 0;
            var result = testObject.GetAllUsers(0, 0, out totalRecords);

            _repository.Verify(r => r.GetQueryableList(It.IsAny<FindAllUsersQuery>()));
        }

        [Fact]
        public void GetAllUsers_Transforms_Results_Into_a_MembershipCollection()
        {
            var totalRecords = 0;
            IQueryable<User> userList = TestDataHelper.CreateUserListForNameSearch();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindAllUsersQuery>())).Returns(userList);

            var result = testObject.GetAllUsers(1, 100, out totalRecords);

            Assert.Equal<int>(3, result.Count);
            Assert.Equal<string>("a", result["a"].UserName);
            Assert.Equal<string>("aa", result["aa"].UserName);
            Assert.Equal<string>("aaa", result["aaa"].UserName);
        }

        [Fact]
        public void GetAllUsers_Returns_an_Empty_Collection_if_No_Matches_are_Found()
        {
            var totalRecords = 0;
            IQueryable<User> userList = (new List<User>()).AsQueryable();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindAllUsersQuery>())).Returns(userList);

            var result = testObject.GetAllUsers(1, 100, out totalRecords);

            Assert.Equal<int>(0, result.Count);            
        }

        [Fact]
        public void GetAllUsers_Sets_TotalRecords_Equal_to_Number_of_Records_Returned()
        {
            var totalRecords = 0;
            IQueryable<User> userList = TestDataHelper.CreateUserListForEmailSearch();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindAllUsersQuery>())).Returns(userList);

            var result = testObject.GetAllUsers(1, 100, out totalRecords);

            Assert.Equal<int>(3, totalRecords);
            Assert.Equal<int>(result.Count, totalRecords);
        }

        [Fact]
        public void GetAllUsers_Returns_Only_Users_From_the_Page_Requested()
        {
            var totalRecords = 0;
            IQueryable<User> userList = TestDataHelper.CreateUserListForNameSearch();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<FindAllUsersQuery>())).Returns(userList);
            var pageIndex = 3;
            var pageSize = 1;


            var result = testObject.GetAllUsers(pageIndex, pageSize, out totalRecords);

            Assert.Equal<int>(1, totalRecords);
            Assert.Equal<string>("aaa", result["aaa"].UserName);
        }
    }
}