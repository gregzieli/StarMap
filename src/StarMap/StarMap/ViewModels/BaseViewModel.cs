using StarMap.Core.Abstractions;
using StarMap.Models;
using StarMap.Services;
using Xamarin.Forms;

namespace StarMap.ViewModels
{
    public class BaseViewModel : ObservableBase
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>() ?? new MockDataStore();

        private bool _isBusy;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                    IsNotBusy = !_isBusy;
            }
        }

        private bool _isNotBusy = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is not busy.
        /// </summary>
        /// <value><c>true</c> if this instance is not busy; otherwise, <c>false</c>.</value>
        public bool IsNotBusy
        {
            get => _isNotBusy;
            set
            {
                if (SetProperty(ref _isNotBusy, value))
                    IsBusy = !_isNotBusy;
            }
        }

        private string _title = string.Empty;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
