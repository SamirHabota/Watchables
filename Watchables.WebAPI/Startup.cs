using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eProdaja.WebAPI.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Watchables.WebAPI.Database;
using Watchables.WebAPI.Securitydb;
using Watchables.WebAPI.Services;

namespace Watchables.WebAPI
{

    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            //Controllers and filters
            services.AddControllers(x => x.Filters.Add<ErrorFilter>());

            //Swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Watchables API", Version = "v1" });

                c.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme() {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    Description = "Input your username and password to access this API",
                    In = ParameterLocation.Header,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basicAuth" }
                        }, new List<string>() }
                });
            });


            //Database
            services.AddDbContext<_160304Context>(options => options.UseSqlServer(Configuration.GetConnectionString("connectionString")));

            //AutoMapper
            services.AddAutoMapper();

            //Authentication
            services.AddAuthentication("BasicAuthentication")
               .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            //Interface overrides
            services.AddScoped<IInitializeService, InitializeService>();
            services.AddScoped<ICinemasService, CinemasService>();
            services.AddScoped<IHallsService, HallsService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IMoviesService, MoviesService>();
            services.AddScoped<IAiringDayService, AiringDayService>();
            services.AddScoped<IAiringDaysOfCinemaService, AirinDaysOfCinemaService>();
            services.AddScoped<ICinemaDayMovieService, CinemaDayMovieService>();
            services.AddScoped<IAppointmentsService, AppointmentsService>();
            services.AddScoped<IShowsService, ShowsService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ICRUDService<Model.Subscription, Model.Requests.SubscriptionSearchRequest, Model.Requests.InsertSubscriptionRequest, Model.Requests.InsertSubscriptionRequest>, SubscriptionService>();
            services.AddScoped<ICRUDService<Model.Rotation, Model.Requests.RotationSearchRequest, Model.Requests.InsertRotationRequest, Model.Requests.InsertRotationRequest>, RotationsService>();
            services.AddScoped<ICRUDService<Model.Order, Model.Requests.OrderSerachRequest, Model.Requests.InsertOrderRequest, Model.Requests.InsertOrderRequest>, OrderService>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();


            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
