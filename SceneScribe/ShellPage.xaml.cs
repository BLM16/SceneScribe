using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SceneScribe.Engine;
using SceneScribe.ViewModels;
using SceneScribe.Views;
using System;
using System.IO;
using System.Reflection;

namespace SceneScribe
{
	public sealed partial class ShellPage : Page
	{
		public ShellPageViewModel ViewModel { get; private set; }

		public ShellPage()
		{
			this.InitializeComponent();
			ViewModel = new ShellPageViewModel
			{
				ActiveScreenplay = TestScreenplay
			};

			// Register top menu button handlers
			BtnHome.Click += (sender, e) => SetActiveTab(typeof(HomePage));
			BtnEditor.Click += (sender, e) => SetActiveTab(typeof(EditorPage));
			BtnPublish.Click += (sender, e) => SetActiveTab(typeof(PublishPage));
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			// Load the home page initially
			SetActiveTab(typeof(HomePage));
		}

		/// <summary>
		/// Sets the active page to the specified <paramref name="page"/>.
		/// </summary>
		/// <param name="page">The page to set as active.</param>
		/// <exception cref="ArgumentException">Thrown when the <paramref name="page"/> is not a valid tab.</exception>
		private void SetActiveTab(Type page)
		{
			// Make all the buttons inactive (removes the active state from the currently active button)
			BtnHome.Active = BtnEditor.Active = BtnPublish.Active = false;

			// Mark the appropriate button as active and navigate to the corresponding page
			if (page == typeof(HomePage))
			{
				BtnHome.Active = true;
				ContentFrame.Navigate(typeof(HomePage));
			}
			else if (page == typeof(EditorPage))
			{
				BtnEditor.Active= true;
				ContentFrame.Navigate(typeof(EditorPage), ViewModel.ActiveScreenplay);
			}
			else if (page == typeof(PublishPage))
			{
				BtnPublish.Active = true;
				ContentFrame.Navigate(typeof(PublishPage), ViewModel.ActiveScreenplay);
			}
			else
			{
				throw new ArgumentException($"{nameof(page)} is not a valid page.");
			}
		}

		/// <summary>
		/// The callback for pressing the theme button.
		/// Toggles Scene Scribe between light and dark mode.
		/// </summary>
		private void ThemeToggle(object sender, RoutedEventArgs e)
		{
			this.RequestedTheme = RequestedTheme == ElementTheme.Dark
				? ElementTheme.Light : ElementTheme.Dark;
		}

		private static Screenplay TestScreenplay
			=> Screenplay.FromXML(Path.Combine(
					Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
					"TestScreenplay.xml"));
	}
}
