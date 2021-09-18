using BlockSearch.Application;
using BlockSearch.Application.SearcherClients;
using BlockSearch.Common.Logger;
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
            services.AddSingleton(typeof(ISearcherClientFactory), typeof(SearcherClientFactory));
            services.AddTransient(typeof(IBlockSearchService), typeof(BlockSearchService));
            services.AddTransient(typeof(ISearcherClient), typeof(EthereumSearcherClient));

            // grab httpclient options from appsettings
            services.Configure<EthereumSearcherOptions>(Configuration.GetSection(nameof(EthereumSearcherClient)));
            // initialise http client for ethereum searcher
            services.AddHttpClient<ISearcherClient, EthereumSearcherClient>();
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
