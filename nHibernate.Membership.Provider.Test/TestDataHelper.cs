using System;
using System.Collections.Generic;
using System.Linq;
using nHibernate.Membership.Provider.Entities;
using System.Web.Security;

namespace nHibernate.Membership.Provider.Test
{
    public class TestDataHelper
    {

        public static IQueryable<User> CreateUserListForEmailSearch()
        {
            return (new List<User>()
                             {
                                 new User {Username = "a", Email = "a@b.com", Id = Guid.NewGuid(), ApplicationName = "UnitTest", 
                                     Comment = string.Empty, CreationDate = DateTime.Now, FailedPasswordAnswerAttemptCount = 0, 
                                     FailedPasswordAnswerAttemptWindowStart = DateTime.Now, FailedPasswordAttemptCount = 0, 
                                     FailedPasswordAttemptWindowStart = DateTime.Now},

                                 new User {Username = "b", Email = "a@b.com", Id = Guid.NewGuid(), ApplicationName = "UnitTest", 
                                     Comment = string.Empty, CreationDate = DateTime.Now, FailedPasswordAnswerAttemptCount = 0, 
                                     FailedPasswordAnswerAttemptWindowStart = DateTime.Now, FailedPasswordAttemptCount = 0, 
                                     FailedPasswordAttemptWindowStart = DateTime.Now},

                                 new User {Username = "c", Email = "a@b.com", Id = Guid.NewGuid(), ApplicationName = "UnitTest", 
                                     Comment = string.Empty, CreationDate = DateTime.Now, FailedPasswordAnswerAttemptCount = 0, 
                                     FailedPasswordAnswerAttemptWindowStart = DateTime.Now, FailedPasswordAttemptCount = 0, 
                                     FailedPasswordAttemptWindowStart = DateTime.Now}

                            }).AsQueryable();
        }


        public static IQueryable<User> CreateUserListForNameSearch()
        {
            return (new List<User>()
                             {
                                 new User {Username = "a", Email = "a@b.com", Id = Guid.NewGuid(), ApplicationName = "UnitTest", 
                                     Comment = string.Empty, CreationDate = DateTime.Now, FailedPasswordAnswerAttemptCount = 0, 
                                     FailedPasswordAnswerAttemptWindowStart = DateTime.Now, FailedPasswordAttemptCount = 0, 
                                     FailedPasswordAttemptWindowStart = DateTime.Now},

                                 new User {Username = "aa", Email = "a@b.com", Id = Guid.NewGuid(), ApplicationName = "UnitTest", 
                                     Comment = string.Empty, CreationDate = DateTime.Now, FailedPasswordAnswerAttemptCount = 0, 
                                     FailedPasswordAnswerAttemptWindowStart = DateTime.Now, FailedPasswordAttemptCount = 0, 
                                     FailedPasswordAttemptWindowStart = DateTime.Now},

                                 new User {Username = "aaa", Email = "a@b.com", Id = Guid.NewGuid(), ApplicationName = "UnitTest", 
                                     Comment = string.Empty, CreationDate = DateTime.Now, FailedPasswordAnswerAttemptCount = 0, 
                                     FailedPasswordAnswerAttemptWindowStart = DateTime.Now, FailedPasswordAttemptCount = 0, 
                                     FailedPasswordAttemptWindowStart = DateTime.Now}

                            }).AsQueryable();
        }

        public static MembershipUser CreateMembershipUser(string userName, int id, string email) {
            return new MembershipUser("nHibernateMembershipProvider",
                                        userName, id, email,
                                        "PasswordQuestion", "Comment",
                                        true, false,
                                        DateTime.Now, DateTime.Now,
                                        DateTime.Now,
                                        DateTime.Now,
                                        DateTime.Now);
        
        }

    }
}
