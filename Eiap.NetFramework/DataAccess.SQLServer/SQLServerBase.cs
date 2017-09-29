
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Eiap.NetFramework
{
    public class SQLServerBase
    {
        internal void PrintLog(Action<string> log, string cmdText, IDataParameter[] paramters)
        {
            if (log != null)
            {
                StringBuilder logcontent = new StringBuilder("sql:");
                logcontent.Append(cmdText);
                logcontent.Append("\t");
                foreach (IDataParameter para in paramters)
                {
                    logcontent.Append(para.Value);
                    logcontent.Append("\t");
                }
                logcontent.Append("\r\n");
                log.Invoke(logcontent.ToString());
            }
        }
    }
}
