using System.Data.Entity;
using Geev.Domain.Uow;

namespace Geev.EntityFramework.Uow
{
    public interface IEfUnitOfWorkFilterExecuter : IUnitOfWorkFilterExecuter
    {
        void ApplyCurrentFilters(IUnitOfWork unitOfWork, DbContext dbContext);
    }
}