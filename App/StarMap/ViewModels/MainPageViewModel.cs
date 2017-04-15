using System;
using Prism.Commands;
using Prism.Navigation;
using StarMap.Cll.Abstractions;
using StarMap.Cll.Models;
using System.Collections.ObjectModel;

namespace StarMap.ViewModels
{
  public class MainPageViewModel : Navigator
  {
    IStarManager _starManager;

    private Star _selectedStar;
    public Star SelectedStar
    {
      get { return _selectedStar; }
      set { SetProperty(ref _selectedStar, value); }
    }

    private bool _isSelected;
    public bool IsSelected
    {
      get { return _isSelected; }
      set { SetProperty(ref _isSelected, value); }
    }

    private ObservableCollection<Star> _visibleStars;
    public ObservableCollection<Star> VisibleStars
    {
      get { return _visibleStars; }
      set { SetProperty(ref _visibleStars, value); }
    }

    public DelegateCommand SelectStarCommand { get; private set; }
    public DelegateCommand UnselectStarCommand { get; private set; }

    //string _title = "Titel";
    //public string Title
    //{
    //  get { return _title; }
    //  set { SetProperty(ref _title, value); }
    //}

    //private bool _isActive = false;
    //public bool IsActive
    //{
    //  get { return _isActive; }
    //  set { SetProperty(ref _isActive, value); }
    //}

    //public override DelegateCommand<string> NavigateCommand
    //{
    //  get => base.NavigateCommand;
    //  set => base.NavigateCommand = value.ObservesCanExecute(() => IsActive);
    //}
    public DelegateCommand ShowStarDetailsCommand { get; private set; }

    public MainPageViewModel(INavigationService navigationService, IStarManager starManager) : base(navigationService)
    {
      _starManager = starManager;
      // TODO: maybe move to OnNavigat[ed/ing]To
      VisibleStars = new ObservableCollection<Star>(
        _starManager.GetStars(
          new Cll.Filters.StarFilter() { Limit = 100, MaxDistance = 7000 }));

      SelectStarCommand = new DelegateCommand(SelectStar);
      try
      {
        ShowStarDetailsCommand = new DelegateCommand(ShowStarDetails).ObservesCanExecute(() => IsSelected);
      }
      catch (Exception e)
      {

        throw;
      }
      
    }

    private void ShowStarDetails()
    {
      Navigate("DetailPage", SelectedStar.Id);
      //Another option
      //Navigate($"NavigationPage/DetailPage?id={SelectedStar.Id}");
    }

    private void SelectStar()
    {
      if (SelectedStar == null)
      {
        SelectedStar = VisibleStars[new Random().Next(VisibleStars.Count)];
        IsSelected = true;
      }        
      else
      {
        SelectedStar = null;
        IsSelected = false;
      }
        
    }
  }
}
