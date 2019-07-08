using FluentNHibernate.Cfg;

namespace Geev.NHibernate.Configuration
{
    /// <summary>
    /// Used to configure ABP NHibernate module.
    /// </summary>
    public interface IGeevNHibernateModuleConfiguration
    {
        /// <summary>
        /// Used to get and modify NHibernate fluent configuration.
        /// You can add mappings to this object.
        /// Do not call BuildSessionFactory on it.
        /// </summary>
        FluentConfiguration FluentConfiguration { get; }
    }
}