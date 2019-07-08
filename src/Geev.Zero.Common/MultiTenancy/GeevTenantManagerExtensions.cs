using System.Collections.Generic;
using Geev.Authorization.Users;
using Geev.Threading;

namespace Geev.MultiTenancy
{
    public static class GeevTenantManagerExtensions
    {
        public static void Create<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, TTenant tenant)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            AsyncHelper.RunSync(() => tenantManager.CreateAsync(tenant));
        }

        public static void Update<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, TTenant tenant)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            AsyncHelper.RunSync(() => tenantManager.UpdateAsync(tenant));
        }

        public static TTenant FindById<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, int id)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            return AsyncHelper.RunSync(() => tenantManager.FindByIdAsync(id));
        }

        public static TTenant GetById<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, int id)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            return AsyncHelper.RunSync(() => tenantManager.GetByIdAsync(id));
        }

        public static TTenant FindByTenancyName<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, string tenancyName)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            return AsyncHelper.RunSync(() => tenantManager.FindByTenancyNameAsync(tenancyName));
        }

        public static void Delete<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, TTenant tenant)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            AsyncHelper.RunSync(() => tenantManager.DeleteAsync(tenant));
        }

        public static string GetFeatureValueOrNull<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, int tenantId, string featureName)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            return AsyncHelper.RunSync(() => tenantManager.GetFeatureValueOrNullAsync(tenantId, featureName));
        }

        public static IReadOnlyList<NameValue> GetFeatureValues<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, int tenantId)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            return AsyncHelper.RunSync(() => tenantManager.GetFeatureValuesAsync(tenantId));
        }

        public static void SetFeatureValues<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, int tenantId, params NameValue[] values)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            AsyncHelper.RunSync(() => tenantManager.SetFeatureValuesAsync(tenantId, values));
        }

        public static void SetFeatureValue<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, int tenantId, string featureName, string value)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            AsyncHelper.RunSync(() => tenantManager.SetFeatureValueAsync(tenantId, featureName, value));
        }

        public static void SetFeatureValue<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, TTenant tenant, string featureName, string value)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            AsyncHelper.RunSync(() => tenantManager.SetFeatureValueAsync(tenant, featureName, value));
        }

        public static void ResetAllFeatures<TTenant, TUser>(this GeevTenantManager<TTenant, TUser> tenantManager, int tenantId)
            where TTenant : GeevTenant<TUser>
            where TUser : GeevUserBase
        {
            AsyncHelper.RunSync(() => tenantManager.ResetAllFeaturesAsync(tenantId));
        }

    }
}