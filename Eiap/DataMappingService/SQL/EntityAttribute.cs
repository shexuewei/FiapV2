using System;

namespace Eiap
{
    /// <summary>
    /// 实体特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class EntityAttribute : Attribute
    {
        private string _TableName;
        public EntityAttribute(string TableName)
        {
            _TableName = TableName;
        }

        public virtual string TableName { get { return _TableName; } }
    }
}
