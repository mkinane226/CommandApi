using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandApi.Entity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CommandApi.Web
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
            // Manage Database Access
            var builder = new NpgsqlConnectionStringBuilder();
            builder.ConnectionString = Configuration.GetConnectionString("PostgreSqlConnection");
            builder.Database = Configuration["Database"];
            builder.Username = Configuration["UserID"];
            builder.Password = Configuration["Password"];
            services.AddDbContext<CommandContext>(opt =>
                opt.UseNpgsql(builder.ConnectionString,
                b => b.MigrationsAssembly("CommandApi.Web")));
            services.AddControllers();
            // Depency injection register of Mock/SqlCommandAPIRepo against ICommand
            services.AddScoped<ICommandAPIRepo, SqlCommandAPIRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
