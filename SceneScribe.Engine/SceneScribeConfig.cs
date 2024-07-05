using SceneScribe.Engine.Helpers;

namespace SceneScribe.Engine;

/// <summary>
/// Contains Scene Scribe configuration logic.
/// </summary>
public sealed class SceneScribeConfig
{
	/// <summary>
	/// The base path for all Scene Scribe data.
	/// </summary>
	private static readonly string _sceneScribeDataPath = Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SceneScribe");

	/// <summary>
	/// The path for the GroupBrowser config.
	/// </summary>
	private static readonly string _groupBrowserConfigPath = Path.Combine(_sceneScribeDataPath, "GroupBrowserConfig.xml");

	private SceneScribeGroup? _groupBrowserRoot = null;

	/// <summary>
	/// The root <see cref="SceneScribeGroup"/> for the GroupBrowswer.
	/// </summary>
	public SceneScribeGroup GroupBrowserRoot
		=> _groupBrowserRoot ??= XmlHelper.DeserializeFile<SceneScribeGroup>(_groupBrowserConfigPath);

	/// <summary>
	/// The default path for where to save new projects.
	/// </summary>
	public static readonly string DefaultScreenplaySavePath = Path.Combine(_sceneScribeDataPath, "Screenplays");

    public SceneScribeConfig()
    {
		// Create the required files and directories if they don't exist
		FileHelper.CreateFileIfNotExists(_groupBrowserConfigPath, XmlHelper.Serialize(new SceneScribeGroup()));
		FileHelper.CreateDirectoryIfNotExists(DefaultScreenplaySavePath);

		// Setup a listener to auto-save config changes to the group browser
		GroupBrowserRoot.PropertyChanged += (sender, e) => FileHelper.SaveAsXML(_groupBrowserConfigPath, GroupBrowserRoot);
    }
}
