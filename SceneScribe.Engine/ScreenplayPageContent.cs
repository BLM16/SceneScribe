namespace SceneScribe.Engine;

/// <summary>
/// Contains the content for a page of a screenplay.
/// </summary>
public class ScreenplayPageContent
{
    /// <summary>
    /// The list of components on the page.
    /// </summary>
    public List<ScreenplayComponent> Components { get; init; } = new();
}
