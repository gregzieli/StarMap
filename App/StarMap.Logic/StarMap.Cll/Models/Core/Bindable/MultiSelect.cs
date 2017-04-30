using Microsoft.Practices.Prism.Mvvm;

namespace StarMap.Cll.Models.Core.Bindable
{
  // I wanted the CLL abstracted from the MVVM pattern, so that all that binding logic was in the VM layer.
  // Optionally, I could:
  // 1. delete this class
  // 2. create a child in the VM namespace of the class that extended this one
  // 3. that child would have this property....
  // Or, a wrapper class, not a child. 
  // Those would adhere to the Open-Close principle.
  // For now, let's leave it like this - with one TODO - if there's time, to avoid prism dependency,
  // implement just INotifyPropertyChanged
  public class MultiSelectable : BindableBase
  {
    private bool _isSelected = true;
    public bool IsSelected
    {
      get { return _isSelected; }
      set { SetProperty(ref _isSelected, value); }
    }
  }
}
