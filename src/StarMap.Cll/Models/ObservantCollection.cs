using StarMap.Core.Abstractions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace StarMap.Core.Models
{
    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items added, removed, or when the whole list is refreshed.
    /// Also, it notifies when an element's property changes.
    /// </summary>
    public class ObservantCollection<T> : ObservableCollection<T>, INotifyElementChanged where T : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler ElementChanged;

        public ObservantCollection() : base()
        { }

        public ObservantCollection(IEnumerable<T> source) : base(source)
        {
            Subscribe(source);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                Unsubscribe(e.OldItems);

            if (e.NewItems != null)
                Subscribe(e.NewItems);

            base.OnCollectionChanged(e);
        }

        void Subscribe(IEnumerable source)
        {
            foreach (INotifyPropertyChanged item in source)
                item.PropertyChanged += Item_PropertyChanged;
        }

        void Unsubscribe(IEnumerable source)
        {
            foreach (INotifyPropertyChanged item in source)
                item.PropertyChanged -= Item_PropertyChanged;
        }

        //https://codeblog.jonskeet.uk/2015/01/30/clean-event-handlers-invocation-with-c-6/
        // if i unsubscribe from ElementChanged, it will be null, and without ?., throws nullrefexc.
        void Item_PropertyChanged(object sender, PropertyChangedEventArgs e) => ElementChanged?.Invoke(sender, e);
    }
}
