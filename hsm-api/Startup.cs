using hsm_api.ConfigurationOptions.DimensionSettings;
using hsm_api.ConfigurationOptions.TimerSettings;
using hsm_api.Domain.DimensionGenerators;
using hsm_api.Domain.FinishProduction;
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
            services.Configure<FinishProductionTimerSettings>(Configuration.GetSection(nameof(FinishProductionTimerSettings)));
            services.Configure<ThicknessGeneratorSettings>(Configuration.GetSection(nameof(ThicknessGeneratorSettings)));
            services.Configure<WidthGeneratorSettings>(Configuration.GetSection(nameof(WidthGeneratorSettings)));
            services.Configure<WeightGeneratorSettings>(Configuration.GetSection(nameof(WeightGeneratorSettings)));
            services.AddDbContext<WebhookContext>(opt => opt.UseInMemoryDatabase(nameof(Webhook)));
            services.AddDbContext<MessageContext>(opt => opt.UseInMemoryDatabase(nameof(Message)));
            services.AddScoped<PseudoRandomDimensionGenerator<ThicknessGeneratorSettings>>();
            services.AddScoped<PseudoRandomDimensionGenerator<WidthGeneratorSettings>>();
            services.AddScoped<PseudoRandomDimensionGenerator<WeightGeneratorSettings>>();
            services.AddSingleton<IDynamicIntervalTimer<StartProductionTimerSettings>, DynamicIntervalTimer<StartProductionTimerSettings>>();
            services.AddSingleton<IDynamicIntervalTimer<FinishProductionTimerSettings>, DynamicIntervalTimer<FinishProductionTimerSettings>>();
            services.AddHttpClient<StartProductionHttpMessageSender>();
            services.AddHttpClient<FinishProductionHttpMessageSender>();
            services.AddSingleton<StartProductionHttpMessageSender>();
            services.AddSingleton<FinishProductionHttpMessageSender>();
            services.AddSingleton<StartProductionService>();
            services.AddSingleton<FinishProductionService>();
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
            app.ApplicationServices.GetService<IDynamicIntervalTimer<FinishProductionTimerSettings>>();
        }
    }
}
