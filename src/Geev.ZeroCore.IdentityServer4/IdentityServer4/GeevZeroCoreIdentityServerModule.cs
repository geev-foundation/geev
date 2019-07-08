using Geev.AutoMapper;
using Geev.Modules;
using Geev.Reflection.Extensions;
using Geev.Zero;
using IdentityServer4.Models;

namespace Geev.IdentityServer4
{
    [DependsOn(typeof(GeevZeroCoreModule), typeof(GeevAutoMapperModule))]
    public class GeevZeroCoreIdentityServerModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.GeevAutoMapper().Configurators.Add(config =>
            {
                //PersistedGrant -> PersistedGrantEntity
                config.CreateMap<PersistedGrant, PersistedGrantEntity>()
                    .ForMember(d => d.Id, c => c.MapFrom(s => s.Key));

                //PersistedGrantEntity -> PersistedGrant
                config.CreateMap<PersistedGrantEntity, PersistedGrant>()
                    .ForMember(d => d.Key, c => c.MapFrom(s => s.Id));
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevZeroCoreIdentityServerModule).GetAssembly());
        }
    }
}
