using System;
using System.Linq;
using System.Linq.Expressions;
using Geev.EntityFrameworkCore.Tests.Domain;

namespace Geev.EntityFrameworkCore.Tests.Ef
{
    public class TicketListItemRepository : SupportRepositoryBase<TicketListItem>
    {
        private IQueryable<TicketListItem> View => Context.TicketListItems;

        public TicketListItemRepository(IDbContextProvider<SupportDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public override IQueryable<TicketListItem> GetAllIncluding(params Expression<Func<TicketListItem, object>>[] propertySelectors) => View;
    }
}
