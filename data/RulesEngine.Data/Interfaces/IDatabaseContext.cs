using System.Data.Common;

namespace RulesEngine.Data.Interface
{
    public interface IDatabaseContext<out TDbConnection> where TDbConnection : DbConnection 
    {
        TDbConnection Connection { get; }
        string DbName { get; }

    }
}
