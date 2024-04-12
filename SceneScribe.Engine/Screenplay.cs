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
	/// The list of pages in the screenplay.
	/// </summary>
    public List<ScreenplayPageContent> Pages { get; init; } = new();

	/// <summary>
	/// Serializes the screenplay into XML to be saved to a file.
	/// </summary>
	/// <returns>An XML document as a string.</returns>
    public string SerializeToXML()
    {
		using var sw = new StringWriter();
		var xml = new XmlSerializer(typeof(Screenplay));

		xml.Serialize(sw, this);

		return sw.ToString();
	}
}
