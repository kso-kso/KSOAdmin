using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using KSOAdmin.Models.Core;

namespace KSOAdmin.Core.EFDbContext
{
    public static class EFValidationProperty
    {
        /// <summary>
        /// 指定需要验证的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expression">对指定属性进行验证x=>{x.Name,x.Size}</param>
        /// <returns></returns>
        public static  ResponseModel ValidationEntity<T>(this T entity, Expression<Func<T, object>> expression = null, Expression<Func<T, object>> validateProperties = null)
        {
            return ValidationEntity<T>(entity, expression?.GetExpressionProperty<T>(), validateProperties?.GetExpressionProperty<T>());
        }
        /// <summary>
        /// specificProperties=null并且validateProperties=null，对所有属性验证，只验证其是否合法，不验证是否为空(除属性标识指定了不能为空外)
        /// specificProperties!=null，对指定属性校验，并且都必须有值
        /// null并且validateProperties!=null，对指定属性校验，不判断是否有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="specificProperties">验证指定的属性，并且非空判断</param>
        /// <param name="validateProperties">验证指定属性，只对字段合法性判断，不验证是否为空</param>
        /// <returns></returns>
        public static ResponseModel ValidationEntity<T>(this T entity, string[] specificProperties, string[] validateProperties = null)
        {
            ResponseModel responseData = new ResponseModel();
            if (entity == null) return ResponseModel.Fail("对象不能为null");

            PropertyInfo[] propertyArray = typeof(T).GetProperties();
            //若T为object取不到属性
            if (propertyArray.Length == 0)
            {
                propertyArray = entity.GetType().GetProperties();
            }
            List<PropertyInfo> compareProper = new List<PropertyInfo>();

            //只验证数据合法性，验证非空
            if (specificProperties != null && specificProperties.Length > 0)
            {
                compareProper.AddRange(propertyArray.Where(x => specificProperties.Contains(x.Name)));
            }

            //只验证数据合法性，不验证非空
            if (validateProperties != null && validateProperties.Length > 0)
            {
                compareProper.AddRange(propertyArray.Where(x => validateProperties.Contains(x.Name)));
            }
            if (compareProper.Count() > 0)
            {
                propertyArray = compareProper.ToArray();
            }
            foreach (PropertyInfo propertyInfo in propertyArray)
            {
                object value = propertyInfo.GetValue(entity);
                //设置默认状态的值
                if (propertyInfo.Name == "Enable" || propertyInfo.Name == "AuditStatus")
                {
                    if (value == null)
                    {
                        propertyInfo.SetValue(entity, 0);
                        continue;
                    }
                }
            }
            return ResponseModel.Success();
        }
         
    }
}
