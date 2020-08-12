using System;

namespace CORE
{
    public class InvalidBusinessObjectException : Exception
    {
        #region Data members

        string _methodName = string.Empty;

        #endregion

        #region Properties

        public string MethodName
        {
            get { return _methodName; }
            set { _methodName = value; }
        }

        #endregion

        #region Constructor

        public InvalidBusinessObjectException(string message) : base(message)
        {
        }

        #endregion
    }
}
