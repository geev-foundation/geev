using System.Data.Common;
using System.Threading.Tasks;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.Runtime.Session;
using Geev.Transactions.Extensions;
using NHibernate;

namespace Geev.NHibernate.Uow
{
    /// <summary>
    /// Implements Unit of work for NHibernate.
    /// </summary>
    public class NhUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        /// <summary>
        /// Gets NHibernate session object to perform queries.
        /// </summary>
        public ISession Session { get; private set; }

        /// <summary>
        /// <see cref="NhUnitOfWork"/> uses this DbConnection if it's set.
        /// This is usually set in tests.
        /// </summary>
        public DbConnection DbConnection { get; set; }

        private readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;

        /// <summary>
        /// Creates a new instance of <see cref="NhUnitOfWork"/>.
        /// </summary>
        public NhUnitOfWork(
            ISessionFactory sessionFactory,
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkDefaultOptions defaultOptions,
            IUnitOfWorkFilterExecuter filterExecuter)
            : base(
                  connectionStringResolver,
                  defaultOptions,
                  filterExecuter)
        {
            _sessionFactory = sessionFactory;
        }

        protected override void BeginUow()
        {
            Session = DbConnection != null
                ? _sessionFactory.WithOptions().Connection(DbConnection).OpenSession()
                : _sessionFactory.OpenSession();

            if (Options.IsTransactional == true)
            {
                _transaction = Options.IsolationLevel.HasValue
                    ? Session.BeginTransaction(Options.IsolationLevel.Value.ToSystemDataIsolationLevel())
                    : Session.BeginTransaction();
            }

            CheckAndSetSoftDelete();
            CheckAndSetMayHaveTenant();
            CheckAndSetMustHaveTenant();
        }

        protected virtual void CheckAndSetSoftDelete()
        {
            if (IsFilterEnabled(GeevDataFilters.SoftDelete))
            {
                ApplyEnableFilter(GeevDataFilters.SoftDelete); //Enable Filters
                ApplyFilterParameterValue(GeevDataFilters.SoftDelete, GeevDataFilters.Parameters.IsDeleted, false); //ApplyFilter
            }
            else
            {
                ApplyDisableFilter(GeevDataFilters.SoftDelete); //Disable filters
            }
        }

        protected virtual void CheckAndSetMustHaveTenant()
        {
            if (GeevSession.TenantId != null && IsFilterEnabled(GeevDataFilters.MustHaveTenant))
            {
                ApplyEnableFilter(GeevDataFilters.MustHaveTenant); //Enable Filters
                ApplyFilterParameterValue(GeevDataFilters.MustHaveTenant, GeevDataFilters.Parameters.TenantId, GeevSession.GetTenantId()); //ApplyFilter
            }
            else
            {
                ApplyDisableFilter(GeevDataFilters.MustHaveTenant); //Disable Filters
            }
        }

        protected virtual void CheckAndSetMayHaveTenant()
        {
            if (GeevSession.TenantId != null && IsFilterEnabled(GeevDataFilters.MayHaveTenant))
            {
                ApplyEnableFilter(GeevDataFilters.MayHaveTenant); //Enable Filters
                ApplyFilterParameterValue(GeevDataFilters.MayHaveTenant, GeevDataFilters.Parameters.TenantId, GeevSession.TenantId); //ApplyFilter
            }
            else
            {
                ApplyDisableFilter(GeevDataFilters.MayHaveTenant); //Disable Filters
            }
        }

        public override void SaveChanges()
        {
            Session.Flush();
        }

        public override Task SaveChangesAsync()
        {
            Session.Flush();
            return Task.FromResult(0);
        }

        /// <summary>
        /// Commits transaction and closes database connection.
        /// </summary>
        protected override void CompleteUow()
        {
            SaveChanges();
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        protected override Task CompleteUowAsync()
        {
            CompleteUow();
            return Task.FromResult(0);
        }

        /// <summary>
        /// Rollbacks transaction and closes database connection.
        /// </summary>
        protected override void DisposeUow()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            Session.Dispose();
        }
    }
}