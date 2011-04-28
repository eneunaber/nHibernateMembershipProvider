using System;
using System.Web.Security;
using nHibernate.Membership.Provider.Entities;

namespace nHibernate.Membership.Provider
{
    public class UserTranslator : nHibernate.Membership.Provider.IUserTranslator
    {
        public MembershipUser TranslateToMemberShipUser(Entities.User user)
        {
            return new MembershipUser("nHibernateMembershipProvider",
                                        user.Username, user.Id, user.Email,
                                        user.PasswordQuestion, user.Comment,
                                        user.IsApproved, user.IsLockedOut,
                                        user.CreationDate, user.LastLoginDate,
                                        user.LastActivityDate,
                                        user.LastPasswordChangedDate,
                                        user.LastLockedOutDate); ;
        }

        public Entities.User TranslateToUser(MembershipUser membershipUser)
        {
            return new User
            {
                Username = membershipUser.UserName,
                Id = (Guid) membershipUser.ProviderUserKey,
                Email = membershipUser.Email,
                PasswordQuestion = membershipUser.PasswordQuestion,
                Comment = membershipUser.Comment,
                IsApproved = membershipUser.IsApproved,
                IsLockedOut = membershipUser.IsLockedOut,
                CreationDate = membershipUser.CreationDate,
                LastLoginDate = membershipUser.LastLoginDate,
                LastActivityDate = membershipUser.LastActivityDate,
                LastPasswordChangedDate = membershipUser.LastPasswordChangedDate,
                LastLockedOutDate = membershipUser.LastLockoutDate
            };
        }
    }
}
