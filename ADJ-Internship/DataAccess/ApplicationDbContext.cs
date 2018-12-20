using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ADJ.DataAccess
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
				: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			ConfigureEntities(builder);
		}

		private void ConfigureEntities(ModelBuilder builder)
		{
			var types = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetTypes();
			var maps = (from t in types
									where t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
												&& !t.GetTypeInfo().IsAbstract
												&& !t.GetTypeInfo().IsInterface
									select Activator.CreateInstance(t)).ToArray();

			foreach (var map in maps)
			{
				//var methodInfo = typeof(ModelBuilder).GetMethod(nameof(builder.ApplyConfiguration), new []{ typeof(IEntityTypeConfiguration<>) });
				var methodInfo = typeof(ModelBuilder).GetMethods().First(x => x.Name == nameof(builder.ApplyConfiguration));
				methodInfo = methodInfo.MakeGenericMethod(map.GetType().GetInterfaces().First().GenericTypeArguments);
				methodInfo.Invoke(builder, new[] { map });
			}
		}
	}

	// For Migrations
	public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			var config = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
			var configuration = config.Build();

			builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
			return new ApplicationDbContext(builder.Options);
		}
	}
}
