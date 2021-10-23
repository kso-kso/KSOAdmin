using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using KSOAdmin.Models.Base;

namespace KSOAdmin.Core.EFDbContext
{
    public static class ModelOperation
    {
        /// <summary>
        /// 获取标记的表名字
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetEntityTableName(this Type type)
        {
            var attribute= type.GetCustomAttribute(typeof(EntityAttribute));
            if (attribute!=null && attribute is EntityAttribute)
            {
                return ((EntityAttribute)attribute).TableName;
            }
            return type.Name;
        }
        public static string GetEntityTableKey(this Type type) 
        {
            var enumerable = type.GetProperties().Where(key=>key.IsKey()).FirstOrDefault();
            return enumerable.Name;
        }
        public static bool IsKey(this PropertyInfo propertyInfo)
        {
            object[] keyAttributes = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false);
            if (keyAttributes.Length > 0)
                return true;
            return false;
        }

        public static IQueryable<T> OrderIsDesc<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> orderBy, bool IsOrderby)
        {
            if (IsOrderby)
            {
                return queryable.OrderBy(orderBy);
            }
            else 
            {
                return queryable.OrderByDescending(orderBy);
            }
        }

        /// <summary>
        /// 获取对象里指定成员名称
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="properties"> 格式 Expression<Func<entityt, object>> exp = x => new { x.字段1, x.字段2 };或x=>x.Name</param>
        /// <returns></returns>
        public static string[] GetExpressionProperty<TEntity>(this Expression<Func<TEntity, object>> properties)
        {
            if (properties == null)
                return new string[] { };
            if (properties.Body is NewExpression)
                return ((NewExpression)properties.Body).Members.Select(x => x.Name).ToArray();
            if (properties.Body is MemberExpression)
                return new string[] { ((MemberExpression)properties.Body).Member.Name };
            if (properties.Body is UnaryExpression)
                return new string[] { ((properties.Body as UnaryExpression).Operand as MemberExpression).Member.Name };
            throw new Exception("未实现的表达式");
        }
    }
}
