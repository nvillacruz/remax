using Dna;
using Dna.AspNet;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Text;

namespace Remax.Web.Server
{
    /// <summary>
    /// The startup class that handles constructing the ASP.Net server services
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Main entry point for start of web server
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Add proper cookie request to foloow GDPR
            services.Configure<CookiePolicyOptions>(options =>
            {
                //This lambda determins whether user consent for 
                //non -essential cookies is needed for a given request
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add SendGrid email sender
            services.AddSendGridEmailSender();

            // Add general email template sender
            services.AddEmailTemplateSender();

            // Add ApplicationDbContext to DI
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Framework.Construction.Configuration.GetConnectionString("DefaultConnection")));

            // AddIdentity adds cookie based authentication
            // Adds scoped classes for things like UserManager, SignInManager, PasswordHashers etc..
            // NOTE: Automatically adds the validated user from a cookie to the HttpContext.User
            // https://github.com/aspnet/Identity/blob/85f8a49aef68bf9763cd9854ce1dd4a26a7c5d3c/src/Identity/IdentityServiceCollectionExtensions.cs
            services.AddIdentity<ApplicationUser, IdentityRole>()

                // Adds UserStore and RoleStore from this context
                // That are consumed by the UserManager and RoleManager
                // https://github.com/aspnet/Identity/blob/dev/src/EF/IdentityEntityFrameworkBuilderExtensions.cs
                .AddEntityFrameworkStores<ApplicationDbContext>()

                // Adds a provider that generates unique keys and hashes for things like
                // forgot password links, phone number verification codes etc...
                .AddDefaultTokenProviders();

            // Add JWT Authentication for Api clients
            services.AddAuthentication().
                AddJwtBearer(options =>
                {
                    // Set validation parameters
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Validate issuer
                        ValidateIssuer = true,
                        // Validate audience
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        // Validate signature
                        ValidateIssuerSigningKey = true,
                        // Set issuer
                        ValidIssuer = Framework.Construction.Configuration["Jwt:Issuer"],
                        // Set audience
                        ValidAudience = Framework.Construction.Configuration["Jwt:Audience"],
                        // Set signing key
                        IssuerSigningKey = new SymmetricSecurityKey(
                            // Get our secret key from configuration
                            Encoding.UTF8.GetBytes(Framework.Construction.Configuration["Jwt:SecretKey"])),
                    };
                });

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = Framework.Construction.Configuration["GoogleAuth:ClientId"];
                options.ClientSecret = Framework.Construction.Configuration["GoogleAuth:ClientSecret"];
            });

            // Change password policy
            services.Configure<IdentityOptions>(options =>
            {
                //Password Settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                //User Settings
                options.User.RequireUniqueEmail = true;

               // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

            });

            // Alter application cookie info
            services.ConfigureApplicationCookie(options =>
            {
                // Redirect to /login 
                options.LoginPath = "/login";

                // Change cookie timeout to expire in 15 seconds
                options.ExpireTimeSpan = TimeSpan.FromSeconds(1500);
            });

            //Enable CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            // Use MVC style website
            services.AddMvc(options =>
            {
                //options.InputFormatters.Add(new XmlSerializerInputFormatter());
                //options.OutputFormatters.Add(new DefaultContractResolver());
            })

            //Add Fluent validation
            .AddFluentValidation(x=> x.RegisterValidatorsFromAssemblyContaining<Startup>())

           // State we are a minimum compatibility of 2.1 onwards
           .SetCompatibilityVersion(CompatibilityVersion.Version_2_1); 



            services.AddMvc().AddJsonOptions(opt =>
            {
                if (opt.SerializerSettings.ContractResolver != null)
                {
                    var resolver = opt.SerializerSettings.ContractResolver as DefaultContractResolver;
                    resolver.NamingStrategy = null;
                }
            });

            

            //services.AddSignalR();

            ////Multipart
            //services.Configure<FormOptions>(x =>
            //{
            //    x.MultipartBodyLengthLimit = 60000000;
            //    x.MemoryBufferThreshold = Int32.MaxValue;
            //});

         
            ////Enforcing SSL globally
            //services.Configure<MvcOptions>(options =>
            //{
            //    //options.SslPort = 3000;
            //    options.Filters.Add(new RequireHttpsAttribute());
            //});

            // Register the Swagger generator, defining one or more Swagger documents  
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Y Bizz Reservation System API",
                    Version = "v1",
                    Description = "Y Bizz Reservation System API",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Nelson Villacruz", Email = "nelson.villacruz@ygroup.ph", Url = "vccoffees.com" }
                });

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Remax.Web.Server.xml"));
            });


            //services.Configure<FormOptions>(x =>
            //{
            //    x.ValueLengthLimit = int.MaxValue;
            //    x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            //});

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="serviceProvider"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            // Store instance of the DI service provider so our application can access it anywhere
            app.UseDnaFramework();

            // Setup Identity
            app.UseAuthentication();

            //Redirect HTTP calls to HTTPS
            //var options = new RewriteOptions()
            //.AddRedirectToHttps();
            //app.UseRewriter(options);


            // If in development...
            if (env.IsDevelopment())
            {
                // Show any exceptions in browser when they crash
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();

            }
            // Otherwise...
            else
            // Just show generic error page
            {

                app.UseExceptionHandler("/Home/Error");

                // In production, tell the browsers (via the HSTS header)
                // to only try and access our site via HTTPS, not HTTP
                app.UseHsts();
            }

            // Redirect all calls from HTTP to HTTPS
            //app.UseHttpsRedirection();

            // Force non-essential cookies to only store
            // if the user has consented
            app.UseCookiePolicy();

            // Serve static files
            app.UseStaticFiles();

            //Use CORS set up above
            app.UseCors("AllowAllOrigins");

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<MessageHub>("/message");
            //});

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "My API V1");

            });

            // Setup MVC routes
            app.UseMvc(routes =>
            {
                // Default route of /controller/action/info
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{moreInfo?}");

            });

            
        }
    }
}
