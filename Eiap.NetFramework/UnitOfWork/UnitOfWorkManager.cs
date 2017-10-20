
using System;
using System.Collections.Generic;

namespace Eiap.NetFramework
{
    public class UnitOfWorkManager : IUnitOfWork
    {
        List<IUnitOfWorkCommandConnection> repositoryList = null;
        ISQLCommandDataAccessConnection _Con = null;

        public UnitOfWorkManager(ISQLCommandDataAccessConnection con)
        {
            _Con = con;
            repositoryList = new List<IUnitOfWorkCommandConnection>();
        }

        public void SetRepository(IUnitOfWorkCommandConnection res)
        {
            res.SQLDataAccessConnection = _Con;
            repositoryList.Add(res);
        }

        public void Commit(bool istransaction = false)
        {
            try
            {
                if (istransaction)
                {
                    _Con.Commit();
                }
            }
            catch (Exception ex)
            {
                _Con.Rollback();
                throw ex;
            }
        }

        public void Dispose()
        {
            if (repositoryList != null && repositoryList.Count > 0)
            {
                foreach (IUnitOfWorkCommandConnection res in repositoryList)
                {
                    IDisposable disposable = res as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        public void Open(bool istransaction = false)
        {
            try
            {
                _Con.Create();
                _Con.DBOpen();
                if (istransaction)
                {
                    _Con.BeginTransaction();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Close()
        {
            Dispose();
        }
    }
}
