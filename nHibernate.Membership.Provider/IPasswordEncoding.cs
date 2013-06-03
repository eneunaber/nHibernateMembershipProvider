using System.Web.Security;
namespace nHibernate.Membership.Provider
{
    public interface IPasswordEncoder
    {
        string EncodePassword(string password);
        string UnEncodePassword(string encodedPassword);
    }
}
