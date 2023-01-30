using System.Diagnostics;
using Microsoft.Extensions.Logging;
using RepoDb;
using RepoDb.Interfaces;

namespace RulesEngine.Data
{
    public class TraceService : ITrace 
    {
        private readonly ILogger<TraceService> _logger;

        public TraceService(ILogger<TraceService> logger)
        {
            _logger = logger;
        }

        public void BeforeAverage(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterAverage(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeAverageAll(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement, null);
        }

        public void AfterAverageAll(TraceLog log)
        {
            _logger.LogTrace(new EventId(), log.Result.ToString(), null);
        }

        public void BeforeBatchQuery(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterBatchQuery(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeCount(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterCount(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeCountAll(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterCountAll(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeDelete(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterDelete(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeDeleteAll(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterDeleteAll(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeExists(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterExists(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeExecuteNonQuery(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterExecuteNonQuery(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeExecuteQuery(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterExecuteQuery(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeExecuteReader(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterExecuteReader(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeExecuteScalar(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterExecuteScalar(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeInsert(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
            Debug.Write( log.Statement);
        }

        public void AfterInsert(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
            Debug.Write($"{log.ExecutionTime}: {log.Result}");
        }

        public void BeforeInsertAll(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterInsertAll(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeMax(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterMax(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeMaxAll(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterMaxAll(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeMerge(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterMerge(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeMergeAll(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterMergeAll(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeMin(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterMin(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeMinAll(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterMinAll(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeQuery(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterQuery(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeQueryAll(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterQueryAll(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeQueryMultiple(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterQueryMultiple(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeSum(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterSum(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeSumAll(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterSumAll(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeTruncate(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterTruncate(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeUpdate(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterUpdate(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }

        public void BeforeUpdateAll(CancellableTraceLog log)
        {
            _logger.LogTrace(log.Statement);
        }

        public void AfterUpdateAll(TraceLog log)
        {
            _logger.LogTrace(log.Result.ToString());
        }
    }
}
