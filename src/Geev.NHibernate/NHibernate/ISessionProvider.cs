using NHibernate;

namespace Geev.NHibernate
{
    public interface ISessionProvider
    {
        ISession Session { get; }
    }
}