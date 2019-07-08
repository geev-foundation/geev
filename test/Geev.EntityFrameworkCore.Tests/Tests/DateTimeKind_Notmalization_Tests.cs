﻿using System;
using System.Linq;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.EntityFrameworkCore.Tests.Domain;
using Geev.Timing;
using Shouldly;
using Xunit;

namespace Geev.EntityFrameworkCore.Tests.Tests
{
    public class DateTimeKind_Notmalization_Tests : EntityFrameworkCoreModuleTestBase
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IRepository<BlogCategory> _blogCategoryRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DateTimeKind_Notmalization_Tests()
        {
            _blogRepository = Resolve<IRepository<Blog>>();
            _blogCategoryRepository = Resolve<IRepository<BlogCategory>>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
        }

        [Fact]
        public void DateTime_Kind_Should_Be_Normalized_To_UTC_Test()
        {
            // Clock.Provider is set to 'ClockProviders.Utc' 
            // in the constructor of EntityFrameworkCoreModuleTestBase
            Clock.Kind.ShouldBe(DateTimeKind.Utc);

            //Act

            var blogs = _blogRepository.GetAllList();

            //Assert

            blogs.Count.ShouldBeGreaterThan(0);

            foreach (var blog in blogs)
            {
                blog.CreationTime.Kind.ShouldBe(DateTimeKind.Utc);
            }
        }

        [Fact]
        public void DateTime_Kind_Should_Not_Be_Normalized_Test()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var category = _blogCategoryRepository.GetAllList().FirstOrDefault();
                _blogCategoryRepository.EnsureCollectionLoaded(category, c => c.SubCategories);

                //Assert

                category.ShouldNotBeNull();
                category.CreationTime.Kind.ShouldBe(DateTimeKind.Unspecified);

                foreach (var subCategory in category.SubCategories)
                {
                    subCategory.CreationTime.Kind.ShouldBe(DateTimeKind.Unspecified);
                }

                uow.Complete();
            }
        }
    }
}
