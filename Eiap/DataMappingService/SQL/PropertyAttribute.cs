using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eiap
{
    /// <summary>
    /// 实体属性特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class PropertyAttribute : Attribute
    {
        private string _ColumnName;

        public PropertyAttribute(string ColumnName)
        {
            _ColumnName = ColumnName;
        }

        /// <summary>
        /// 属性名（列名）
        /// </summary>
        public string ColumnName { get { return _ColumnName; } }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 列长
        /// </summary>
        public int ColumnLength { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public string ForeignKey { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 是否索引
        /// </summary>
        public bool IsIndex { get; set; }

        public string IndexGroup { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsNull { get; set; }

    }
}
