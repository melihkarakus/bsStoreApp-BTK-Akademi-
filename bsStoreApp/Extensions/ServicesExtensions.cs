﻿using bsStoreApp.Repositories.Contracts;
using bsStoreApp.Repositories.EFCore;
using bsStoreApp.Services;
using bsStoreApp.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace bsStoreApp.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection Services, IConfiguration Configuration)
        {
            //Appsettings.json tanımlanan context bağlantısını burada geçilmesi gereken kodlamadır.
            Services.AddDbContext<RepositoryContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("sqlConnection")));
        }

        public static void ConfigureRepositoryManager(this IServiceCollection Repository)
        {
            Repository.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void ConfigureServiceManager(this IServiceCollection Services)
        {
            Services.AddScoped<IServiceManager, ServiceManager>();
        }
    }
}
