using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using docker_sandbox_dev_api.Contract;
using docker_sandbox_dev_api.Services;
using docker_sandbox_dev_api.Dal;

namespace docker_sandbox_dev_api
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
            services.AddControllers();
            services.AddSingleton<IConfiguration>(this.Configuration);
            services.AddTransient<IDockerService, DockerService>();

            services.AddDbContext<SandboxDbContext>(options =>
            {
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = "http://localhost:5000";
                        options.RequireHttpsMetadata = false;
                        options.ApiName = "api1";
                    });

            //services.AddAuthorization(options =>
            //{
            //    IConfigurationSection section = this.Configuration.GetSection("Authorization");
            //    Dictionary<string, string[]> policies = new Dictionary<string, string[]>();
            //    section.Bind(policies);
            //    foreach (KeyValuePair<string, string[]> policy in policies)
            //    {
            //        options.AddPolicy(policy.Key, p =>
            //        {
            //            p.RequireRole(policy.Value);
            //        });
            //    }
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
