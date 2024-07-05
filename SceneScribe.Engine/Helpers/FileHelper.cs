namespace SceneScribe.Engine.Helpers;

/// <summary>
/// Contains methods to help with common file operations for Scene Scribe.
/// </summary>
public static class FileHelper
{
	/// <summary>
	/// Creates the directory and its parents if they don't exist.
	/// </summary>
	/// <remarks>
	/// If <paramref name="path"/> is a directory it will create it.
	/// If <paramref name="path"/> is a file it will create the parent directory.
	/// </remarks>
	/// <param name="path">The path to create the directories for.</param>
	public static void CreateDirectoryIfNotExists(string path)
	{
		// If path is a directory, create that directory
		if (Path.GetExtension(path) == string.Empty)
		{
			Directory.CreateDirectory(path);
			return;
		}

		// If path is a file, create the parent directory
		if (Path.GetDirectoryName(path) is string dir)
		{
			Directory.CreateDirectory(dir);
			return;
		}
	}

	/// <summary>
	/// Creates the directories and given file if it does not exist.
	/// </summary>
	/// <param name="path">The path of the file to create.</param>
	public static void CreateFileIfNotExists(string path)
	{
		if (File.Exists(path)) return;

		CreateDirectoryIfNotExists(path);
		using var _ = File.Create(path);
	}

	/// <summary>
	/// Creates the directories and given file if it does not exist.
	/// Saves <paramref name="defaultFileContents"/> to the new file if it was created.
	/// </summary>
	/// <param name="path"></param>
	/// <param name="defaultFileContents"></param>
	public static void CreateFileIfNotExists(string path, string defaultFileContents)
	{
		if (File.Exists(path)) return;

		CreateFileIfNotExists(path);
		File.WriteAllText(path, defaultFileContents);
	}

	/// <summary>
	/// Serializes <paramref name="obj"/> to XML and saves it to <paramref name="path"/>.
	/// </summary>
	/// <remarks>
	/// Creates the file if it does not exist.
	/// Overwrites the file with the serialized content.
	/// </remarks>
	/// <typeparam name="T"><c><see langword="typeof"/>(<paramref name="obj"/>)</c></typeparam>
	/// <param name="path">The path of the file to save to.</param>
	/// <param name="obj">The object to serialize and save.</param>
	public static void SaveAsXML<T>(string path, T obj)
	{
		CreateFileIfNotExists(path);

		using var writer = new FileStream(path, FileMode.Truncate, FileAccess.Write);
		XmlHelper.Serialize(obj, writer);
	}
}
