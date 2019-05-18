using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace FoodAndStyleOrderPlanning
{
    public class Startup
    {
        private List<string> AllowedUserEmails;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContextPool<FoodAndStyleOrderPlanningDbContext>( o => { o.UseSqlServer(Configuration.GetConnectionString("FoodAndStyleOrderPlanningDb")); } );

            services.AddScoped<IData<Restaurant>, SqlRestaurantData>();
            services.AddScoped<IData<Supplier>, SqlSupplierData>();
            services.AddScoped<IData<Product>, SqlProductData>();
            services.AddScoped<IData<Recipe>, SqlRecipeData>();
            services.AddScoped<IData<Ingredient>, SqlIngredientData>();
            services.AddScoped<IData<Order>, SqlOrderData>();
            services.AddScoped<IData<OrderRecipeItem>, SqlOrderRecipeItemData>();
            services.AddScoped<IData<User>, SqlUserData>();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {

                if (AllowedUserEmails == null)
                {
                    using (var serviceScope = app.ApplicationServices.CreateScope())
                    {
                        var services = serviceScope.ServiceProvider;
                        var data = services.GetService<IData<User>>();
                        AllowedUserEmails = data.GetByName(null).Select(u => u.Email).ToList();
                        AllowedUserEmails.Add("giorgos.ilios@gmail.com");
                        AllowedUserEmails.Add("kkatsimigas@yahoo.gr");
                    }
                }

                if (context.Request.Path.ToString() != "/AccessDenied")
                {
                    string userPrincipalName = context.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"];
                    if (!string.IsNullOrEmpty(userPrincipalName) && !AllowedUserEmails.Contains(userPrincipalName.ToLower()))
                        context.Response.Redirect("/AccessDenied");
                }
                    
                await next.Invoke();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }

    public class DBHelper
    {
        private readonly IConfiguration config;
        private IData<User> data;

        public DBHelper(IConfiguration config, IData<User> data)
        {
            this.config = config;
            this.data = data;
        }

        public IEnumerable<string> GetUsers()
        {
            List<string> users = data.GetByName(null).Select(u => u.Email).ToList();
            users.Add("giorgos.ilios@gmail.com");
            users.Add("kkatsimigas@yahoo.gr");
            return users;
        }
    }

}
