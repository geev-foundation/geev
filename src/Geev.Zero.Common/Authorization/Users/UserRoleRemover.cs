using Geev.Dependency;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.Events.Bus.Entities;
using Geev.Events.Bus.Handlers;

namespace Geev.Authorization.Users
{
    /// <summary>
    /// Removes the user from all user roles when a user is deleted.
    /// </summary>
    public class UserRoleRemover :
        IEventHandler<EntityDeletedEventData<GeevUserBase>>,
        ITransientDependency
    {
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserRoleRemover(
            IUnitOfWorkManager unitOfWorkManager, 
            IRepository<UserRole, long> userRoleRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userRoleRepository = userRoleRepository;
        }

        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<GeevUserBase> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(eventData.Entity.TenantId))
            {
                _userRoleRepository.Delete(
                    ur => ur.UserId == eventData.Entity.Id
                );
            }
        }
    }
}
