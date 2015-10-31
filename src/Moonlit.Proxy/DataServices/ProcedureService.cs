using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Castle.DynamicProxy;
using Moonlit.Proxy.Properties;

namespace Moonlit.Proxy.DataServices
{
    public class ProcedureService
    {
        private readonly string _connectionString;
        private readonly DbProviderFactory _providerFactory;
        Dictionary<Type, object> _type2proxy = new Dictionary<Type, object>();
        Castle.DynamicProxy.ProxyGenerator _generator = new Castle.DynamicProxy.ProxyGenerator();

        public ProcedureService(string connectionString, DbProviderFactory providerFactory)
        {
            _connectionString = connectionString;
            _providerFactory = providerFactory;
        }

        public ProcedureService(string connectionStringName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
            _connectionString = connectionString.ConnectionString;
            _providerFactory = DbProviderFactories.GetFactory(connectionString.ProviderName);
        }

        public IT Create<IT>()
        {
            return (IT)Create(typeof(IT));
        }
        public object Create(Type type)
        {
            if (!_type2proxy.ContainsKey(type))
            {
                object proxy = _generator.CreateClassProxy(typeof(object), new Type[] { type }, new ProcedureInterceptor(_providerFactory));
                _type2proxy.Add(type, proxy);
            }
            return _type2proxy[type];
        }

        class ProcedureInterceptor : IInterceptor
        {
            private readonly DbProviderFactory _providerFactory;

            public ProcedureInterceptor(DbProviderFactory providerFactory)
            {
                _providerFactory = providerFactory;
            }

            #region IInterceptor 成员

            public void Intercept(IInvocation invocation)
            {
                var conn = _providerFactory.CreateConnection();
                DbCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = invocation.Method.Name;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var arg in invocation.Method.GetParameters())
                {
                    var dbParameter = _providerFactory.CreateParameter();
                    dbParameter.ParameterName = arg.Name;
                    dbParameter.Value = invocation.Arguments[0];
                    cmd.Parameters.Add(dbParameter);
                }
                if (invocation.Method.ReturnType == typeof(int) || invocation.Method.ReturnType == typeof(void))
                    invocation.ReturnValue = cmd.ExecuteNonQuery();
                if (invocation.Method.ReturnType == typeof(DataTable))
                {
                    var adapter = _providerFactory.CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    invocation.ReturnValue = dt;
                }
                throw new NotSupportedException(string.Format(Resources.SPNotSupportReturnType, invocation.Method.ReturnType));
            }

            #endregion
        }
    }
}
