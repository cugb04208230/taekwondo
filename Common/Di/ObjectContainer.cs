using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Di
{
	public class ObjectContainer
	{
		private static readonly object Lockobject = new object();
		private static readonly object ProviderLockobject = new object();
		private static ObjectContainer _container;
		private static IServiceProvider _provider;
		public static ObjectContainer Instance
		{
			get
			{
				lock (Lockobject)
				{
					if (_container == null)
						_container = new ObjectContainer();
				}
				return _container;
			}
		}

		public IServiceCollection Collection { set; get; }

		public IServiceProvider Provider
		{
			get
			{
				lock (ProviderLockobject)
				{
					if (_provider == null)
						_provider = Collection.BuildServiceProvider();
				}
				return _provider;
			}
		}

		public T Resolver<T>()
		{
			return (T)Provider.GetService(typeof(T));
		}

		public IConfiguration AppConfiguration { set; get; }
	}
}
