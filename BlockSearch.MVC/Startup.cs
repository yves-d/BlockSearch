using BlockSearch.Application;
using BlockSearch.Application.CryptoService;
using BlockSearch.Application.ExternalClients;
using BlockSearch.Infrastructure.Logger;
using BlockSearch.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlockSearch.MVC
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            services.AddTransient(typeof(ICryptoServiceFactory), typeof(CryptoServiceFactory));
            services.AddTransient(typeof(IBlockSearchService), typeof(BlockSearchService));
            services.AddTransient(typeof(IEthereumClient), typeof(NethereumClient));

            services.AddScoped<EthereumService>()
                .AddScoped<ICryptoService, EthereumService>(s => s.GetService<EthereumService>());

            // grab connection options from appsettings
            services.Configure<NethereumClientOptions>(Configuration.GetSection(nameof(NethereumClient)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
