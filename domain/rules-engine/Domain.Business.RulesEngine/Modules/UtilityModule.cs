using Autofac;
using Domain.RulesEngine.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Domain.RulesEngine.Business.Modules
{
	public class BapsUtilityModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
			builder.RegisterType<MemoryCache>().As<IMemoryCache>().SingleInstance();
            builder.RegisterType<LocalSettings>().As<ILocalSettings>();
            builder.RegisterType<CacherService>().As<ICacherService>();
            base.Load(builder);
		}
	}
}
