using CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace CORE.Base
{
    internal abstract class DataAccessTable<T> : DataAccessObject<T> where T : BusinessObject
    {
        public DataAccessTable(string connectionString) : base(connectionString) { }

        #region Private Methods

        private string GetKeyName()
        {
            try
            {
                return typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.GetCustomAttribute(typeof(PrimaryKey), true) != null)
                    .Select(x => x.Name)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(typeof(T).Name + "::NOT key.\n" + ex.Message, ex);
            }
        }

        private object GetKeyValue(T businessObject)
        {
            try
            {
                PropertyInfo p = businessObject.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.GetCustomAttribute(typeof(PrimaryKey), true) != null)
                    .FirstOrDefault();
                return p.GetValue(businessObject, null);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Public Methods

        public T SelectByPrimaryKey(object value)
        {
            string tableName = typeof(T).Name;
            string fieldName = GetKeyName();

            try
            {
                IDataReader dataReader = SqlHelper.ExecuteReader(
                    MainConnection
                    , CommandType.Text
                    , "SELECT * FROM " + tableName + " WHERE " + fieldName + " = @Value"
                    , new SqlParameter("@Value", value)
                );
                if (dataReader.Read())
                {
                    try
                    {
                        return PopulateBusinessObjectFromReader(dataReader);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        dataReader.Dispose();
                    }
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SelectByPrimaryKey::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public List<T> FilterByField(string fieldName, object value)
        {
            string tableName = typeof(T).Name;

            try
            {
                IDataReader dataReader = SqlHelper.ExecuteReader(
                    MainConnection
                    , CommandType.Text
                    , "SELECT * FROM " + tableName + " WHERE " + fieldName + " = @Value"
                    , new SqlParameter("@Value", value)
                );
                return PopulateObjectsFromReader(dataReader);
            }
            catch (Exception ex)
            {
                throw new Exception("FilterByField::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public bool Insert(T businessObject, bool withKey = false)
        {
            if (!businessObject.IsValid)
            {
                throw new InvalidBusinessObjectException(businessObject.ValidationResult);
            }

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string tableName = businessObject.GetType().Name;
            string columns = "";
            string values = "";
            string prefix = "";
            List<string> arr = tableName.Split('_').ToList();
            if (arr.Count == 2)
            {
                prefix = arr[1].Substring(0, 4);
            }
            try
            {
                foreach (var p in businessObject.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (p.GetCustomAttribute(typeof(PrimaryKey), true) != null)
                    {
                        string seq = ((PrimaryKey)p.GetCustomAttribute(typeof(PrimaryKey), true)).Sequence;
                        if (!string.IsNullOrEmpty(seq))
                        {
                            columns += p.Name + ",";
                            values += " '" + prefix + "' + RIGHT('000000000' + CAST(NEXT VALUE FOR " + seq + " AS varchar), 8), ";
                        }
                        else if (withKey)
                        {
                            values += p.PropertyType.Name == "string" ? "N'@" + p.Name + "'," : "@" + p.Name + ",";
                            columns += p.Name + ",";
                            sqlParams.Add(new SqlParameter("@" + p.Name, p.GetValue(businessObject, null)));
                        }
                    }
                    else
                    {
                        values += p.PropertyType.Name == "string" ? "N'@" + p.Name + "'," : "@" + p.Name + ",";
                        columns += p.Name + ",";
                        sqlParams.Add(new SqlParameter("@" + p.Name, p.GetValue(businessObject, null)));
                    }
                }

                int count = SqlHelper.ExecuteNonQuery(
                    MainConnection
                    , CommandType.Text
                    , "INSERT INTO " + tableName + "(" + columns.TrimEnd(',') + ") VALUES (" + values.TrimEnd(',') + ")"
                    , sqlParams.ToArray()
                );
                if (count == 0) return false;
                else return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Insert::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public bool InsertWithTransaction(SqlTransaction sqlTransaction, T businessObject, bool withKey = false)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string tableName = businessObject.GetType().Name;
            string columns = "";
            string values = "";
            string prefix = "";
            List<string> arr = tableName.Split('_').ToList();
            if (arr.Count == 2)
            {
                prefix = arr[1].Substring(0, 4);
            }
            try
            {
                foreach (var p in businessObject.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (p.GetCustomAttribute(typeof(PrimaryKey), true) != null)
                    {
                        string seq = ((PrimaryKey)p.GetCustomAttribute(typeof(PrimaryKey), true)).Sequence;
                        if (!string.IsNullOrEmpty(seq))
                        {
                            columns += p.Name + ",";
                            values += " '" + prefix + "' + RIGHT('000000000' + CAST(NEXT VALUE FOR " + seq + " AS varchar), 8), ";
                        }
                        else
                        {
                            values += p.PropertyType.Name == "string" ? "N'@" + p.Name + "'," : "@" + p.Name + ",";
                            columns += p.Name + ",";
                            sqlParams.Add(new SqlParameter("@" + p.Name, p.GetValue(businessObject, null)));
                        }
                    }
                    else
                    {
                        values += p.PropertyType.Name == "string" ? "N'@" + p.Name + "'," : "@" + p.Name + ",";
                        columns += p.Name + ",";
                        sqlParams.Add(new SqlParameter("@" + p.Name, p.GetValue(businessObject, null)));
                    }
                }

                int count = SqlHelper.ExecuteNonQuery(
                    sqlTransaction
                    , CommandType.Text
                    , "INSERT INTO " + tableName + "(" + columns.TrimEnd(',') + ") VALUES (" + values.TrimEnd(',') + ")"
                    , sqlParams.ToArray()
                );
                if (count == 0) return false;
                else return true;
            }
            catch (Exception ex)
            {
                throw new Exception("InsertWithTransaction::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }
        public bool UpdateWithTransaction(T businessObject, SqlTransaction sqlTransaction)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string tableName = businessObject.GetType().Name;
            string keyName = GetKeyName();
            object keyValue = GetKeyValue(businessObject);
            string values = "";
            try
            {
                foreach (var p in businessObject.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.GetCustomAttribute(typeof(PrimaryKey), true) == null))
                {
                    values += p.Name + " = @" + p.Name + ",";
                    SqlParameter temp = new SqlParameter("@" + p.Name, p.GetValue(businessObject, null));
                    sqlParams.Add(temp);
                }
                sqlParams.Add(new SqlParameter("@" + keyName, keyValue));

                int count = SqlHelper.ExecuteNonQuery(
                    sqlTransaction
                    , CommandType.Text
                    , "UPDATE " + tableName + " SET " + values.TrimEnd(',') + " WHERE " + keyName + " = @" + keyName
                    , sqlParams.ToArray()
                );
                if (count == 0) return false;
                else return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Update::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }
        public bool Update(T businessObject)
        {
            if (!businessObject.IsValid)
            {
                throw new InvalidBusinessObjectException(businessObject.ValidationResult);
            }

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string tableName = businessObject.GetType().Name;
            string keyName = GetKeyName();
            object keyValue = GetKeyValue(businessObject);
            string values = "";

            try
            {
                foreach (var p in businessObject.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.GetCustomAttribute(typeof(PrimaryKey), true) == null))
                {
                    values += p.Name + " = @" + p.Name + ",";
                    SqlParameter temp = new SqlParameter("@" + p.Name, p.GetValue(businessObject, null));
                    sqlParams.Add(temp);
                }
                sqlParams.Add(new SqlParameter("@" + keyName, keyValue));

                int count = SqlHelper.ExecuteNonQuery(
                    MainConnection
                    , CommandType.Text
                    , "UPDATE " + tableName + " SET " + values.TrimEnd(',') + " WHERE " + keyName + " = @" + keyName
                    , sqlParams.ToArray()
                );
                if (count == 0) return false;
                else return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Update::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public bool UpdateWithTransaction(SqlTransaction sqlTransaction, T businessObject)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string tableName = businessObject.GetType().Name;
            string keyName = GetKeyName();
            object keyValue = GetKeyValue(businessObject);
            string values = "";

            try
            {
                foreach (var p in businessObject.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.GetCustomAttribute(typeof(PrimaryKey), true) == null))
                {
                    values += p.Name + " = @" + p.Name + ",";
                    SqlParameter temp = new SqlParameter("@" + p.Name, p.GetValue(businessObject, null));
                    sqlParams.Add(temp);
                }
                sqlParams.Add(new SqlParameter("@" + keyName, keyValue));

                int count = SqlHelper.ExecuteNonQuery(
                    sqlTransaction
                    , CommandType.Text
                    , "UPDATE " + tableName + " SET " + values.TrimEnd(',') + " WHERE " + keyName + " = @" + keyName
                    , sqlParams.ToArray()
                );
                if (count == 0) return false;
                else return true;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateWithTransaction::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }
        public bool DeleteWithTransaction(SqlTransaction sqlTransaction,object value)
        {
            string tableName = typeof(T).Name;

            try
            {
                int count = SqlHelper.ExecuteNonQuery(
                    sqlTransaction
                    , CommandType.Text
                    , "DELETE FROM " + tableName + " WHERE " + GetKeyName() + " = @Value"
                    , new SqlParameter("@Value", value)
                );
                if (count == 0) return false;
                else return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Delete::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }
        public bool Delete(object value)
        {
            string tableName = typeof(T).Name;

            try
            {
                int count = SqlHelper.ExecuteNonQuery(
                    MainConnection
                    , CommandType.Text
                    , "DELETE FROM " + tableName + " WHERE " + GetKeyName() + " = @Value"
                    , new SqlParameter("@Value", value)
                );
                if (count == 0) return false;
                else return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Delete::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public bool DeleteByField(string fieldName, object value)
        {
            string tableName = typeof(T).Name;

            try
            {
                int count = SqlHelper.ExecuteNonQuery(
                    MainConnection
                    , CommandType.Text
                    , "DELETE FROM " + tableName + " WHERE " + fieldName + " = @Value"
                    , new SqlParameter("@Value", value)
                );
                if (count == 0) return false;
                else return true;
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteByField::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public List<T> SelectAll()
        {
            string tableName = typeof(T).Name;

            try
            {
                IDataReader dataReader = SqlHelper.ExecuteReader(
                                MainConnection
                                , CommandType.Text
                                , "SELECT * FROM " + tableName
                                );
                return PopulateObjectsFromReader(dataReader);
            }
            catch (Exception ex)
            {
                throw new Exception("SelectAll::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public string InsertReturnId(T businessObject, bool withKey = false)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string tableName = businessObject.GetType().Name;
            string columns = "";
            string values = "";
            string prefix = "";
            string id = "";
            List<string> arr = tableName.Split('_').ToList();
            if (arr.Count == 2)
            {
                prefix = arr[1].Substring(0, 4);
            }
            try
            {
                foreach (var p in businessObject.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (p.GetCustomAttribute(typeof(PrimaryKey), true) != null)
                    {
                        string seq = ((PrimaryKey)p.GetCustomAttribute(typeof(PrimaryKey), true)).Sequence;

                        if (!string.IsNullOrEmpty(seq))
                        {
                            columns += p.Name + ",";
                            values += " '" + prefix + "' + RIGHT('0000000000' + CAST(NEXT VALUE FOR " + seq + " AS varchar), 8), ";
                        }
                        else if (withKey)
                        {
                            values += p.PropertyType.Name == "string" ? "N'@" + p.Name + "'," : "@" + p.Name + ",";
                            columns += p.Name + ",";
                            SqlParameter temp = new SqlParameter("@" + p.Name, p.GetValue(businessObject, null));
                            sqlParams.Add(temp);
                        }
                    }
                    else
                    {
                        values += p.PropertyType.Name == "string" ? "N'@" + p.Name + "'," : "@" + p.Name + ",";
                        columns += p.Name + ",";
                        SqlParameter temp = new SqlParameter("@" + p.Name, p.GetValue(businessObject, null));
                        sqlParams.Add(temp);
                    }
                }
                string tableID = businessObject.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)[0].Name;

                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(
                    MainConnection
                    , CommandType.Text
                    , "INSERT INTO " + tableName + "(" + columns.TrimEnd(',') + ") OUTPUT INSERTED." + tableID + " VALUES (" + values.TrimEnd(',') + ")"
                    , sqlParams.ToArray()
                );
                id = ds.Tables[0].Rows[0][0].ToString();
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("InsertReturnId::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public string InsertReturnIdWithTransaction(SqlTransaction sqlTransaction, T businessObject, bool withKey = false)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string tableName = businessObject.GetType().Name;
            string columns = "";
            string values = "";
            string prefix = "";
            string id = "";
            List<string> arr = tableName.Split('_').ToList();
            if (arr.Count == 2)
            {
                prefix = arr[1].Substring(0, 4);
            }
            try
            {
                foreach (var p in businessObject.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (p.GetCustomAttribute(typeof(PrimaryKey), true) != null)
                    {
                        string seq = ((PrimaryKey)p.GetCustomAttribute(typeof(PrimaryKey), true)).Sequence;

                        if (!string.IsNullOrEmpty(seq))
                        {
                            columns += p.Name + ",";
                            values += " '" + prefix + "' + RIGHT('0000000000' + CAST(NEXT VALUE FOR " + seq + " AS varchar), 8), ";
                        }
                        else
                        {
                            values += p.PropertyType.Name == "string" ? "N'@" + p.Name + "'," : "@" + p.Name + ",";
                            columns += p.Name + ",";
                            SqlParameter temp = new SqlParameter("@" + p.Name, p.GetValue(businessObject, null));
                            sqlParams.Add(temp);
                        }
                    }
                    else
                    {
                        values += p.PropertyType.Name == "string" ? "N'@" + p.Name + "'," : "@" + p.Name + ",";
                        columns += p.Name + ",";
                        SqlParameter temp = new SqlParameter("@" + p.Name, p.GetValue(businessObject, null));
                        sqlParams.Add(temp);
                    }
                }
                string tableID = businessObject.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)[0].Name;

                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(
                    sqlTransaction
                    , CommandType.Text
                    , "INSERT INTO " + tableName + "(" + columns.TrimEnd(',') + ") OUTPUT INSERTED." + tableID + " VALUES (" + values.TrimEnd(',') + ")"
                    , sqlParams.ToArray()
                );
                id = ds.Tables[0].Rows[0][0].ToString();
                if (id.Contains(prefix) == false) return "FAILED";
                else return id;
            }
            catch (Exception ex)
            {
                throw new Exception("InsertReturnId::'" + tableName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public List<string> SelectObjectByTable(string tableName, string fieldName, string value)
        {

            try
            {
                IDataReader dataReader = SqlHelper.ExecuteReader(
                                MainConnection
                                , CommandType.Text
                                , "SELECT * FROM " + tableName + " WHERE " + fieldName + " = @Value"
                                , new SqlParameter("@Value", value)
                                );
                if (dataReader.Read())
                {
                    try
                    {
                        List<string> list = new List<string>();
                        for (var i = 0; i < dataReader.FieldCount; i++)
                        {
                            string a = dataReader.GetName(i) + "-" + dataReader.GetValue(i);
                            list.Add(a);
                        }


                        return list;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        dataReader.Dispose();
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(tableName + "::SelectByPrimaryKey::Error occured.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        #endregion
    }
}