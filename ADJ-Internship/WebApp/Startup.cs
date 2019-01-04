using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADJ.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ADJ.WebApp.Identity;
using ADJ.WebApp.Infrastructure.Middlewares;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ADJ.WebApp
{
  public class Startup
  {
    private IServiceProvider _serviceProvider;

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      // Autofac Container
      var builder = new ContainerBuilder();

      // DbContext
      DbContextConfig.Register(services, Configuration);

      // Autofac
      AutofacConfig.Register(builder);

      // AutoMapper
      AutoMapperConfig.Register(builder);

      // Identity
      services.AddDbContext<AppIdentityDbContext>(options =>
          options.UseSqlServer(
              Configuration.GetConnectionString("DefaultConnection")));
      services.AddDefaultIdentity<IdentityUser>()
          .AddEntityFrameworkStores<AppIdentityDbContext>();

      // MVC
      services.AddMemoryCache();
      services.AddDistributedMemoryCache();
      services.AddSession();
      services.Configure<CookiePolicyOptions>(options =>
      {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });

      services.AddMvc(options =>
          {
            //options.Filters.Add(typeof(GlobalExceptionFilter));
            //options.ModelBinderProviders.Insert(0, new DateTimeUtcModelBinderProvider());
          })
          .AddJsonOptions(x =>
          {
            x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            x.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
          }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      // Build Autofac Service Provider
      builder.Populate(services);
      var container = builder.Build();
      _serviceProvider = new AutofacServiceProvider(container);
      return _serviceProvider;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      // Session
      app.UseSession();

      // Authentication
      app.UseAuthentication();

      // AppContext
      app.UseMiddleware<ApplicationContextPrincipalBuilderMiddleware>();

      // MVC
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseCookiePolicy();
      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
        routes.MapRoute(
          name: "ProgressCheck",
          template: "{controller=ProgressCheck}/{action=Index}/{pageIndex?}"
          );
        routes.MapRoute(
         name: "Manifest",
         template: "{controller=Manifest}/{action=Index}/{pageIndex?}"
         );
      });
    }
  }
}
