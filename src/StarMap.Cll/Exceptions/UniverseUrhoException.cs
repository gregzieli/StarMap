using System;

namespace StarMap.Cll.Exceptions
{
    public class UniverseUrhoException : Exception
    {
        public UniverseUrhoException() { }
        public UniverseUrhoException(string message) : base(message) { }
        public UniverseUrhoException(Exception inner) : base("Universe error", inner) { }
        public UniverseUrhoException(string message, Exception inner) : base(message, inner) { }

    }
}
