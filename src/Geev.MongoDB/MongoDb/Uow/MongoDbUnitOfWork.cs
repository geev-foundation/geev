using System.Threading.Tasks;
using Geev.Dependency;
using Geev.Domain.Uow;
using Geev.MongoDb.Configuration;
using MongoDB.Driver;

namespace Geev.MongoDb.Uow
{
    /// <summary>
    /// Implements Unit of work for MongoDB.
    /// </summary>
    public class MongoDbUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        /// <summary>
        /// Gets a reference to MongoDB Database.
        /// </summary>
        public MongoDatabase Database { get; private set; }

        private readonly IGeevMongoDbModuleConfiguration _configuration;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MongoDbUnitOfWork(
            IGeevMongoDbModuleConfiguration configuration, 
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkFilterExecuter filterExecuter,
            IUnitOfWorkDefaultOptions defaultOptions)
            : base(
                  connectionStringResolver, 
                  defaultOptions,
                  filterExecuter)
        {
            _configuration = configuration;
        }

        #pragma warning disable
        protected override void BeginUow()
        {
            //TODO: MongoClientExtensions.GetServer(MongoClient)' is obsolete: 'Use the new API instead.
            Database = new MongoClient(_configuration.ConnectionString)
                .GetServer()
                .GetDatabase(_configuration.DatabaseName);
        }
        #pragma warning restore

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