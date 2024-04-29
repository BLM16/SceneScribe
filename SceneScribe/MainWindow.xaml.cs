using Microsoft.UI.Xaml;

namespace SceneScribe
{
	/// <summary>
	/// The main window content that handles pagination for Scene Scribe.
	/// Solely renders <see cref="ShellPage"/>.
	/// </summary>
	public sealed partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();

			AppWindow.Title = "Scene Scribe";
			AppWindow.SetIcon(@"Assets\FeatherAppIcon.ico");

			ShellPage.Navigate(typeof(ShellPage));
		}
	}
}
