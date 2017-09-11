using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Xml;

namespace RunTecMs.Common.ConvertUtility
{
  public  class ConvertToList
    {
        /// <summary>
        /// DataSet转换成集合
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="dataSet">数据源</param>
        /// <param name="tableIndex">需要转换表的索引</param>
        /// <returns></returns>
        public static IList<T> DataSetToList<T>(DataSet dataSet, int tableIndex)
        {
            //确认参数有效
            if (dataSet == null || dataSet.Tables.Count <= 0 || tableIndex < 0)
            {
                return null;
            }
            DataTable dt = dataSet.Tables[tableIndex];

            IList<T> list = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //创建泛型对象
                T _t = Activator.CreateInstance<T>();
                //获取对象所有属性
                PropertyInfo[] propertyInfo = _t.GetType().GetProperties();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        //属性名称和列名相同时赋值
                        if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                info.SetValue(_t, dt.Rows[i][j], null);
                            }
                            else
                            {
                                info.SetValue(_t, null, null);
                            }
                            break;
                        }
                    }
                }
                list.Add(_t);
            }
            return list;
        }

        /// <summary>
        /// DataTable转换成集合
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="dataSet">数据源</param>
        /// <param name="tableIndex">需要转换表的索引</param>
        /// <returns></returns>
        public static IList<T> DataTableToList<T>(DataTable dataTable)
        {
            //确认参数有效
            if (dataTable == null || dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
            {
                return null;
            }
            DataTable dt = dataTable;

            IList<T> list = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //创建泛型对象
                T _t = Activator.CreateInstance<T>();
                //获取对象所有属性
                PropertyInfo[] propertyInfo = _t.GetType().GetProperties();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        //属性名称和列名相同时赋值
                        if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                info.SetValue(_t, dt.Rows[i][j], null);
                            }
                            else
                            {
                                info.SetValue(_t, null, null);
                            }
                            break;
                        }
                    }
                }
                list.Add(_t);
            }
            return list;
        }

        /// <summary>
        ///  xml转换为对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="headtag"></param>
        /// <returns></returns>
        public static List<T> XmlToList<T>(string xml, string headtag) where T : new()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            List<T> list = new List<T>();

            PropertyInfo[] propinfos = null;

            XmlNodeList nodelist = doc.GetElementsByTagName(headtag);
            foreach (XmlNode nodef in nodelist)
            {
                XmlNodeList itemNodelist = nodef.ChildNodes;
                T entity = new T();
                foreach (XmlNode node in itemNodelist)
                {
                    //初始化propertyinfo
                    if (propinfos == null)
                    {
                        Type objtype = entity.GetType();
                        propinfos = objtype.GetProperties();
                    }
                    //填充entity类的属性
                    foreach (PropertyInfo propinfo in propinfos)
                    {
                        if (!string.Equals(node.Name, propinfo.Name, StringComparison.CurrentCultureIgnoreCase))
                            continue;

                        string v = node.InnerText;
                        if (v != null)
                        {
                            propinfo.SetValue(entity, Convert.ChangeType(v, propinfo.PropertyType), null);
                            continue;
                        }
                    }
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// DataTable转换成单项目
        /// </summary>
        /// <typeparam dataTable>转换数据源</typeparam>
        /// <returns></returns>
        public static IList<int> DataTableToIntList(DataTable dataTable)
        {
            //确认参数有效
            if (dataTable == null || dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
            {
                return null;
            }
            DataTable dt = dataTable;

            IList<int> list = new List<int>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //创建泛型对象
                int result = 0;
                //获取对象所有属性
                if (dt.Rows[i][0] != DBNull.Value)
                {
                    result = Convert.ToInt32(dt.Rows[i][0]);
                    list.Add(result);
                }
            }
            return list;
        }

        /// <summary>
        /// DataTable转换成单项目
        /// </summary>
        /// <typeparam dataTable>转换数据源</typeparam>
        /// <returns></returns>
        public static IList<string> DataTableToStringList(DataTable dataTable)
        {
            //确认参数有效
            if (dataTable == null || dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
            {
                return null;
            }
            DataTable dt = dataTable;

            IList<string> list = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //创建泛型对象
                string result = "";
                //获取对象所有属性
                if (dt.Rows[i][0] != DBNull.Value)
                {
                    result = Convert.ToString(dt.Rows[i][0]);
                    list.Add(result);
                }
            }
            return list;
        }
    }
}
