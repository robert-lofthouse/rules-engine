using System.ComponentModel;

namespace Domain.RulesEngine.Enums
{
	public enum CacheKey
	{
		[Description("CountryConfig")] CountryConfig,
		[Description("IBanData")] IBanData,
		[Description("CurrenctReleaseVersion")] CurrentReleaseVersion,
        [Description("RuleSets")] RuleSets
	}
}
