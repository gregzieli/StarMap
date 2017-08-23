using System.ComponentModel;

namespace StarMap.Core.Abstractions
{
  /// <summary>
  /// Notifies clients that a collection's element value has changed.
  /// </summary>
  public interface INotifyElementChanged
  {
    /// <summary>
    /// Occurs when an element's property value in the collection changes.
    /// </summary>
    event PropertyChangedEventHandler ElementChanged;
  }
}
