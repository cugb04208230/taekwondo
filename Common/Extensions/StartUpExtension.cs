using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Common.Di;
using Common.Filters;
using Common.Log;
using Common.MiddleWare;
//using Hangfire;
//using ICanPay.Alipay;
//using ICanPay.Core;
//using ICanPay.Unionpay;
//using ICanPay.Wechatpay;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;

namespace Common.Extensions
{
	public static class StartUpExtension
	{
		#region Services/ConfigureServices

		public static IServiceCollection UseConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddOptions();
			return services;
		}

		public static IServiceCollection UseMvc(this IServiceCollection services)
		{
			services.AddMvc().AddJsonOptions(options =>
			{
				//忽略循环引用
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				//不使用驼峰样式的key
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				//设置时间格式
				options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
				options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			}
			);
			return services;
		}

		public static IServiceCollection UseSwagger(this IServiceCollection services, IConfiguration configuration, params string[] xmls)
		{// Register the Swagger generator, defining one or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info
				{
					Title = "Taekwondo-Api",
					Description = Settings.ApiHost,
					TermsOfService = "None"
				});
				//Set the comments path for the swagger json and ui.
				var basePath = PlatformServices.Default.Application.ApplicationBasePath;
				foreach (var xml in xmls)
				{
					var xmlPath = Path.Combine(basePath, xml);
					c.IncludeXmlComments(xmlPath);
				}
				c.OperationFilter<ApplySummariesOperationFilter>();
				//                c.IncludeXmlComments(Path.Combine(basePath, "CSG.BOSHPC.Dto.xml"));
			});
			return services;
		}

		public static IServiceCollection UseRedis(this IServiceCollection services, IConfiguration configuration)
		{
			var redisConfig = configuration.GetSection("RedisConfig");
			services.AddDistributedRedisCache(option =>
			{
				option.Configuration = redisConfig["Connection"];
				option.InstanceName = redisConfig["InstanceName"];
			});
			return services;
		}

		public static IServiceCollection UseDi(this IServiceCollection services)
		{

			var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(o => o.FullName.StartsWith("Taekwondo")).ToArray();
			foreach (var assembly in assemblies)
			{
				try
				{
					foreach (var type in assembly.GetTypes())
					{
						if ( type.Name.EndsWith("Service") || type.Name.EndsWith("Repository"))
						{
							var interfaces = type.GetInterfaces();
							if (interfaces.Any())
							{
								foreach (var interfaceType in interfaces)
								{
									services.AddTransient(interfaceType, type);
								}
							}
							else
							{
								services.AddTransient(type);
							}
						}
					}
				}
				catch (Exception error)
				{
					System.Diagnostics.Trace.WriteLine(error);
				}
			}
			return services;
		}

//		public static IServiceCollection UseHangfire(this IServiceCollection services, IConfiguration configuration)
//		{
//			var str = configuration.GetSection("ConnectionStrings")["JobMsSqlContext"];
//			services.AddHangfire(r =>
//			{
//				r.UseSqlServerStorage(str);
//				r.UseDefaultActivator();
//			});
//
//
//			//            HangfireRegistAssembly(configuration); 
//			return services;
//		}

//		public static IServiceCollection UserICanPay(this IServiceCollection services, IConfiguration configuration)
//		{
//			services.AddICanPay(a =>
//			{
//				var gateways = new Gateways();
//
//				// 设置商户数据
//				var alipayMerchant = new ICanPay.Alipay.Merchant
//				{
//					AppId = "",
//					NotifyUrl = "",
//					ReturnUrl = "",
//					AlipayPublicKey = "",
//					Privatekey = ""
//				};
//
//				var wechatpayMerchant = new ICanPay.Wechatpay.Merchant
//				{
//					AppId = "wx2428e34e0e7dc6ef",
//					MchId = "1233410002",
//					Key = "e10adc3849ba56abbe56e056f20f883e",
//					AppSecret = "51c56b886b5be869567dd389b3e5d1d6",
//					SslCertPath = "Certs/apiclient_cert.p12",
//					SslCertPassword = "1233410002",
//					NotifyUrl = "http://localhost:61337/Notify"
//				};
//
//				var unionpayMerchant = new ICanPay.Unionpay.Merchant
//				{
//					AppId = "777290058110048",
//					CertPwd = "000000",
//					CertPath = "Certs/acp_test_sign.pfx",
//					NotifyUrl = "http://localhost:61337/Notify",
//					FrontUrl = "http://localhost:61337/Notify"
//				};
//
//				gateways.Add(new AlipayGateway(alipayMerchant));
//				gateways.Add(new WechatpayGateway(wechatpayMerchant));
//				gateways.Add(new UnionpayGateway(unionpayMerchant));
//
//				return gateways;
//			});
//			return services;
//		}

		#endregion

		#region Configure/ApplicationBuilder

		public static IApplicationBuilder UseMiddlewareExceptionHandler(this IApplicationBuilder app)
		{
			app.UseMiddleware(typeof(ExceptionHandlerMiddleWare)).UseMvc(routes =>
			{
				routes.MapRoute("default", "{controller=Auth}/{action=Account}/{id?}");
				routes.MapRoute("defaultApi", "api/{controller}");
			});
			return app;
		}

		public static IApplicationBuilder UseLog(this IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{

			loggerFactory.AddNLog();
			env.ConfigureNLog("log.config");
			TaskScheduler.UnobservedTaskException += (s, ev) => { LogHelper.Error(ev.Exception); };
			return app;
		}

		public static IApplicationBuilder AppUseSwagger(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.RoutePrefix = "swagger";
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});
			return app;
		}

		public static IConfiguration BuildConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, true);

			return builder.Build();
		}

