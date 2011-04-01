using System;
using System.Linq;
using System.Web.Security;
using NHibernate;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;

namespace nHibernate.Membership.Provider
{
    public class nHibernateMembershipProvider : MembershipProvider
    {
        private IRepository _repository;
        private IQueryFactory _queryFactory;

        public nHibernateMembershipProvider()
        {
        }

        public nHibernateMembershipProvider(IRepository repository, IQueryFactory queryFactory)
        {
            this._repository = repository;
            this._queryFactory = queryFactory;
        }

        #region "Fields"
        private bool _enablePasswordReset;
        private bool _enablePasswordRetrieval;
        private int _maxInvalidPasswordAttempts;
        private int _minRequiredNonAlphanumericCharacters;
        private int _minRequiredPasswordLength;
        private int _passwordAttemptWindow;
        private MembershipPasswordFormat _passwordFormat;
        private string _passwordStrengthRegularExpression;
        private bool _requiresQuestionAndAnswer;
        private bool _requiresUniqueEmail;
        #endregion

        #region "Properties"
        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }

        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return _requiresQuestionAndAnswer; }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }

        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _passwordFormat; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonAlphanumericCharacters; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }
        #endregion

        #region "Public Overridden Members"
        public override MembershipUser CreateUser(string username, string password, string email,
                                                  string passwordQuestion, string passwordAnswer, bool isApproved,
                                                  object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
                                                             string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var user = _repository.GetById<User>(providerUserKey);
            if (user == null) return null;
            UpdateUserLastActivityDate(userIsOnline, user);
            return createMembershipUser(user);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var query = _queryFactory.createFindUserByUsernameQuery(username, "myApp");
            var users = _repository.GetQueryableList<User>(query);
            if (users.Count() == 0) return null;
            var selectedUser = (from user in users
                        select user).First();
            UpdateUserLastActivityDate(userIsOnline, selectedUser);
            return createMembershipUser(selectedUser);

        }

        public override string GetUserNameByEmail(string email)
        {
            var user = FindSingleUserByQuery(_queryFactory.createFindUserByEmailQuery(email, "myApp"));
            return user != null? user.UserName : string.Empty;
        }

        //TODO: Do we want to change the way Delete works? Maybe pass in the User object vs doing 3 queries
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var user = _repository.GetOne<User>(_queryFactory.createFindUsersWithNameLikeQuery(username, "myApp"));
            if (user == null) 
                return false;
            _repository.Delete<User>(user.Id);
            return true;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var userCollection = FindUsersByQuery(_queryFactory.createFindAllUsersQuery("myApp"), pageIndex, pageSize);
            totalRecords = userCollection.Count;
            return userCollection;
        }

        //TODO: Can I test the first two lines
        public override int GetNumberOfUsersOnline()
        {
            var onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);
            var usersCurrentlyOnline = _repository.GetQueryableList<User>(_queryFactory.createUsersLastActivityQuery(compareTime, "myApp"));
            return usersCurrentlyOnline.Distinct().Count();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
                                                                 out int totalRecords)
        {
            var userCollection = FindUsersByQuery(_queryFactory.createFindUsersWithNameLikeQuery(usernameToMatch, "myApp"), pageIndex, pageSize);
            totalRecords = userCollection.Count;
            return userCollection;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
                                                                  out int totalRecords)
        {
            var userCollection = FindUsersByQuery(_queryFactory.createFindUsersWithEmailLikeQuery(emailToMatch, "myApp"), pageIndex, pageSize);
            totalRecords = userCollection.Count;
            return userCollection;
        }
        #endregion

        #region "Private Methods"

        private MembershipUser FindSingleUserByQuery(QueryBase<User> query)
        {
            var users = _repository.GetQueryableList<User>(query);
            if (users.Count() == 0) return null;
            return createMembershipUser((from user in users
                                         select user).First());
        }

        private MembershipUserCollection FindUsersByQuery(QueryBase<User> query)
        {
            var users = _repository.GetQueryableList<User>(query);
            return BuildUserCollectionFromQueryResults(users);
        }

        private MembershipUserCollection FindUsersByQuery(QueryBase<User> query, int pageIndex, int pageSize)
        {
            var users = _repository.GetQueryableList<User>(query)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize);
            return BuildUserCollectionFromQueryResults(users);
        }

        private static MembershipUserCollection BuildUserCollectionFromQueryResults(IQueryable<User> users)
        {
            var userCollection = new MembershipUserCollection();
            users.ToList().ForEach(user =>
                                   userCollection.Add(createMembershipUser(user))
                );
            return userCollection;
        }

        private static MembershipUser createMembershipUser(User user)
        {
            return new MembershipUser("nHibernateMembershipProvider",
                                        user.Username, user.Id, user.Email,
                                        user.PasswordQuestion, user.Comment,
                                        user.IsApproved, user.IsLockedOut,
                                        user.CreationDate, user.LastLoginDate,
                                        user.LastActivityDate,
                                        user.LastPasswordChangedDate,
                                        user.LastLockedOutDate);
        }

        private void UpdateUserLastActivityDate(bool userIsOnline, User user)
        {
            if (userIsOnline)
            {
                user.LastActivityDate = DateTime.Now;
                _repository.Save<User>(user);
            }
        }

        #endregion

    }
}