using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using System.Collections.ObjectModel;
using StarMap.Cll.Models;

namespace StarMap.ViewModels
{
  public class MasterDetailViewModel : Navigator
  {
    private ObservableCollection<Constellation> _constellations;
    public ObservableCollection<Constellation> Constellations
    {
      get { return _constellations; }
      set { SetProperty(ref _constellations, value); }
    }
    public MasterDetailViewModel(INavigationService navigationService) : base(navigationService)
    {
      Constellations = new ObservableCollection<Constellation>(new List<Constellation>() { new Constellation() { Name = "UGAGA00" } });
    }
  }
}
