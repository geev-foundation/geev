using System.Collections.Generic;
using Geev.Domain.Entities;

namespace Geev.ZeroCore.SampleApp.Core.Shop
{
    public class Product : Entity, IMultiLingualEntity<ProductTranslation>
    {
        public virtual decimal Price { get; set; }

        public virtual int Stock { get; set; }

        public virtual ICollection<ProductTranslation> Translations { get; set; }
    }
}
