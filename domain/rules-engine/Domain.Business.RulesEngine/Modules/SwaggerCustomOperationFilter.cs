using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Domain.RulesEngine.Business.Modules
{
	public class SwaggerCustomOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (operation.Parameters == null)
			{
				operation.Parameters = new List<OpenApiParameter>();
			}
			operation.Parameters.Add(new OpenApiParameter
			{
				Name = "Authorization",
				In = ParameterLocation.Header,
				Required = false // set to false if this is optional
			});
			operation.Parameters.Add(new OpenApiParameter
			{
				Name = "systemId",
				In = ParameterLocation.Header,
				Required = false // set to false if this is optional
			});
			// used by multiple apis
			operation.Parameters.Add(new OpenApiParameter
			{
				Name = "Challenge",
				In = ParameterLocation.Header,
				Required = false // set to false if this is optional
			});
			// used by the entity api
			operation.Parameters.Add(new OpenApiParameter
			{
				Name = "ViewDefinition",
				In = ParameterLocation.Header,
				Required = false // set to false if this is optional
			});
			// to be implemented by newer apis
			operation.Parameters.Add(new OpenApiParameter
			{
				Name = "clientIp",
				In = ParameterLocation.Header,
				Required = false // set to false if this is optional
			});
		}
	}
}
