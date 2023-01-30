using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace Domain.RulesEngine.Business.Modules
{
	public class DomainServicesModule : Module
	{
		private readonly string _domainService;

		public DomainServicesModule(string domainService)
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
