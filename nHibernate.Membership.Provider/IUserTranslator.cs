using System.Web.Security;
using nHibernate.Membership.Provider.Entities;
namespace nHibernate.Membership.Provider
{
    public interface IUserTranslator
    {
        MembershipUser TranslateToMemberShipUser(User user);
        User TranslateToUser(MembershipUser membershipUser);
    }
}
