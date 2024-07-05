using System.Collections.ObjectModel;
using System.Xml.Serialization;
using SceneScribe.Engine.Helpers;

namespace SceneScribe.Engine;

/// <summary>
/// Contains all the information about a screenplay.
/// </summary>
[XmlType("Screenplay")]
public class Screenplay
{
	/// <summary>
	/// The screenplay's title.
	/// </summary>
    public string Title { get; init; } = "Untitled Screenplay";

	/// <summary>
	/// The list of components in the screenplay.
	/// </summary>
    public ObservableCollection<ScreenplayComponent> Components { get; init; } = new();

	/// <summary>
	/// Serializes the screenplay into XML to be saved to a file.
	/// </summary>
	/// <returns>An XML document as a string.</returns>
	public string ToXML() => XmlHelper.Serialize(this);

	/// <summary>
	/// Deserializes the screenplay from an XML file.
	/// </summary>
	/// <param name="path">The XML document file path to deserialize.</param>
	/// <returns>A <see cref="Screenplay"/> representing the XML document.</returns>
	public static Screenplay? FromXML(string path) => XmlHelper.DeserializeFile<Screenplay>(path);
}
