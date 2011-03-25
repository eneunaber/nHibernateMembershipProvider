using Moq;
using NHibernate;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProviderTestBase : InMemoryDatabaseTest
    {
        protected Mock<IRepository> _repository;
        protected Mock<ISession> _session;
        protected nHibernateMembershipProvider testObject;


        public nHibernateMembershipProviderTestBase()
            : base(typeof (nHibernateMembershipProvider).Assembly)
        {
            _repository = new Mock<IRepository>();
            _session = new Mock<ISession>();
            testObject  = new nHibernateMembershipProvider(_repository.Object);

        }
    }
}
