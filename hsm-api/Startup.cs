using hsm_api.ConfigurationOptions;
using hsm_api.Domain.StartProduction;
using hsm_api.Infrastructure;
using hsm_api.Models;
using hsm_api.Models.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace hsm_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<StartProductionTimerSettings>(Configuration.GetSection(nameof(StartProductionTimerSettings)));
            services.AddDbContext<WebhookContext>(opt => opt.UseInMemoryDatabase(nameof(Webhook)));
            services.AddDbContext<MessageContext>(opt => opt.UseInMemoryDatabase(nameof(Message)));
            services.AddSingleton<IDynamicIntervalTimer<StartProductionTimerSettings>, DynamicIntervalTimer<StartProductionTimerSettings>>();
            services.AddHttpClient<StartProductionHttpMessageSender>();
            services.AddSingleton<StartProductionHttpMessageSender>();
            services.AddSingleton<StartProductionService>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HSM API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HSM API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            InitTimers(app);
        }

        private static void InitTimers(IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IDynamicIntervalTimer<StartProductionTimerSettings>>();
        }
    }
}
