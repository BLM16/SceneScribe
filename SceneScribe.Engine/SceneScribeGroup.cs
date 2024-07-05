using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;

namespace SceneScribe.Engine;

/// <summary>
/// Acts as a directory containing Scene Scribe projects and sub-directories.
/// </summary>
[XmlType("Group")]
public class SceneScribeGroup : INotifyPropertyChanged
{
	/// <summary>
	/// The group's ID.
	/// </summary>
	[XmlAttribute]
	public Guid ID { get; init; } = Guid.NewGuid();

	private string _name = "Untitled Group";

	/// <summary>
	/// The group's name.
	/// </summary>
	[XmlAttribute]
	public string Name
	{
		get => _name;
		set
		{
			if (_name == value) return;

			_name = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// A list of the sub-groups in this group.
	/// </summary>
	public TrulyObservableCollection<SceneScribeGroup> Groups { get; init; } = new();

	/// <summary>
	/// A list of the projects in this group.
	/// </summary>
	public TrulyObservableCollection<SceneScribeProject> Projects { get; init; } = new();

	public SceneScribeGroup()
    {
		Groups.CollectionChanged += (sender, e) => OnPropertyChanged(sender, new PropertyChangedEventArgs(nameof(Groups)));
		Projects.CollectionChanged += (sender, e) => OnPropertyChanged(sender, new PropertyChangedEventArgs(nameof(Projects)));
    }

	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);
	private void OnPropertyChanged([CallerMemberName] string propertyName = "") => OnPropertyChanged(this, new(propertyName));
}
