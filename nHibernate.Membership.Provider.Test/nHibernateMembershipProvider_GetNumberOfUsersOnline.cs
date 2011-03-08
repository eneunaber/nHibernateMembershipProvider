using System.Collections.Generic;
using System.Linq;
using Moq;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;
using NHibernate;
using Xunit;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_GetNumberOfUsersOnline : InMemoryDatabaseTest
    {
        private Mock<IRepository> _repository;
        private Mock<ISession> _session;
        private nHibernateMembershipProvider testObject;


        public nHibernateMembershipProvider_GetNumberOfUsersOnline()
            : base(typeof (nHibernateMembershipProvider).Assembly)
        {
            _repository = new Mock<IRepository>();
            _session = new Mock<ISession>();
            testObject  = new nHibernateMembershipProvider(_repository.Object);

        }

        [Fact]
        public void GetNumberOfUsersOnline_Creates_a_UsersLastActivityQuery_and_Passes_it_to_Repository()
        {
            var result = testObject.GetNumberOfUsersOnline();

            _repository.Verify(r => r.GetQueryableList(It.IsAny<UsersLastActivityQuery>()));
        }


        [Fact]
        public void GetNumberOfUsersOnline_Applies_and_Returns_Count_to_Results_Returned_by_Repository()
        {
            IQueryable<User> expectedResults = TestDataHelper.CreateUserListForEmailSearch();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<UsersLastActivityQuery>())).Returns(expectedResults); 

            var result = testObject.GetNumberOfUsersOnline();

            Assert.Equal(3, result);
        }


        [Fact]
        public void GetNumberOfUsersOnline_Returns_0_Count_if_Returned_by_Repository()
        {
            IQueryable<User> expectedResults = new List<User>().AsQueryable();
            _repository.Setup(r => r.GetQueryableList(It.IsAny<UsersLastActivityQuery>())).Returns(expectedResults);

            var result = testObject.GetNumberOfUsersOnline();

            Assert.Equal(0, result);
        }
       
    }
}