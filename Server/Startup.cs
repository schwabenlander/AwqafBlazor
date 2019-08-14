using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.Linq;
using AutoMapper;
using AwqafBlazor.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AwqafBlazor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });

            services.AddMvc().AddNewtonsoftJson();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddDbContextPool<AwqafDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AwqafDB"));
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            var mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddScoped<IFiscalYearRepository, SqlFiscalYearRepository>();
            services.AddScoped<IAccountRepository, SqlAccountRepository>();
            services.AddScoped<IAccountLedgerRepository, SqlAccountLedgerRepository>();
            services.AddScoped<IVoucherRepository, SqlVoucherRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }

            app.UseStaticFiles();
            app.UseClientSideBlazorFiles<Client.Startup>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });
        }
    }
}