//		public static IApplicationBuilder AppUseHangfire(this IApplicationBuilder app, IConfiguration configuration)
//		{
//			app.UseHangfireServer();
//			app.UseHangfireServer(new BackgroundJobServerOptions
//			{
//				Queues = new[] { "default", "apis", "jobs" }
//			});
//			var options = new DashboardOptions
//			{
//				Authorization = new[] { new CustomDashboardAuthorizationFilter() }
//			};
//			app.UseHangfireDashboard("/hangfire", options);
//			HangfireRegistAssembly(configuration);
//			return app;
//		}
//
//		public static void HangfireRegistAssembly(IConfiguration configuration)
//		{
//			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
//				.Where(o => o.FullName.StartsWith("CSG.BOSHPC.Application")).ToArray();
//			foreach (var assembly in assemblies)
//			{
//				try
//				{
//					foreach (var type in assembly.GetTypes())
//					{
//						if (type.Name == "JobService")
//						{
//							foreach (var method in type.GetMethods())
//							{
//								var attr =
//									Attribute.GetCustomAttribute(method, typeof(JobServiceAttribute), false) as
//										JobServiceAttribute;
//								if (attr == null)
//								{
//									continue;
//								}
//								var cron = configuration.GetSection("JobSettings")[$"Cron{method.Name}"] ?? attr.Cron;
//								HangfireRegist($"{method.DeclaringType.Name}.{method.Name}", method, cron,
//									TimeZoneInfo.Local, "jobs");
//							}
//						}
//					}
//				}
//				catch (Exception error)
//				{
//					LogHelper.Error(error);
//				}
//			}
//		}
//
//		/// <summary>
//		/// Register RecurringJob via <see cref="MethodInfo"/>.
//		/// </summary>
//		/// <param name="recurringJobId">The identifier of the RecurringJob</param>
//		/// <param name="method">the specified method</param>
//		/// <param name="cron">Cron expressions</param>
//		/// <param name="timeZone"><see cref="TimeZoneInfo"/></param>
//		/// <param name="queue">Queue name</param>
//		public static void HangfireRegist(string recurringJobId, MethodInfo method, string cron, TimeZoneInfo timeZone, string queue)
//		{
//			if (recurringJobId == null) throw new ArgumentNullException(nameof(recurringJobId));
//			if (method == null) throw new ArgumentNullException(nameof(method));
//			if (cron == null) throw new ArgumentNullException(nameof(cron));
//			if (timeZone == null) throw new ArgumentNullException(nameof(timeZone));
//			if (queue == null) throw new ArgumentNullException(nameof(queue));
//
//			var parameters = method.GetParameters();
//
//			Expression[] args = new Expression[parameters.Length];
//
//			for (int i = 0; i < parameters.Length; i++)
//			{
//				args[i] = Expression.Default(parameters[i].ParameterType);
//			}
//
//			var x = Expression.Parameter(method.DeclaringType, "x");
//
//			var methodCall = method.IsStatic ? Expression.Call(method, args) : Expression.Call(x, method, args);
//
//			var addOrUpdate = Expression.Call(
//				typeof(RecurringJob),
//				nameof(RecurringJob.AddOrUpdate),
//				new Type[] { method.DeclaringType },
//				new Expression[]
//				{
//					Expression.Constant(recurringJobId),
//					Expression.Lambda(methodCall, x),
//					Expression.Constant(cron),
//					Expression.Constant(timeZone),
//					Expression.Constant(queue)
//				});
//
//			Expression.Lambda(addOrUpdate).Compile().DynamicInvoke();
//		}
//
//		public static IApplicationBuilder UseICanPay(this IApplicationBuilder app)
//		{
//			app.UseICanPay();
//			return app;
//		}

		#endregion
	}
}
