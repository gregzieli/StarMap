using System;
using Xamarin.Forms;

namespace StarMap.Views
{
  public partial class MasterDetail : MasterDetailPage
  {
    public MasterDetail()
    {
      InitializeComponent();
    }
    
    void SwitchChanged(object sender, ToggledEventArgs e)
    {
      // Very sad that it comes to this. If there's time, think of another solution.
      var vm = BindingContext as ViewModels.MasterDetailViewModel;
      if (vm == null)
        return;

      // It's not OK either, it get's called too many times.
      // Probably the best solution would be to lose the MasterDetail altogether
      // and just implement a view that would slide the same way
      // then I would be in the same context, not needing those events

      // I want to send an event with constellation filter when:
      // 1. Select All is clicked
      // 2. Clear All is clicked
      // 3. Any of the switches is clicked
    }
  }
}