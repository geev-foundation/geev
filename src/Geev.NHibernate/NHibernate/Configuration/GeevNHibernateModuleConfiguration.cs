using FluentNHibernate.Cfg;

namespace Geev.NHibernate.Configuration
{
    internal class GeevNHibernateModuleConfiguration : IGeevNHibernateModuleConfiguration
    {
        public FluentConfiguration FluentConfiguration { get; }

        public GeevNHibernateModuleConfiguration()
        {
            FluentConfiguration = Fluently.Configure();
        }
    }
}