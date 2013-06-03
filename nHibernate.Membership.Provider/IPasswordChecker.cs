using System.Web.Security;
namespace nHibernate.Membership.Provider
{
    public interface IPasswordChecker
    {
        bool CheckPassword(string password, string dbpassword, MembershipPasswordFormat passwordFormat);
    }
}
