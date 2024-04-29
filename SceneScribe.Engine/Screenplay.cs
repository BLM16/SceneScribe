using System.Xml.Serialization;

namespace SceneScribe.Engine;

/// <summary>
/// Contains all the information about a screenplay.
/// </summary>
[Serializable]
public class Screenplay
{
	/// <summary>
	/// The screenplay's title.
	/// </summary>
    public string Title { get; init; } = "Untitled Screenplay";

	/// <summary>
	/// The list of components in the screenplay.
	/// </summary>
    public List<ScreenplayComponent> Components { get; init; } = new();

	/// <summary>
	/// Serializes the screenplay into XML to be saved to a file.
	/// </summary>
	/// <returns>An XML document as a string.</returns>
    public string ToXML()
    {
		using var sw = new StringWriter();
		var serializer = new XmlSerializer(typeof(Screenplay));

		serializer.Serialize(sw, this);

		return sw.ToString();
	}

	/// <summary>
	/// Deserializes the screenplay from an XML file.
	/// </summary>
	/// <param name="path">The XML document file path to deserialize.</param>
	/// <returns>A <see cref="Screenplay"/> representing the XML document.</returns>
	public static Screenplay? FromXML(string path)
	{
		using var sw = new StreamReader(path);
		var serializer = new XmlSerializer(typeof(Screenplay));

		return serializer.Deserialize(sw) as Screenplay;
	}
}
