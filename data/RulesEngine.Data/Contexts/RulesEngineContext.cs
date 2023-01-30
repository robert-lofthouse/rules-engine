using System;
using System.Data.Common;
using Domain.RulesEngine.Interface;
using RulesEngine.Data.Interface;
using Microsoft.Extensions.Logging;

namespace RulesEngine.Data.Contexts
{
   
    public class RulesEngineContext<TDbConnection> : IDisposable, IDatabaseContext<TDbConnection> where TDbConnection : DbConnection  
    {
        private readonly ILogger<RulesEngineContext<TDbConnection>> _logger;
        private readonly string _connectionString;
        private TDbConnection _connection;

        public TDbConnection Connection
        {
            get
            {
                if (_connection != null) return _connection;
               
                _connection = GetConnection();

                return _connection;
            }
        }
        public string DbName => "RulesEngine";

        public RulesEngineContext(ILocalSettings settings, ILogger<RulesEngineContext<TDbConnection>> logger)
        {
            _logger = logger;
            _connectionString = settings.ConnectionStrings[DbName];
            _logger.LogTrace($"{DbName} Connection String : {_connectionString}");

        }

        private TDbConnection GetConnection()
        {

            var connection = Activator.CreateInstance<TDbConnection>();
            connection.ConnectionString = _connectionString;
            
            connection.Open();
            _logger.LogTrace($"{DbName} Connection created");
            return connection;
        }

        public void Dispose()
        {
            if (_connection == null) return;

            _connection.Close();

            _connection.Dispose();

            _connection = null;

            _logger.LogTrace($"{DbName} Connection destroyed");
        }
    }
}
