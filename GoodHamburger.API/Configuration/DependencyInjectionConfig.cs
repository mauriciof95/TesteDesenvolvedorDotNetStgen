using GoodHamburger.Application.OrderContext;
using GoodHamburger.Application.ProductContext;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.UnitOfWork;
using GoodHamburger.Infrastructure.Repositories;

namespace GoodHamburger.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            //services
            services.AddScoped<ProductService>();
            services.AddScoped<OrderService>();
        }
    }
}
