using System;
using System.Web.Security;
using Moq;
using nHibernate.Membership.Provider.Entities;
using Xunit;

namespace nHibernate.Membership.Provider.Test
{
    public class nHibernateMembershipProvider_UpdateUserTest : nHibernateMembershipProviderTestBase
    {
        [Fact]
        public void UpdateUser_Translates_MembershipUser_and_Passes_User_to_Repository()
        {
            Mock<IUserTranslator> _userTranslator = new Mock<IUserTranslator>();
            testObject = new nHibernateMembershipProvider(_repository.Object, _queryFactory.Object, _userTranslator.Object);

            var memberShipUser = new MembershipUser("nHibernateMembershipProvider",
                                        "userName", -1, "me@you.com",
                                        "PasswordQuestion", "Comment",
                                        true, false,
                                        DateTime.Now, DateTime.Now,
                                        DateTime.Now,
                                        DateTime.Now,
                                        DateTime.Now);

            var user = new User();

            _userTranslator.Setup(ut => ut.TranslateToUser(memberShipUser)).Returns(user);

            testObject.UpdateUser(memberShipUser);

            _repository.Verify(r => r.Save<User>(user));
        }

    }
}