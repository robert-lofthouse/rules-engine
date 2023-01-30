namespace RulesEngine.Data.Interface
{
    public interface IRulesEngineRepository<TEntity, in TKey> : IRepository<TEntity,TKey>
        where TEntity : class
    {

    }
}
