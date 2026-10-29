using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,string configuration)
        {
            // Register infrastructure services here, e.g., database context, repositories, etc.
            // Example:
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration ?? throw new InvalidOperationException("Connection string 'ShopingContext' not found."), b => b.MigrationsAssembly("Infrastructure")));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
