using System.Threading.Tasks;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.MemoryDb.Configuration;

namespace Geev.MemoryDb.Uow
{
    /// <summary>
    /// Implements Unit of work for MemoryDb.
    /// </summary>
    public class MemoryDbUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        /// <summary>
        /// Gets a reference to Memory Database.
        /// </summary>
        public MemoryDatabase Database { get; private set; }

        private readonly IGeevMemoryDbModuleConfiguration _configuration;
        private readonly MemoryDatabase _memoryDatabase;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MemoryDbUnitOfWork(
            IGeevMemoryDbModuleConfiguration configuration, 
            MemoryDatabase memoryDatabase, 
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkFilterExecuter filterExecuter,
            IUnitOfWorkDefaultOptions defaultOptions)
            : base(
                  connectionStringResolver, 
                  defaultOptions,
                  filterExecuter)
        {
            _configuration = configuration;
            _memoryDatabase = memoryDatabase;
        }

        protected override void BeginUow()
        {
            Database = _memoryDatabase;
        }

        public override void SaveChanges()
        {

        }

        #pragma warning disable 1998
        public override async Task SaveChangesAsync()
        {

        }
        #pragma warning restore 1998

        protected override void CompleteUow()
        {

        }

        #pragma warning disable 1998
        protected override async Task CompleteUowAsync()
        {

        }
        #pragma warning restore 1998

        protected override void DisposeUow()
        {

        }
    }
}