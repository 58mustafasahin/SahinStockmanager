using System.Globalization;

namespace SM.Core.Exceptions
{
    public class CustomAppException : Exception
    {
        public CustomAppException() : base() { }

        public CustomAppException(string message) : base(message) { }

        public CustomAppException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
