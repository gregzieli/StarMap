using System.ComponentModel;

namespace StarMap.Core.Abstractions
{
  public interface INotifyElementChanged
  {
    /// <summary>
    /// Occurs when an element's property value in the collection changes.
    /// </summary>
    event PropertyChangedEventHandler ElementChanged;
  }
}
