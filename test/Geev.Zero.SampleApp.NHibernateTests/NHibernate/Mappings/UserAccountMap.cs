using Geev.Authorization.Users;
using Geev.NHibernate.EntityMappings;

namespace Geev.Zero.SampleApp.NHibernate.Mappings
{
    public class UserAccountMap : EntityMap<UserAccount, long>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserAccountMap() : base("GeevUserAccounts")
        {
            Map(x => x.TenantId);
            Map(x => x.UserId);
            Map(x => x.UserName);
            Map(x => x.EmailAddress);
            Map(x => x.UserLinkId);

            this.MapFullAudited();
        }
    }
}
