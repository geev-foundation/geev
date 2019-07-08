using System.ComponentModel.DataAnnotations;
using Geev.AutoMapper;
using GeevAspNetCoreDemo.Core.Domain;

namespace GeevAspNetCoreDemo.Core.Application.Dtos
{
    [AutoMapTo(typeof(Product))]
    public class ProductCreateInput
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public float? Price { get; set; }
    }
}