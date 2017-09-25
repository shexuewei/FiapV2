
using System;

namespace Eiap
{
    /// <summary>
    /// 数据库访问拦截器
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public class SQLDataAccessLoggerAttribute : InterceptorMethodBeginAttibute
    {
        private LogLevel _Level;
        private bool _Enable;


        public SQLDataAccessLoggerAttribute(LogLevel level, bool enable)
        {
            this.Priority = -1;
            _Level = level;
            _Enable = enable;
        }


        /// <summary>
        /// 执行数据库访问
        /// </summary>
        /// <param name="args"></param>
        public override void Execute(InterceptorMethodArgs args)
        {
            ISQLBase sqlinstance = args.InstanceObject as ISQLBase;
            if (_Enable)
            {
                sqlinstance.Logger.Print(args.MethodParameters[0].ToString(), "SQLDataAccessLoggerAttribute", _Level);
            }
        }
    }
}
