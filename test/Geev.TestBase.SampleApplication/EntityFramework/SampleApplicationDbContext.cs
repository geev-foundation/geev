using System.Data.Common;
using System.Data.Entity;
using Geev.Domain.Repositories;
using Geev.EntityFramework;
using Geev.TestBase.SampleApplication.ContacLists;
using Geev.TestBase.SampleApplication.Crm;
using Geev.TestBase.SampleApplication.EntityFramework.Repositories;
using Geev.TestBase.SampleApplication.GuidEntities;
using Geev.TestBase.SampleApplication.Messages;
using Geev.TestBase.SampleApplication.People;

namespace Geev.TestBase.SampleApplication.EntityFramework
{
    [AutoRepositoryTypes(
        typeof(IRepository<>),
        typeof(IRepository<,>),
        typeof(SampleApplicationEfRepositoryBase<>),
        typeof(SampleApplicationEfRepositoryBase<,>)
    )]
    public class SampleApplicationDbContext : GeevDbContext
    {
        public virtual IDbSet<ContactList> ContactLists { get; set; }

        public virtual IDbSet<Person> People { get; set; }

        public virtual IDbSet<Message> Messages { get; set; }

        public virtual IDbSet<Company> Companies { get; set; }

        public virtual IDbSet<Branch> Branches { get; set; }

        public virtual IDbSet<Hotel> Hotels { get; set; }

        public virtual IDbSet<Room> Rooms { get; set; }

        public virtual IDbSet<TestEntityWithGuidPk> TestEntityWithGuidPks { get; set; }

        public virtual IDbSet<TestEntityWithGuidPkAndDbGeneratedValue> TestEntityWithGuidPkAndDbGeneratedValues { get; set; }

        public SampleApplicationDbContext()
        {

        }

        public SampleApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public SampleApplicationDbContext(DbConnection connection)
            : base(connection, false)
        {

        }

        public SampleApplicationDbContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {

        }
    }
}
