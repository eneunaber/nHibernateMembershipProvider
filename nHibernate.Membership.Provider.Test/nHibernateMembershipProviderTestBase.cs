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
        protected IUserTranslator _userTranslator;


        public nHibernateMembershipProviderTestBase()
            : base(typeof (nHibernateMembershipProvider).Assembly)
        {
            _repository = new Mock<IRepository>();
            _session = new Mock<ISession>();
            _queryFactory = new Mock<IQueryFactory>();
            _userTranslator = new UserTranslator();
            testObject = new nHibernateMembershipProvider(_repository.Object, _queryFactory.Object, _userTranslator);

        }
    }
}
