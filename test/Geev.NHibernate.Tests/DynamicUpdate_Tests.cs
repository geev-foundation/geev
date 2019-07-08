using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.NHibernate.Tests.Entities;
using Shouldly;
using Xunit;

namespace Geev.NHibernate.Tests
{
    public class DynamicUpdate_Tests : NHibernateTestBase
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DynamicUpdate_Tests()
        {
            _bookRepository = Resolve<IRepository<Book>>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
            UsingSession(session => session.Save(new Book { Name = "Hitchhikers Guide to the Galaxy" }));
        }

        [Fact]
        public void Should_Set_CreatorUserId_When_DynamicInsert_Is_Enabled()
        {
            GeevSession.UserId = 1;

            using (var uow = _unitOfWorkManager.Begin())
            {
                var book = _bookRepository.Get(1);
                book.ShouldNotBeNull();
                book.Name = "Hitchhiker's Guide to the Galaxy";
                _bookRepository.Update(book);
                uow.Complete();
            }

            var book2 = _bookRepository.Get(1);
            book2.LastModifierUserId.ShouldNotBeNull();
        }
    }
}
