using GeevAspNetCoreDemo.Core.Domain;
using Geev.AspNetCore.OData.Controllers;
using Geev.Dependency;
using Geev.Domain.Repositories;

namespace GeevAspNetCoreDemo.Controllers
{
    public class ProductsController : GeevODataEntityController<Product>, ITransientDependency
    {
        public ProductsController(IRepository<Product> repository) : base(repository)
        {
            GetPermissionName = "GetProductPermission";
            GetAllPermissionName = "GetAllProductsPermission";
            CreatePermissionName = "CreateProductPermission";
            UpdatePermissionName = "UpdateProductPermission";
            DeletePermissionName = "DeleteProductPermission";
        }
    }
}
