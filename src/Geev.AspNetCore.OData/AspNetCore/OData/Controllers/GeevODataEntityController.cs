﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Geev.Authorization;
using Geev.Domain.Entities;
using Geev.Domain.Repositories;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.OData.Controllers
{
    public abstract class GeevODataEntityController<TEntity> : GeevODataEntityController<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected GeevODataEntityController(IRepository<TEntity> repository)
            : base(repository)
        {

        }
    }

    public abstract class GeevODataEntityController<TEntity, TPrimaryKey> : GeevODataController
        where TPrimaryKey : IEquatable<TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected IRepository<TEntity, TPrimaryKey> Repository { get; private set; }
        
        protected GeevODataEntityController(IRepository<TEntity, TPrimaryKey> repository)
        {
            Repository = repository;
        }

        protected virtual string GetPermissionName { get; set; }

        protected virtual string GetAllPermissionName { get; set; }

        protected virtual string CreatePermissionName { get; set; }

        protected virtual string UpdatePermissionName { get; set; }

        protected virtual string DeletePermissionName { get; set; }

        [EnableQuery]
        public virtual IQueryable<TEntity> Get()
        {
            CheckGetAllPermission();

            return Repository.GetAll();
        }

        [EnableQuery]
        public virtual SingleResult<TEntity> Get([FromODataUri] TPrimaryKey key)
        {
            CheckGetPermission();

            var entity = Repository.GetAll().Where(e => e.Id.Equals(key));

            return SingleResult.Create(entity);
        }

        public virtual async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            CheckCreatePermission();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdEntity = await Repository.InsertAsync(entity);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            
            return Created(createdEntity);
        }

        public virtual async Task<IActionResult> Patch([FromODataUri] TPrimaryKey key, [FromBody] Delta<TEntity> entity)
        {
            CheckUpdatePermission();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var dbLookup = await Repository.GetAsync(key);
            if (dbLookup == null)
            {
                return NotFound();
            }
            
            entity.Patch(dbLookup);

            return Updated(entity);
        }

        public virtual async Task<IActionResult> Put([FromODataUri] TPrimaryKey key, [FromBody] TEntity update)
        {
            CheckUpdatePermission();
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(update.Id))
            {
                return BadRequest();
            }
            
            var updated = await Repository.UpdateAsync(update);

            return Updated(updated);
        }

        public virtual async Task<IActionResult> Delete([FromODataUri] TPrimaryKey key)
        {
            CheckDeletePermission();

            var product = await Repository.GetAsync(key);
            if (product == null)
            {
                return NotFound();
            }
            
            await Repository.DeleteAsync(key);

            return StatusCode((int)HttpStatusCode.NoContent);
        }

        protected virtual void CheckPermission(string permissionName)
        {
            if (!string.IsNullOrEmpty(permissionName))
            {
                PermissionChecker.Authorize(permissionName);
            }
        }

        protected virtual void CheckGetPermission()
        {
            CheckPermission(GetPermissionName);
        }

        protected virtual void CheckGetAllPermission()
        {
            CheckPermission(GetAllPermissionName);
        }

        protected virtual void CheckCreatePermission()
        {
            CheckPermission(CreatePermissionName);
        }

        protected virtual void CheckUpdatePermission()
        {
            CheckPermission(UpdatePermissionName);
        }

        protected virtual void CheckDeletePermission()
        {
            CheckPermission(DeletePermissionName);
        }
    }
}
