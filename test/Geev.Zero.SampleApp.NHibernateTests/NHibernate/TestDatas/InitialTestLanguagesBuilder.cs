using System.Linq;
using Geev.Localization;
using Geev.Zero.SampleApp.MultiTenancy;
using NHibernate;
using NHibernate.Linq;

namespace Geev.Zero.SampleApp.NHibernate.TestDatas
{
    public class InitialTestLanguagesBuilder
    {
        private readonly ISession _session;

        public InitialTestLanguagesBuilder(ISession session)
        {
            _session = session;
        }

        public void Build()
        {
            InitializeLanguagesOnDatabase();
        }

        private void InitializeLanguagesOnDatabase()
        {
            var defaultTenant = _session.Query<Tenant>().Single(t => t.Name == Tenant.DefaultTenantName);
            
            //Host languages
            _session.Save(new ApplicationLanguage { Name = "en", DisplayName = "English" });
            _session.Save(new ApplicationLanguage { Name = "tr", DisplayName = "Türkçe" });
            _session.Save(new ApplicationLanguage { Name = "de", DisplayName = "German" });

            //Default tenant languages
            _session.Save(new ApplicationLanguage { Name = "zh-Hans", DisplayName = "简体中文", TenantId = defaultTenant.Id });
        }
    }
}