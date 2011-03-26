using Moq;
using NHibernate;
using nHibernate.Membership.Provider.Queries;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProviderTestBase : InMemoryDatabaseTest
    {
        protected Mock<IRepository> _repository;
        protected Mock<ISession> _session;
        protected Mock<IQueryFactory> _queryFactory;
        protected nHibernateMembershipProvider testObject;


        public nHibernateMembershipProviderTestBase()
            : base(typeof (nHibernateMembershipProvider).Assembly)
        {
            _repository = new Mock<IRepository>();
            _session = new Mock<ISession>();
            _queryFactory = new Mock<IQueryFactory>();
            testObject = new nHibernateMembershipProvider(_repository.Object, _queryFactory.Object);

        }
    }
}
