using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LocalTool
{
    /// <summary>
    /// 数据转换类
    /// </summary>
    public class DataConversion
    {
        #region xml数据转实体对象

        #region 应用方式事例
        //static List<TestUserClass> testUserList = new List<TestUserClass>();
        //XmlDocument doc = new XmlDocument();
        //doc.Load(System.Environment.CurrentDirectory + "\\Configs\\SystemConfig\\TestUser.xml");
        //    XmlNodeList list = doc.SelectNodes("/TestUserList/TestUser");
        //    foreach (XmlNode item in list)
        //    {
        //        TestUserClass t = new TestUserClass();
        //t = XmlHelper.LoadConfiguration<TestUserClass>(item)
        // testUserList.Add(t);    
    #endregion

    /// <summary>
    /// 设置基本数据类型字段值
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="field">字段</param>
    /// <param name="node">节点</param>
    private static void SetBasicType(object obj, FieldInfo field, XmlNode node)
        {
            XmlNode element = node.SelectSingleNode(field.Name);
            if (element != null)
            { //有值
                field.SetValue(obj, GetValue(field.FieldType, element.InnerText));
            }
            else
            { //没值，默认或引发异常

            }
        }

        /// <summary>
        /// 设置基本数据类型属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="property">属性</param>
        /// <param name="node">节点</param>
        private static void SetBasicType(object obj, PropertyInfo property, XmlNode node)
        {
            XmlNode element = node.SelectSingleNode(property.Name);
            if (element != null)
            { //有值
                property.SetValue(obj, GetValue(property.PropertyType, element.InnerText), null);
            }
            else
            { //没值,默认或引发异常

            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="field">字段</param>
        /// <param name="node">节点</param>
        private static void SetArray(object obj, FieldInfo field, XmlNode node)
        {
            XmlNodeList nodelist = node.SelectNodes(field.Name);
            if (nodelist.Count > 0)
            {
                Type elementType = field.FieldType.GetElementType();
                Array array = Array.CreateInstance(elementType, nodelist.Count);
                for (int i = 0; i < nodelist.Count; i++)
                {
                    if (isBasicType(elementType))
                    { //是基本数据类型
                        array.SetValue(GetValue(elementType, nodelist[i].InnerText), i);
                    }
                    else if (elementType.IsArray)
                    {
                        throw new ArgumentException("不支持二维数组配置!");
                    }
                    else
                    { //是类
                        array.SetValue(LoadConfiguration(elementType, nodelist[i]), i);
                    }
                }
                field.SetValue(obj, array);

            }
            else
            {  // 默认值或引发异常

            }
        }

        /// <summary>
        /// 设置数组
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="property">属性</param>
        /// <param name="node">节点</param>
        private static void SetArray(object obj, PropertyInfo property, XmlNode node)
        {
            XmlNodeList nodelist = node.SelectNodes(property.Name);
            if (nodelist.Count > 0)
            {
                Type elementType = property.PropertyType.GetElementType();
                Array array = Array.CreateInstance(elementType, nodelist.Count);
                for (int i = 0; i < nodelist.Count; i++)
                {
                    if (isBasicType(elementType))
                    { //是基本数据类型
                        array.SetValue(GetValue(elementType, nodelist[i].InnerText), i);
                    }
                    else if (elementType.IsArray)
                    {
                        throw new ArgumentException("不支持二维数组配置!");
                    }
                    else
                    { //是类
                        array.SetValue(LoadConfiguration(elementType, nodelist[i]), i);
                    }
                }
                property.SetValue(obj, array, null);

            }
            else
            {  // 默认值或引发异常

            }
        }

        /// <summary>
        /// 设置字段数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="field">字段</param>
        /// <param name="node">节点</param>
        private static void SetObject(object obj, FieldInfo field, XmlNode node)
        {
            XmlNode element = node.SelectSingleNode(field.Name);
            if (element != null)
            { //有值
                field.SetValue(obj, LoadConfiguration(field.FieldType, element));
            }
            else
            { //默认值 或引发异常 

            }
        }

        /// <summary>
        /// 设置属性数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="property">属性</param>
        /// <param name="node">节点</param>
        private static void SetObject(object obj, PropertyInfo property, XmlNode node)
        {
            XmlNode element = node.SelectSingleNode(property.Name);
            if (element != null)
            { //有值
                property.SetValue(obj, LoadConfiguration(property.PropertyType, element), null);
            }
            else
            { //默认值 或引发异常

            }
        }

        /// <summary>
        /// 加载字段
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="type">类型</param>
        /// <param name="node">配置节点</param>
        private static void LoadField(object obj, Type type, XmlNode node)
        {
            FieldInfo[] fields = type.GetFields();
            int len = fields.Length;

            for (int i = 0; i < len; i++)
            {
                if (isBasicType(fields[i].FieldType))
                {//基本数据类型
                    SetBasicType(obj, fields[i], node);
                }
                else if (fields[i].FieldType.IsArray)
                {   //是数组
                    SetArray(obj, fields[i], node);
                }
                else
                {//是类
                    SetObject(obj, fields[i], node);
                }
            }
        }

        /// <summary>
        /// 加载属性
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="type">类型</param>
        /// <param name="node">配置节点</param>
        private static void LoadProperty(object obj, Type type, XmlNode node)
        {
            PropertyInfo[] properties = type.GetProperties();
            int len = properties.Length;

            for (int i = 0; i < len; i++)
            {
                if (isBasicType(properties[i].PropertyType))
                {//基本数据类型
                    SetBasicType(obj, properties[i], node);
                }
                else if (properties[i].PropertyType.IsArray)
                { //是数组
                    SetArray(obj, properties[i], node);
                }
                else
                { //是类
                    SetObject(obj, properties[i], node);
                }
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <param name="node">xml节点</param>
        /// <returns></returns>
        public static object LoadConfiguration(Type type, XmlNode node)
        {
            object t = type.Assembly.CreateInstance(type.FullName);

            LoadField(t, type, node);
            LoadProperty(t, type, node);

            return t;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T LoadConfiguration<T>(XmlNode node)
        {
            return (T)LoadConfiguration(typeof(T), node);
        }

        /// <summary>
        /// 字符串转基本数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object GetValue(Type type, string value)
        {
            switch (type.FullName)
            {
                case "System.String":
                    return value;
                case "System.DateTime":
                    return Convert.ToDateTime(value);
                case "System.Int16":
                    return Convert.ToInt16(value);
                case "System.UInt16":
                    return Convert.ToUInt16(value);
                case "System.Int32":
                    return Convert.ToInt32(value);
                case "System.UInt32":
                    return Convert.ToUInt32(value);
                case "System.Int64":
                    return Convert.ToInt64(value);
                case "System.UInt64":
                    return Convert.ToUInt64(value);
                case "System.Boolean":
                    return !(value == "0" || string.IsNullOrEmpty(value));
                default:
                    throw new ArgumentException("配置错误,类型不符合!");
            }
        }

        /// <summary>
        /// 获取某类型是不是基本数据类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true 表示基本数据类型 false 表示不是</returns>
        public static bool isBasicType(Type type)
        {
            if (type.FullName == "System.DateTime")
            {
                return true;
            }
            return type.IsPrimitive || type.FullName == "System.String";
        }
        #endregion
        #region 实例类转xml

        #endregion
    }
}
