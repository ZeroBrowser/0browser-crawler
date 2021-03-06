using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ZeroBrowser.Crawler.Api.HostedService;
using ZeroBrowser.Crawler.Common.Channels;
using ZeroBrowser.Crawler.Common.Interfaces;
using ZeroBrowser.Crawler.Common.Models;
using ZeroBrowser.Crawler.Common.Queues;
using ZeroBrowser.Crawler.Core;
using ZeroBrowser.Crawler.Frontier;
using ZeroBrowser.Crawler.Puppeteer;

namespace ZeroBrowser.Crawler.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IWebHostEnvironment env)
        {
            _hostingEnvironment = env;

            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.            
            services.AddDbContext<CrawlerDBContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton, ServiceLifetime.Singleton);

            services.AddSingleton<ICrawler, Core.Crawler>();
            services.AddSingleton<IHeadlessBrowserService, HeadlessBrowserService>();
            services.AddSingleton<IBackgroundUrlQueue, BackgroundUrlQueue>();
            services.AddSingleton<FrontierState>();
            services.AddSingleton<IRepositoryQueue, RepositoryQueue>();
            services.AddSingleton<IRepository, SQLiteRepository>();
            services.AddSingleton<IManageHeadlessBrowser, ManageHeadlessBrowser>();
            services.AddSingleton<IUrlChannel, UrlChannel>();
            services.AddSingleton<IFrontier, Frontier.Frontier>();
            
            services.AddHostedService<FrontierUrlQueuedHostedService>();
            services.AddHostedService<ParallelCrawlerHostedService>();
            services.AddHostedService<RepositoryQueuedHostedService>();
            
            services.Configure<CrawlerOptions>(Configuration.GetSection(CrawlerOptions.Section));


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ZeroBrowser.Crawler.Api", Version = "v1" });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZeroBrowser.Crawler.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();


                });
            });
        }
    }
}
