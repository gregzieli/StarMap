using StarMap.Core.Abstractions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StarMap.Core.Models
{
    /// <summary>
    /// A collection that observes its members' PropertyChanged events.
    /// </summary>
    /// <remarks>
    /// Sometimes there is no point in using Observable Collection, if you're not interested to whatever happens to the collection as a whole.
    /// </remarks>
    public class ElementAwareCollection<T> : Collection<T>, INotifyElementChanged where T : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler ElementChanged;

        public ElementAwareCollection() : base()
        { }

        public ElementAwareCollection(IList<T> source) : base(source)
        {
            Subscribe(source);
        }

        protected override void InsertItem(int index, T item)
        {
            item.PropertyChanged += Item_PropertyChanged;
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            this[index].PropertyChanged -= Item_PropertyChanged;
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            Unsubscribe(this);
            base.ClearItems();
        }

        protected override void SetItem(int index, T item)
        {
            this[index].PropertyChanged -= Item_PropertyChanged;
            item.PropertyChanged += Item_PropertyChanged;
            base.SetItem(index, item);
        }

        private void Subscribe(IEnumerable source)
        {
            foreach (INotifyPropertyChanged item in source)
                item.PropertyChanged += Item_PropertyChanged;
        }

        private void Unsubscribe(IEnumerable source)
        {
            foreach (INotifyPropertyChanged item in source)
                item.PropertyChanged -= Item_PropertyChanged;
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e) => ElementChanged?.Invoke(sender, e);
    }
}
