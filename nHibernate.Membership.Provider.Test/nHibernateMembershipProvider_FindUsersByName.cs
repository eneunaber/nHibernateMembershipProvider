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
        public void FindUsersByEmail_Creates_a_FindUsersByEmailQuery_and_Passes_it_to_Repository()
        {

        }

    }
}