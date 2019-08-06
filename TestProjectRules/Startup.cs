using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services;
using Services.Effects;
using Services.Options;
using Swashbuckle.AspNetCore.Swagger;
using Telegram.Bot;
using TestProjectRules.Filters;

namespace TestProjectRules
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
            services.AddMvc(config => config.Filters.Add<ExceptionFilter>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Rules", Version = "v1" });
            });

            services.AddOptions();
            services.Configure<TelegramOptions>(Configuration.GetSection("Telegram"));
            services.Configure<SmtpOptions>(Configuration.GetSection("Smtp"));
            
            services.AddSingleton<EmailService>();
            services.AddSingleton<TelegramEffect>();
            services.AddSingleton<SmtpEffect>();
            services.AddSingleton<FilterService>();
            services.AddSingleton(typeof(ExpressionBuilder<>));
            services.AddSingleton(
                sp => new TelegramBotClient(sp.GetService<IOptions<TelegramOptions>>().Value.ApiToken));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rules API V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
