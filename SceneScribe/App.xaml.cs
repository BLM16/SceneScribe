using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using SceneScribe.Engine;
using SceneScribe.Views;
using System.IO;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace SceneScribe
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public partial class App : Application
	{
		public static MainWindow Window { get; private set; }

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when the application is launched.
		/// </summary>
		/// <param name="args">Details about the launch request and process.</param>
		protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
		{
			Window = new MainWindow();

			// Setup the app based on how it was launched
			var activationArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
			switch (activationArgs.Kind)
			{
				case ExtendedActivationKind.File
				when activationArgs.Data is IFileActivatedEventArgs fileArgs:
					HandleLaunchFromFile(fileArgs);
					break;

				default:
					HandleDefaultLaunch();
					break;
			}

			Window.Activate();
		}

		/// <summary>
		/// Initializes the app for a default launch.
		/// Opens the app to the <see cref="HomePage"/>.
		/// </summary>
		private void HandleDefaultLaunch()
		{
			Window.Initialize(new()
			{
				Config = new(),
				ActiveScreenplay = new()
			}, typeof(HomePage));
		}

		/// <summary>
		/// Initializes the app for a launch from a file.
		/// Sets the view based on what file was launched.
		/// </summary>
		private void HandleLaunchFromFile(IFileActivatedEventArgs args)
		{
			if (args.Files[0] is not IStorageFile file) return;

			if (Path.GetExtension(file.Path) == ".sscrproj")
			{
				try
				{
					// Load the screenplay from the launched file
					var screenplay = Screenplay.FromXML(file.Path);

					// Set the window's active screenplay
					Window.Initialize(new()
					{
						Config = new(),
						ActiveScreenplay = screenplay
					}, typeof(EditorPage));
				}
				catch
				{
					// Could not load the screenplay so default to normal app launch
					HandleDefaultLaunch();
				}
			}
			else
			{
				// File type was not recognized so default to normal app launch
				HandleDefaultLaunch();
			}
		}
	}
}
