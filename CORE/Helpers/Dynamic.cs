using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CORE.Helpers
{
    public static class Dynamic
    {
        public static object CallByName(string namespaceName, string className, string methodName, params object[] obj)
        {
            try
            {
                Type t = Type.GetType(namespaceName + "." + className);
                foreach (MethodInfo m in t.GetMethods().Where(x => x.Name == methodName).OrderByDescending(x => x.GetParameters().Count()))
                {
                    ParameterInfo[] props = m.GetParameters();
                    if (props.Length >= obj.Length)
                    {
                        bool check = true;
                        for (int i = 0; i < obj.Length; i++)
                        {
                            if (obj[i].GetType() != props[i].ParameterType)
                            {
                                check = false;
                                break;
                            }
                        }
                        if (check && (props.Length == obj.Length || props.Length > obj.Length && props[obj.Length].HasDefaultValue))
                        {
                            return m.Invoke(t, obj);
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T GetValue<T>(this T businessObject, DataTable dt, int index)
        {
            try
            {
                foreach (PropertyInfo p in businessObject.GetType().GetProperties())
                {
                    p.SetValue(businessObject, Convert.ChangeType(dt.Rows[index][p.Name], p.PropertyType));
                }
            }
            catch (Exception)
            {
                return default(T);
            }
            return businessObject;
        }

        public static T GetValue<T>(this T businessObject, string name, object value)
        {
            try
            {
                foreach (PropertyInfo p in businessObject.GetType().GetProperties().Where(x => x.Name == name))
                {
                    p.SetValue(businessObject, Convert.ChangeType(value, p.PropertyType));
                    break;
                }
            }
            catch (Exception)
            {

            }
            return businessObject;
        }

        public static T GetValue<T>(this T businessObject, params Tuple<string, object>[] data)
        {
            try
            {
                foreach (var p in businessObject.GetType().GetProperties()
                    .Join(data, x => x.Name, y => y.Item1, (x, y) => new { PropertyInfo = x, Tuple = y }))
                {
                    p.PropertyInfo.SetValue(businessObject, Convert.ChangeType(p.Tuple.Item2, p.PropertyInfo.PropertyType));
                }
            }
            catch (Exception)
            {

            }
            return businessObject;
        }

        public static Tuple<string, object> Obj(string name, object value)
        {
            return new Tuple<string, object>(name, value);
        }
    }
}
