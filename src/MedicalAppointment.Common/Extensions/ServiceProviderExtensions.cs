using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using MedicalAppointment.Common.Data;

namespace MedicalAppointment.Common.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void MigrateDatabase(this IServiceProvider provider)
        {
            provider.GetService<AppDbContext>().Database.Migrate();
        }

        public static void SeedData(this IServiceProvider provider)
        {
            Task.Run(async () =>
            {
                await DbInitializer.Seed(provider);
            }).Wait();
        }
    }
}
