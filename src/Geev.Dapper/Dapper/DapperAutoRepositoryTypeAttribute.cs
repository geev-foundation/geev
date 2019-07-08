using System;

using Geev.Domain.Repositories;

using JetBrains.Annotations;

namespace Geev.Dapper
{
    public class DapperAutoRepositoryTypeAttribute : AutoRepositoryTypesAttribute
    {
        public DapperAutoRepositoryTypeAttribute(
            [NotNull] Type repositoryInterface,
            [NotNull] Type repositoryInterfaceWithPrimaryKey,
            [NotNull] Type repositoryImplementation,
            [NotNull] Type repositoryImplementationWithPrimaryKey)
            : base(repositoryInterface, repositoryInterfaceWithPrimaryKey, repositoryImplementation, repositoryImplementationWithPrimaryKey)
        {
        }
    }
}
