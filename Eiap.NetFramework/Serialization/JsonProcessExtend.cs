using System;
using System.Collections;

namespace Eiap.NetFramework
{
    /// <summary>
    /// Json处理扩展类
    /// </summary>
    public static class JsonProcessExtend
    {
        /// <summary>
        /// 判断是否常用类型
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static bool IsNormalType(this Type objectType)
        {
            if (objectType.IsPrimitive 
                || objectType == typeof(String) 
                || objectType == typeof(Decimal)
                || objectType == typeof(DateTime))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取集合数
        /// </summary>
        /// <param name="enumeratorList"></param>
        /// <returns></returns>
        public static int GetEnumeratorCount(this IEnumerator enumeratorList)
        {
            int objCount = 0;
            while (enumeratorList.MoveNext())
            {
                objCount++;
            }
            return objCount;
        }
    }
}
