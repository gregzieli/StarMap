using System;

namespace StarMap.Cll.Exceptions
{
  public class StarDetailUrhoException : Exception
  {
    public StarDetailUrhoException() { }
    public StarDetailUrhoException(string message) : base(message) { }
    public StarDetailUrhoException(string message, Exception inner) : base(message, inner) { }
  }
}
