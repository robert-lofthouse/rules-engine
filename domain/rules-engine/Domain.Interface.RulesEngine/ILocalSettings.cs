using System.Collections.Generic;

namespace Domain.RulesEngine.Interface

{
    public interface ILocalSettings
    {
        Dictionary<string,string> ConnectionStrings { get; set; }
        int CommandTimeout { get; }
        int CacheItemExpiry { get; }        

    }
}