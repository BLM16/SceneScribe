using System.Text;
using System.Xml.Serialization;

namespace SceneScribe.Engine.Helpers;

/// <summary>
/// Contains methods to help with common XML tasks for Scene Scribe.
/// </summary>
internal static class XmlHelper
{
	/// <summary>
	/// Serializes <paramref name="obj"/> into its XML representation.
	/// </summary>
	/// <typeparam name="T"><c><see langword="typeof"/>(<paramref name="obj"/>)</c></typeparam>
	/// <param name="obj">The object to serialize.</param>
	/// <returns>A string containing the XML contents.</returns>
	public static string Serialize<T>(T obj)
    {
        using var writer = new MemoryStream();
        Serialize(obj, writer);
        return Encoding.UTF8.GetString(writer.ToArray());
    }

	/// <summary>
	/// Serializes <paramref name="obj"/> into its XML representation
	/// and writes the XML content to the <paramref name="writer"/>.
	/// </summary>
	/// <typeparam name="T"><c><see langword="typeof"/>(<paramref name="obj"/>)</c></typeparam>
	/// <param name="obj">The object to serialize.</param>
	/// <param name="writer">The <see cref="Stream"/> to write the XML content to.</param>
	public static void Serialize<T>(T obj, Stream writer)
    {
        var serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(writer, obj);
    }

	/// <summary>
	/// Deserializes XML content from a file at the given <paramref name="path"/>.
	/// </summary>
	/// <typeparam name="T">The type of object represented by the file.</typeparam>
	/// <param name="path">The path to the XML file.</param>
	/// <returns>An object of type <typeparamref name="T"/> representing the given XML file.</returns>
	public static T DeserializeFile<T>(string path)
    {
        using var reader = new StreamReader(path);
        var serializer = new XmlSerializer(typeof(T));
        return (T)serializer.Deserialize(reader)!;
    }
}
