using System;

namespace nHibernate.Membership.Provider.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string ApplicationName { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Password { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public Boolean IsApproved { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime CreationDate { get; set; }
        public Boolean IsOnLine { get; set; }
        public Boolean IsLockedOut { get; set; }
        public DateTime LastLockedOutDate { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public DateTime FailedPasswordAttemptWindowStart { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
    }
}