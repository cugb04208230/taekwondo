using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Taekwondo.WebApi
{
	/// <summary>
	/// 
	/// </summary>
    public class Program
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
