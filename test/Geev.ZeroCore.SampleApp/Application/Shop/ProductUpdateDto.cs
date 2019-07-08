using System.Collections.Generic;
using Geev.Application.Services.Dto;

namespace Geev.ZeroCore.SampleApp.Application.Shop
{
    public class ProductUpdateDto: EntityDto
    {
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public ICollection<ProductTranslationDto> Translations { get; set; }
    }
}