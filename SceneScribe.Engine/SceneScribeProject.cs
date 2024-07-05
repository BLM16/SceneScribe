using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace SceneScribe.Engine;

/// <summary>
/// Contains all the information about a Scene Scribe project.
/// </summary>
[XmlType("Project")]
public class SceneScribeProject : INotifyPropertyChanged
{
	/// <summary>
	/// The project's ID.
	/// </summary>
	[XmlAttribute]
	public Guid ID { get; init; } = Guid.NewGuid();

	private string _name = "Untitled Screenplay";

	/// <summary>
	/// The project's name.
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
	/// The path to the saved project file.
	/// </summary>
	[XmlAttribute("Path")]
	public string FilePath { get; init; } = Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
		"SceneScribe");

	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		=> PropertyChanged?.Invoke(this, new(propertyName));
}
