
using System;
using System.Collections.Generic;
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
        private List<tEntity> _AddEntityList;
        private List<tEntity> _UpdateEntityList;
        private List<TPrimarykey> _DeletePrimarykeyList;
        private ISQLCommandMapping<tEntity, TPrimarykey> _CommandMapping;
        private ISQLDataQueryMapping<tEntity, TPrimarykey> _QueryMapping;
        private ISQLQuery _SQLQuery;
        private ICurrentUnitOfWork _CurrentUnitOfWork;
        private readonly IMethodManager _MethodManager;

        public Repository(ISQLCommandMapping<tEntity, TPrimarykey> CommandMapping,
            ISQLDataQueryMapping<tEntity, TPrimarykey> QueryMapping,
            ISQLQuery SQLQuery,
            IMethodManager methodManager)
        {
            _CommandMapping = CommandMapping;
            _QueryMapping = QueryMapping;
            _SQLQuery = SQLQuery;
            _CurrentUnitOfWork = DependencyManager.Instance.Resolver<ICurrentUnitOfWork>();
            _CurrentUnitOfWork.CurrentUnitOfWork.SetRepository(this);
            _MethodManager = methodManager;
        }

        public virtual tEntity Add(tEntity entity)
        {
            if (_AddEntityList == null)
            {
                _AddEntityList = new List<tEntity>();
            }
            _AddEntityList.Add(entity);
            return entity;
        }

        public virtual tEntity Update(tEntity entity)
        {
            if (_UpdateEntityList == null)
            {
                _UpdateEntityList = new List<tEntity>();
            }
            _UpdateEntityList.Add(entity);
            return entity;
        }

        public virtual void Delete(TPrimarykey primarykey)
        {
            if (_DeletePrimarykeyList == null)
            {
                _DeletePrimarykeyList = new List<TPrimarykey>();
            }
            _DeletePrimarykeyList.Add(primarykey);
        }

        public virtual ISQLDataQueryMapping<tEntity, TPrimarykey> Query()
        {
            return _QueryMapping;
        }

        public virtual TResult Query<TResult>(string cmdText, System.Data.CommandType cmdType, System.Data.IDataParameter[] paramters = null)
        {
            TResult result = default(TResult);
            DataSet dataset = _SQLQuery.ExcuteGetDateSet(cmdText, cmdType, paramters);
            result = dataset.DataSetToEntityList<TResult>(_MethodManager);
            return result;
        }

        public virtual void Commit()
        {
            try
            {
                if (_AddEntityList != null && _AddEntityList.Count > 0)
                {
                    _CommandMapping.BatchInsertEntity(_AddEntityList);
                    _AddEntityList.Clear();
                }
                if (_UpdateEntityList != null && _UpdateEntityList.Count > 0)
                {
                    _CommandMapping.BatchUpdateEntity(_UpdateEntityList);
                    _UpdateEntityList.Clear();
                }
                if (_DeletePrimarykeyList != null && _DeletePrimarykeyList.Count > 0)
                {
                    _CommandMapping.BatchDeleteEntity(_DeletePrimarykeyList);
                    _DeletePrimarykeyList.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            if (_AddEntityList != null && _AddEntityList.Count > 0)
            {
                _AddEntityList.Clear();
            }
            if (_UpdateEntityList != null && _UpdateEntityList.Count > 0)
            {
                _UpdateEntityList.Clear();
            }
            if (_DeletePrimarykeyList != null && _DeletePrimarykeyList.Count > 0)
            {
                _DeletePrimarykeyList.Clear();
            }
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

        public virtual ICurrentUnitOfWork CurrentUnitOfWork
        {
            get { return _CurrentUnitOfWork; }
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
