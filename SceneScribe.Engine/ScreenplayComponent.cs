using System.Xml.Serialization;

namespace SceneScribe.Engine;

/// <summary>
/// The different screenplay component types.
/// </summary>
public enum ScreenplayComponentType
{
	SceneHeading,
	SubHeading,
	Action,
	Character,
	Dialogue,
	Parenthetical,
	Transition,
	Directive
}

/// <summary>
/// Contains all the information about a component of a screenplay.
/// </summary>
[Serializable]
public class ScreenplayComponent
{
	/// <summary>
	/// The component type.
	/// </summary>
	[XmlAttribute]
	public ScreenplayComponentType Type { get; set; }

	/// <summary>
	/// The text content of the component.
	/// Capitalizes all the text accordingly for the component type on set.
	/// </summary>
	[XmlText]
	public string Text
	{
		get => _text;
		set => _text = Type switch
		{
			ScreenplayComponentType.SceneHeading or
			ScreenplayComponentType.SubHeading or
			ScreenplayComponentType.Character or
			ScreenplayComponentType.Transition or
			ScreenplayComponentType.Directive => value.ToUpper(),
			_ => value
		};
	}
	private string _text = string.Empty;

	/// <summary>
	/// Should the component be aligned to the right side of the page.
	/// </summary>
	public bool IsRightAligned => Type == ScreenplayComponentType.Transition;

	/// <summary>
	/// The left margin of the component on the page.
	/// </summary>
	public double MarginLeft => Type switch
	{
		ScreenplayComponentType.SceneHeading => 150,
		ScreenplayComponentType.SubHeading => 150,
		ScreenplayComponentType.Action => 150,
		ScreenplayComponentType.Character => 370,
		ScreenplayComponentType.Dialogue => 250,
		ScreenplayComponentType.Parenthetical => 310,
		ScreenplayComponentType.Transition => 0, // Right aligned and not based on margin
		ScreenplayComponentType.Directive => 150,

		_ => throw new NotImplementedException($"The component type `{nameof(Type)}` has not been implemented."),
	};

	/// <summary>
	/// The top margin of the component on the page.
	/// </summary>
	public double MarginTop => Type switch
	{
		ScreenplayComponentType.SceneHeading => 10,
		_ => 0
	};

	/// <summary>
	/// The right margin of the component on the page.
	/// </summary>
	public double MarginRight => Type switch
	{
		ScreenplayComponentType.SceneHeading => 100,
		ScreenplayComponentType.SubHeading => 100,
		ScreenplayComponentType.Action => 100,
		ScreenplayComponentType.Character => 100,
		ScreenplayComponentType.Dialogue => 250,
		ScreenplayComponentType.Parenthetical => 340,
		ScreenplayComponentType.Transition => 100,
		ScreenplayComponentType.Directive => 100,

		_ => throw new NotImplementedException($"The component type `{nameof(Type)}` has not been implemented."),
	};

	/// <summary>
	/// The bottom margin of the component on the page.
	/// </summary>
	public double MarginBottom => Type switch
	{
		ScreenplayComponentType.SceneHeading => 20,
		ScreenplayComponentType.SubHeading => 12,
		ScreenplayComponentType.Action => 12,
		ScreenplayComponentType.Character => 0,
		ScreenplayComponentType.Dialogue => 12,
		ScreenplayComponentType.Parenthetical => 0,
		ScreenplayComponentType.Transition => 12,
		ScreenplayComponentType.Directive => 12,

		_ => throw new NotImplementedException($"The component type `{nameof(Type)}` has not been implemented.")
	};
}
