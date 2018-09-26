using Common.Di;
using Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Taekwondo.Data;

namespace Taekwondo.WebApi
{
	/// <summary>
	/// 
	/// </summary>
    public class Startup
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configuration"></param>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		/// <summary>
		/// 
		/// </summary>
		public IConfiguration Configuration { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.UseConfig(Configuration).UseMvc().UseSwagger(Configuration, "Taekwondo.WebApi.xml", "Taekwondo.Data.xml")
				.UseRedis(Configuration).UseDi().AddDbContext<DataBaseContext>(ServiceLifetime.Transient);
			services.Configure<FormOptions>(options =>
			{
				options.ValueLengthLimit = int.MaxValue;
				options.MultipartBodyLengthLimit = int.MaxValue;
				options.MultipartHeadersLengthLimit = int.MaxValue;
			});
			ObjectContainer.Instance.Collection = services;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <param name="loggerFactory"></param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
				        app.UseMiddlewareExceptionHandler().UseLog(env, loggerFactory).AppUseSwagger().UseStaticFiles();
						ObjectContainer.Instance.AppConfiguration = Configuration;
		}
	}
}
