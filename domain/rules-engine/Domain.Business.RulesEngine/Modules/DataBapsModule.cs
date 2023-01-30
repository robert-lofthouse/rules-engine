using Autofac;
using Microsoft.Extensions.Configuration;
using RulesEngine.Data;
using RulesEngine.Data.Contexts;
using RulesEngine.Data.Interface;
using RulesEngine.Data.Repos;
using RepoDb.Interfaces;
using Module = Autofac.Module;

namespace Domain.RulesEngine.Business.Modules
{
	public class BapsDataBapsModule : Module
	{
		private readonly IConfiguration _configuration;

		public BapsDataBapsModule(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void Load(ContainerBuilder builder)
		{
			RepoDb.PostgreSqlBootstrap.Initialize();


			builder.RegisterInstance(_configuration).As<IConfiguration>().SingleInstance();
            builder.RegisterGeneric(typeof(RulesEngineContext<>)).As(typeof(IDatabaseContext<>)).InstancePerLifetimeScope();
            builder.RegisterType<RuleSetRepository>().As<IRuleSetRepository>();
            builder.RegisterType<RulesRepository>().As<IRulesRepository>();
            builder.RegisterGeneric(typeof(RulesEngineRepository<,>)).As(typeof(IRulesEngineRepository<,>));

            builder.RegisterType<TraceService>().As<ITrace>().SingleInstance();
            builder.RegisterType<CacheService>().As<ICache>().SingleInstance();

			base.Load(builder);
		}
	}
}
