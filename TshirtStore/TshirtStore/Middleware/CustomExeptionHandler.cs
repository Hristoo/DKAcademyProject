using System.Globalization;

namespace TshirtStore.Middleware
{
    public class CustomExeptionHandler : Exception
    {
        public CustomExeptionHandler() : base() { }

        public CustomExeptionHandler(string message) : base(message) { }

        public CustomExeptionHandler(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}
