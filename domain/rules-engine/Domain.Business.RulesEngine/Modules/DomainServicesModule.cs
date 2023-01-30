using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace Domain.RulesEngine.Business.Modules
{
	public class BapsDomainServicesModule : Module
	{
		private readonly string _domainService;

		public BapsDomainServicesModule(string domainService)
		{
			_domainService = domainService;
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(Assembly.Load($"Domain.{_domainService}.Business"))
				   .Where(c => c.Name.EndsWith("Service"))
				   .AsImplementedInterfaces()
				   .InstancePerLifetimeScope();
			base.Load(builder);
		}
	}
}
