using CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace CORE.Base
{
    internal abstract class DataAccessObject<T> : DataAccess where T : BusinessObject
    {
        public DataAccessObject(string connectionString) : base(connectionString) { }

        #region Protected Methods

        protected T PopulateBusinessObjectFromReader(IDataReader dataReader)
        {
            try
            {
                T instance = Activator.CreateInstance<T>();

                foreach (PropertyInfo p in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal(p.Name)))
                    {
                        //var value = dataReader.GetValue(dataReader.GetOrdinal(p.Name));
                        //p.SetValue(instance, Convert.ChangeType(value, p.PropertyType), null);
                        p.SetValue(instance, dataReader.GetValue(dataReader.GetOrdinal(p.Name)), null);
                    }
                }

                return instance;
            }
            catch (Exception ex)
            {
                throw new Exception("Error parse.\n" + ex.Message, ex);
            }
        }

        protected List<T> PopulateObjectsFromReader(IDataReader dataReader)
        {
            try
            {
                List<T> list = new List<T>();

                while (dataReader.Read())
                {
                    list.Add(PopulateBusinessObjectFromReader(dataReader));
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Error parse all.\n" + ex.Message, ex);
            }
        }

        #endregion

        #region Public Methods

        public List<T> SelectFromStore(string storeName, params object[] values)
        {
            try
            {
                IDataReader dataReader = SqlHelper.ExecuteReader(MainConnection, storeName, values);
                return PopulateObjectsFromReader(dataReader);
            }
            catch (Exception ex)
            {
                throw new Exception("SelectFromStore::'" + storeName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public void SelectFromStore(out string ecode, out string edesc, string storeName, params object[] values)
        {
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(MainConnection, storeName, values);
                ecode = ds.Tables[0].Rows[0]["ECODE"].ToString();
                edesc = ds.Tables[0].Rows[0]["EDESC"].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("SelectFromStore::'" + storeName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public List<T> SelectFromStoreOutEcode(out string ecode, out string edesc, string storeName, params object[] values)
        {
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(MainConnection, storeName, values);
                ecode = ds.Tables[0].Rows[0]["ECODE"].ToString();
                edesc = ds.Tables[0].Rows[0]["EDESC"].ToString();
                if (ecode == "000" && ds.Tables.Count > 1)
                {
                    IDataReader dataReader = ds.Tables[1].CreateDataReader();
                    return PopulateObjectsFromReader(dataReader);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SelectFromStore::'" + storeName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public List<T> SelectFromStoreOutParam(string storeName, out object obj, params object[] values)
        {
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(MainConnection, storeName, values);
                IDataReader dataReader = ds.Tables[0].CreateDataReader();
                obj = ds.Tables[1].Rows[0][0];
                return PopulateObjectsFromReader(dataReader);
            }
            catch (Exception ex)
            {
                throw new Exception("SelectFromStoreOutParam::'" + storeName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        #endregion
    }
}
