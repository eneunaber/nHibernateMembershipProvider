using System;
using System.Web.Security;
using nHibernate.Membership.Provider.Entities;
using Xunit;

namespace nHibernate.Membership.Provider.Test
{
    public class UserTranslatorTests
    {
        string Username = "fred";
        Guid Id = Guid.NewGuid();
        string Email = "me@you.com";
        string PasswordQuestion = "mother?";
        string Comment = "I pitty the fool";
        Boolean IsApproved = true;
        Boolean IsLockedOut = false;

        DateTime CreationDate = DateTime.Now;
        DateTime LastLoginDate = DateTime.Now;
        DateTime LastActivityDate = DateTime.Now;
        DateTime LastPasswordChangedDate = DateTime.Now;
        DateTime LastLockedOutDate = DateTime.Now;

        string ProviderName = "nHibernateMembershipProvider";
        
        User user;
        UserTranslator testObject;

        public UserTranslatorTests() {
            user = new User
            {
                Username = Username,
                Id = Id,
                Email = Email,
                PasswordQuestion = PasswordQuestion,
                Comment = Comment,
                IsApproved = IsApproved,
                IsLockedOut = IsLockedOut,
                CreationDate = CreationDate,
                LastLoginDate = LastLoginDate,
                LastActivityDate = LastActivityDate,
                LastPasswordChangedDate = LastPasswordChangedDate,
                LastLockedOutDate = LastLockedOutDate
            };

            testObject = new UserTranslator();
        }


        [Fact]
        public void TranslateToMemberShipUser_Translates_User_to_MembershipUser()
        {

            MembershipUser result = testObject.TranslateToMemberShipUser(user);

            Assert.Equal(Username, result.UserName);
            Assert.Equal(Id, result.ProviderUserKey);
            Assert.Equal(Email, result.Email);
            Assert.Equal(PasswordQuestion, result.PasswordQuestion);
            Assert.Equal(Comment, result.Comment);
            Assert.Equal(IsApproved, result.IsApproved);
            Assert.Equal(IsLockedOut, result.IsLockedOut);

            Assert.Equal(CreationDate, result.CreationDate);
            Assert.Equal(LastLoginDate, result.LastLoginDate);
            Assert.Equal(LastActivityDate, result.LastActivityDate);
            Assert.Equal(LastPasswordChangedDate, result.LastPasswordChangedDate);
            Assert.Equal(LastLockedOutDate, result.LastLockoutDate);

            Assert.Equal(ProviderName, result.ProviderName);

        }

        [Fact]
        public void TranslateToUser_Translates_MembershipUser_to_User()
        {
            //Easier to double translate than set up all the data again
            MembershipUser memUser = testObject.TranslateToMemberShipUser(user);
            User result = testObject.TranslateToUser(memUser);

            Assert.Equal(Username, result.Username);
            Assert.Equal(Id, result.Id);
            Assert.Equal(Email, result.Email);
            Assert.Equal(PasswordQuestion, result.PasswordQuestion);
            Assert.Equal(Comment, result.Comment);
            Assert.Equal(IsApproved, result.IsApproved);
            Assert.Equal(IsLockedOut, result.IsLockedOut);

            Assert.Equal(CreationDate, result.CreationDate);
            Assert.Equal(LastLoginDate, result.LastLoginDate);
            Assert.Equal(LastActivityDate, result.LastActivityDate);
            Assert.Equal(LastPasswordChangedDate, result.LastPasswordChangedDate);
            Assert.Equal(LastLockedOutDate, result.LastLockedOutDate);
        }
    }
}
