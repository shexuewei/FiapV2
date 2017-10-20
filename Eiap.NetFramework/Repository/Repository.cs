
using System;
using System.Data;

namespace Eiap.NetFramework
{
    /// <summary>
    /// 仓储管理实现类
    /// </summary>
    /// <typeparam name="tEntity"></typeparam>
    /// <typeparam name="TPrimarykey"></typeparam>
    public class Repository<tEntity, TPrimarykey> : IRepository<tEntity, TPrimarykey>
        where tEntity : IEntity<TPrimarykey>
        where TPrimarykey : struct
    {
        private ISQLCommandMapping<tEntity, TPrimarykey> _CommandMapping;
        private ISQLDataQueryMapping<tEntity, TPrimarykey> _QueryMapping;
        private ISQLQuery _SQLQuery;
        private IUnitOfWork _CurrentUnitOfWork;
        private readonly IMethodManager _MethodManager;

        public Repository(ISQLCommandMapping<tEntity, TPrimarykey> CommandMapping,
            ISQLDataQueryMapping<tEntity, TPrimarykey> QueryMapping,
            ISQLQuery SQLQuery,
            IMethodManager methodManager,
            IUnitOfWork CurrentUnitOfWork)
        {
            _CommandMapping = CommandMapping;
            _QueryMapping = QueryMapping;
            _SQLQuery = SQLQuery;
            _CurrentUnitOfWork = CurrentUnitOfWork;
            _CurrentUnitOfWork.SetRepository(this);
            _MethodManager = methodManager;
        }

        public virtual tEntity Add(tEntity entity)
        {
            return _CommandMapping.InsertEntity(entity);
        }

        public virtual tEntity Update(tEntity entity)
        {
            _CommandMapping.UpdateEntity(entity);
            return entity;
        }

        public virtual void Delete(TPrimarykey primarykey)
        {
            _CommandMapping.DeleteEntity(primarykey);
        }

        public virtual ISQLDataQueryMapping<tEntity, TPrimarykey> Query()
        {
            return _QueryMapping;
        }

        public virtual TResult Query<TResult>(string cmdText, CommandType cmdType, IDataParameter[] paramters = null)
        {
            TResult result = default(TResult);
            DataSet dataset = _SQLQuery.ExcuteGetDateSet(cmdText, cmdType, paramters);
            result = dataset.DataSetToEntityList<TResult>(_MethodManager);
            return result;
        }

        public void Dispose()
        {
            _CommandMapping.Dispose();
            _QueryMapping.Dispose();
            _SQLQuery.Dispose();
        }

        public virtual ISQLCommandDataAccessConnection SQLDataAccessConnection
        {
            set 
            { 
                _CommandMapping.SQLDataAccessConnection = value; 
            }
        }

        /// <summary>
        /// 日志接口
        /// </summary>
        internal ILogger Logger { get; set; }

        public Action<string> Log {
            get {
                return _CommandMapping.Log;
            }

            set {
                _CommandMapping.Log = value;
                _QueryMapping.Log = value;
                _SQLQuery.Log = value;
            }
        }
    }
}
