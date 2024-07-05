using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SceneScribe.Engine;

/// <summary>
/// A collection of <see cref="INotifyPropertyChanged"/> items that raises <see cref="ObservableCollection{T}.CollectionChanged"/>
/// when any item raises <see cref="INotifyPropertyChanged.PropertyChanged"/>.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
public class TrulyObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
{
	public TrulyObservableCollection() : base()
		=> CollectionChanged += new NotifyCollectionChangedEventHandler(TrulyObservableCollection_CollectionChanged);

	/// <summary>
	/// Subscribes <see cref="Item_PropertyChanged"/> to every new item's PropertyChanged event.
	/// Removes the subscription from removed items.
	/// </summary>
	private void TrulyObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.NewItems != null)
		{
			foreach (INotifyPropertyChanged item in e.NewItems)
			{
				item.PropertyChanged += new PropertyChangedEventHandler(Item_PropertyChanged);
			}
		}

		if (e.OldItems != null)
		{
			foreach (INotifyPropertyChanged item in e.OldItems)
			{
				item.PropertyChanged -= new PropertyChangedEventHandler(Item_PropertyChanged);
			}
		}
	}

	/// <summary>
	/// Calls <see cref="ObservableCollection{T}.OnCollectionChanged"/>
	/// as the handler for whenever an item's property changes.
	/// </summary>
	private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		=> OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
}
