
using System;
using System.IO;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;
using HierarchicalDirectory.Infrastructure;
using HierarchicalDirectory.Domain;
using HierarchicalDirectory.Application;

namespace HierarchicalDirectory.Api
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false)
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config)
				.CreateLogger();

			var services = new ServiceCollection();
			services.AddDbContext<DirectoryDbContext>(options =>
				options.UseSqlite(config.GetConnectionString("DefaultConnection")));
			services.AddScoped<IRepository<Category>, CategoryRepository>();
			services.AddScoped<ICategoryService, CategoryService>();

			var provider = services.BuildServiceProvider();

			var url = config["EmbedIO:Url"] ?? "http://localhost:9696";
			var categoryService = provider.GetService<ICategoryService>();
			using var server = new WebServer(url)
				.WithWebApi("/api", m => m.WithController(() => new CategoriesController(categoryService)));

			Log.Information($"Server started at {url}");
			await server.RunAsync();
		}
	}
}
