using System;
using Common.Data;
using Common.Util;

namespace Common.Models
{
    public class PlatformException: ApplicationException
    {

        /// <summary>
        /// error code
        /// </summary>
        public int Code { get; set; }

        public PlatformException()
        {
	        Code = (int)ErrorCode.GeneralError;
        }

        public PlatformException(string message) : base(message)
        {
	        Code = (int)ErrorCode.GeneralError;
        }

        public PlatformException(string message, Exception innerException) : base(message, innerException)
        {
	        Code = (int)ErrorCode.GeneralError;
        }

        public PlatformException(int errorCode)
        {
	        Code = errorCode;
        }


	    public PlatformException(ErrorCode errorCode):base(errorCode.GetDescription())
		{
			Code = (int)errorCode;
		}

		public PlatformException(string message, int errorCode) : base(message)
        {
	        Code = errorCode;
        }

        public PlatformException(string message, Exception innerException, int errorCode)
            : base(message, innerException)
        {
	        Code = errorCode;
        }
    }
	
}
