using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;

namespace ASC.Utilities
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {
            // Chỉ thêm HttpContextAccessor, bỏ Session và Cache
            services.AddHttpContextAccessor();

            return services;
        }
    }
}