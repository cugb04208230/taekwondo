using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Common.Data
{
    /// <summary>
    /// 连接管理器
    /// 提供线程级别的唯一Connection
    /// </summary>
    public class ConnectionManager : IDisposable
    {


        [ThreadStatic]
        private static Dictionary<string, ConnectionManager> _managers;

        private static Dictionary<string, ConnectionManager> Managers
            => _managers ?? (_managers = new Dictionary<string, ConnectionManager>());

        private IDbConnection _connection;
        private int _referenceCount;
        private readonly string _name;

        /// <summary>
        /// 获取ConnectionManager
        /// **** 注意，不可用于异步方法！！！！！！
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static ConnectionManager Get(string connectionName)
        {
            ConnectionManager mgr;
            if (Managers.ContainsKey(connectionName))
            {
                mgr = Managers[connectionName];
            }
            else
            {
                mgr = new ConnectionManager(connectionName);
                Managers.Add(connectionName, mgr);
            }

            mgr.AddRef();
            return mgr;
        }
        /// <summary>
        /// 直接获取Connection
        /// 异步方法使用
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static IDbConnection GetConn(string connectionName)
        {
            return new SqlConnection(Settings.ConnectionString);
        }

        private ConnectionManager(string connectionName)
        {
            _name = connectionName;
            _connection = new SqlConnection(Settings.ConnectionString);
        }


        public IDbConnection Connection => _connection;

        private void AddRef()
        {
            _referenceCount += 1;
        }

        private void DeRef()
        {
            _referenceCount -= 1;
            if (_referenceCount == 0)
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
                _managers?.Remove(_name);
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DeRef();
            }
        }
        #endregion

    }
}
