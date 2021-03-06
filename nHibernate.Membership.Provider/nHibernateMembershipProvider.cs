﻿using System;
using System.Linq;
using System.Web.Security;
using nHibernate.Membership.Provider.Entities;
using nHibernate.Membership.Provider.Queries;

namespace nHibernate.Membership.Provider
{
    public interface ICustomMembershipProvider {
        byte[] ProviderDecryptPassword(byte[] encodedPassword);
        byte[] ProviderEncryptPassword(byte[] password);
        MembershipPasswordFormat PasswordFormat {get;}
    }

    public class nHibernateMembershipProvider : MembershipProvider, ICustomMembershipProvider
    {
        private IRepository _repository;
        private IQueryFactory _queryFactory;
        private IUserTranslator _userTranslator;

        public nHibernateMembershipProvider()
        {
        }

        public nHibernateMembershipProvider(IRepository repository, IQueryFactory queryFactory, IUserTranslator userTranslator)
        {
            this._repository = repository;
            this._queryFactory = queryFactory;
            this._userTranslator = userTranslator;
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

        public byte[] ProviderDecryptPassword(byte[] encodedPassword) {
            return DecryptPassword(encodedPassword);
        }

        public byte[] ProviderEncryptPassword(byte[] password)
        {
            return EncryptPassword(password);
        }

    
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
            var translatedUser = _userTranslator.TranslateToUser(user);
            _repository.Save<User>(translatedUser);
        }

        public override bool ValidateUser(string userName, string password)
        {
            var query = _queryFactory.createFindValidatedUserByUsernameQuery(userName, "myApp");
            var user = _repository.GetOne<User>(query);

            return false;
        }

        public override bool UnlockUser(string userName)
        {
            var query = _queryFactory.createFindUserByUsernameQuery(userName, "myApp");
            var user = _repository.GetOne<User>(query);
            user.IsLockedOut = false;
            user.LastLockedOutDate = DateTime.Now;
            try {
                _repository.Save<User>(user);
            }
            catch {
                return false;
            }
            return true;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var user = _repository.GetById<User>(providerUserKey);
            return CheckUserForActivityUpdate(user, userIsOnline);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var query = _queryFactory.createFindUserByUsernameQuery(username, "myApp");
            var user = _repository.GetOne<User>(query);
            return CheckUserForActivityUpdate(user, userIsOnline);
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

        
        //TODO: Can I test the first two lines
        public override int GetNumberOfUsersOnline()
        {
            var onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);
            var usersCurrentlyOnline = _repository.GetQueryableList<User>(_queryFactory.createUsersLastActivityQuery(compareTime, "myApp"));
            return usersCurrentlyOnline.Distinct().Count();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return FindUsersByQueryPaged(_queryFactory.createFindAllUsersQuery("myApp"), pageIndex, pageSize, out totalRecords);
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
                                                                 out int totalRecords)
        {
            return FindUsersByQueryPaged(_queryFactory.createFindUsersWithNameLikeQuery(usernameToMatch, "myApp"), pageIndex, pageSize, out totalRecords);
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
                                                                  out int totalRecords)
        {
            return FindUsersByQueryPaged(_queryFactory.createFindUsersWithEmailLikeQuery(emailToMatch, "myApp"), pageIndex, pageSize, out totalRecords);
        }
        #endregion

        #region "Private Methods"

        private MembershipUser FindSingleUserByQuery(QueryBase<User> query)
        {
            var user = _repository.GetOne<User>(query);
            if (user == null) return null;
            return _userTranslator.TranslateToMemberShipUser(user);
        }

        private MembershipUserCollection FindUsersByQuery(QueryBase<User> query)
        {
            var users = _repository.GetQueryableList<User>(query);
            return BuildUserCollectionFromQueryResults(users);
        }

        private MembershipUserCollection FindUsersByQueryPaged(QueryBase<User> query, int pageIndex, int pageSize, out int totalRecords)
        {
            var users = _repository.GetQueryableList<User>(query)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize);
            var userCollection = BuildUserCollectionFromQueryResults(users);
            totalRecords = userCollection.Count;
            return userCollection;        
        }

        private MembershipUserCollection BuildUserCollectionFromQueryResults(IQueryable<User> users)
        {
            var userCollection = new MembershipUserCollection();
            users.ToList().ForEach(user =>
                                   userCollection.Add(_userTranslator.TranslateToMemberShipUser(user))
                );
            return userCollection;
        }

        private MembershipUser CheckUserForActivityUpdate(User user, bool userIsOnline)
        {
            if (user == null) return null;
            UpdateUserLastActivityDate(userIsOnline, user);
            return _userTranslator.TranslateToMemberShipUser(user);        
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