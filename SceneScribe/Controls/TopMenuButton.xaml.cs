using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;

namespace SceneScribe.Controls
{
	/// <summary>
	/// A toggleable button to be used in Scene Scribe's top menu bar.
	/// </summary>
	[ContentProperty(Name = nameof(Text))]
	public sealed partial class TopMenuButton : UserControl
	{
		public TopMenuButton()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// The event that is fired when the button is clicked.
		/// </summary>
		public event RoutedEventHandler Click;

		private void button_Click(object sender, RoutedEventArgs e)
		{
			if (Click is not null)
				Click(sender, e);
		}

		/// <summary>
		/// The text content rendered on the button.
		/// </summary>
		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		/// <summary>
		/// The <see cref="DependencyProperty"/> backing field for <see cref="Text"/>.
		/// </summary>
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register(nameof(Text), typeof(string),
				typeof(TopMenuButton), new PropertyMetadata(string.Empty));

		/// <summary>
		/// Should the button be rendered in its active state.
		/// </summary>
		public bool Active
		{
			get => (bool)GetValue(ActiveProperty);
			set => SetValue(ActiveProperty, value);
		}

		/// <summary>
		/// The <see cref="DependencyProperty"/> backing field for <see cref="Active"/>.
		/// </summary>
		public static readonly DependencyProperty ActiveProperty =
			DependencyProperty.Register("Active", typeof(bool),
				typeof(TopMenuButton), new PropertyMetadata(false));
	}
}
