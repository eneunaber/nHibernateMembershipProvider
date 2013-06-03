using System.Web.Security;

namespace nHibernate.Membership.Provider
{
    public class PasswordChecker : IPasswordChecker
    {
        private IPasswordEncoder _passwordEncoder;

        public PasswordChecker(IPasswordEncoder passwordEncoder)
        {
            _passwordEncoder = passwordEncoder;
        }

        public bool CheckPassword(string password, string dbpassword, MembershipPasswordFormat passwordFormat)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (passwordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = _passwordEncoder.UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = _passwordEncoder.EncodePassword(password);
                    break;
                default:
                    break;
            }

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }
    }
}
