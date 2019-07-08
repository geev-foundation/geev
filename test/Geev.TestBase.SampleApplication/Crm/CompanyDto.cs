using System;
using Geev.Application.Services.Dto;
using Geev.AutoMapper;

namespace Geev.TestBase.SampleApplication.Crm
{
    [AutoMapFrom(typeof(Company))]
    public class CompanyDto : EntityDto
    {
        public string Name { get; set; }

        public DateTime CreationTime { get; set; }

        public Address ShippingAddress { get; set; }

        public Address BillingAddress { get; set; }
    }
}