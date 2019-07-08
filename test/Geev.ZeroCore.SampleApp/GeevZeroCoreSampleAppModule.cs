using Geev.AutoMapper;
using Geev.Configuration;
using Geev.EntityFrameworkCore.Configuration;
using Geev.Modules;
using Geev.Reflection.Extensions;
using Geev.Zero.EntityFrameworkCore;
using Geev.ZeroCore.SampleApp.Application;
using Geev.ZeroCore.SampleApp.Application.Shop;
using Geev.ZeroCore.SampleApp.Core.Shop;
using Geev.ZeroCore.SampleApp.EntityFramework;
using Geev.ZeroCore.SampleApp.EntityFramework.Seed;
using AutoMapper;

namespace Geev.ZeroCore.SampleApp
{
    [DependsOn(typeof(GeevZeroCoreEntityFrameworkCoreModule), typeof(GeevAutoMapperModule))]
    public class GeevZeroCoreSampleAppModule : GeevModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.GeevEfCore().AddDbContext<SampleAppDbContext>(configuration =>
                {
                    GeevZeroTemplateDbContextConfigurer.Configure(configuration.DbContextOptions, configuration.ConnectionString);
                });
            }

            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            Configuration.Features.Providers.Add<AppFeatureProvider>();

            Configuration.CustomConfigProviders.Add(new TestCustomConfigProvider());
            Configuration.CustomConfigProviders.Add(new TestCustomConfigProvider2());
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevZeroCoreSampleAppModule).GetAssembly());

            Configuration.Modules.GeevAutoMapper().Configurators.Add(configuration =>
            {
                CustomDtoMapper.CreateMappings(configuration, new MultiLingualMapContext(
                    IocManager.Resolve<ISettingManager>()
                ));
            });
        }

        public override void PostInitialize()
        {
            SeedHelper.SeedHostDb(IocManager);
        }
    }

    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration, MultiLingualMapContext context)
        {
            configuration.CreateMultiLingualMap<Product, ProductTranslation, ProductListDto>(context);

            configuration.CreateMap<ProductCreateDto, Product>();
            configuration.CreateMap<ProductUpdateDto, Product>();

            configuration.CreateMap<ProductTranslationDto, ProductTranslation>();

            configuration.CreateMultiLingualMap<Order, OrderTranslation, OrderListDto>(context)
                .EntityMap.ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));
        }
    }
}
