using Geev.Authorization.Roles;
using Geev.Dependency;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.Events.Bus.Entities;
using Geev.Events.Bus.Handlers;

namespace Geev.Organizations
{
    /// <summary>
    /// Removes the role from all organization units when a role is deleted.
    /// </summary>
    public class OrganizationUnitRoleRemover : 
        IEventHandler<EntityDeletedEventData<GeevRoleBase>>, 
        ITransientDependency
    {
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public OrganizationUnitRoleRemover(
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository, 
            IUnitOfWorkManager unitOfWorkManager)
        {
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<GeevRoleBase> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(eventData.Entity.TenantId))
            {
                _organizationUnitRoleRepository.Delete(
                    uou => uou.RoleId == eventData.Entity.Id
                );
            }
        }
    }
}
