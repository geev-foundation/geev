using Geev.Domain.Entities;

namespace Geev.ZeroCore.SampleApp.Core.Shop
{
    public class ProductTranslation : Entity, IEntityTranslation<Product>
    {
        public virtual string Name { get; set; }

        public virtual Product Core { get; set; }

        public virtual int CoreId { get; set; }

        public virtual string Language { get; set; }
    }
}