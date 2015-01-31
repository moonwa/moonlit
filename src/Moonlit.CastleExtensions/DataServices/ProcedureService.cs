using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Castle.DynamicProxy;
using Moonlit.CastleExtensions.Properties;

namespace Moonlit.Data
{
    public class ProcedureService 
    {
        Dictionary<Type, object> _type2proxy = new Dictionary<Type, object>();
        Castle.DynamicProxy.ProxyGenerator _generator = new Castle.DynamicProxy.ProxyGenerator();
        private IDatabase _database;

        public ProcedureService(IDatabase database)
        {
            _database = database;
        }
        public IT Create<IT>()
        {
            return (IT)Create(typeof(IT));
        }
        public object Create(Type type)
        {
            if (!_type2proxy.ContainsKey(type))
            {
                object proxy = _generator.CreateClassProxy(typeof(object), new Type[] { type }, new ProcedureInterceptor(_database));
                _type2proxy.Add(type, proxy);
            }
            return _type2proxy[type];
        }

        class ProcedureInterceptor : IInterceptor
        {
            IDatabase _database;
            public ProcedureInterceptor(IDatabase database)
            {
                _database = database;
            }

            #region IInterceptor 成员

            public void Intercept(IInvocation invocation)
            {
                var conn = _database.Connection;
                DbCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = invocation.Method.Name;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var arg in invocation.Method.GetParameters())
                {
                    var param = cmd.Parameters.Add(_database.CreateParameter(arg.Name, invocation.Arguments[0]));
                }
                if (invocation.Method.ReturnType == typeof(int) || invocation.Method.ReturnType == typeof(void))
                    invocation.ReturnValue = cmd.ExecuteNonQuery();
                if (invocation.Method.ReturnType == typeof(DataTable))
                {
                    var adapter = _database.CreateDataAdapter();
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
