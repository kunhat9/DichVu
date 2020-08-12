using CORE.Helpers;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CORE.Base
{
    internal abstract class DataAccess : IDisposable
    {
        #region Data Members

        SqlConnection _mainConnection;
        bool _isDisposed;

        #endregion

        #region Properties

        protected SqlConnection MainConnection
        {
            get { return _mainConnection; }
        }

        #endregion

        #region Constructor

        public DataAccess(string connectionString)
        {
            _mainConnection = new SqlConnection();
            _mainConnection.ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ToString();
            _isDisposed = false;
        }

        #endregion

        #region Public Methods

        public bool ExecuteNonQuery(string storeName, params object[] values)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(MainConnection, storeName, values) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("ExecuteNonQuery::'" + storeName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public DataTable ExecuteStore(string storeName, params object[] values)
        {
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(MainConnection, storeName, values);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("ExecuteStore::'" + storeName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public DataTable ExecuteStoreOutParam(string storeName, out object obj, params object[] values)
        {
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(MainConnection, storeName, values);
                obj = ds.Tables[1];
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("ExecuteStoreOutParam::'" + storeName + "'.\n" + ex.Message, ex);
            }
            finally
            {
                MainConnection.Close();
            }
        }

        public SqlTransaction CreateTransaction()
        {
            if (_mainConnection.State == ConnectionState.Closed)
                _mainConnection.Open();
            return _mainConnection.BeginTransaction();
        }

        public void Close()
        {
            _mainConnection.Close();
        }

        #endregion

        #region IDisposeable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool bIsDisposing)
        {
            if (!_isDisposed)
            {
                if (bIsDisposing)
                {
                    _mainConnection.Dispose();
                    _mainConnection = null;
                }
            }
            _isDisposed = true;
        }

        #endregion
    }
}
