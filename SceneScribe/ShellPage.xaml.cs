using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SceneScribe.ViewModels;
using SceneScribe.Views;
using System;

namespace SceneScribe
{
	public sealed partial class ShellPage : Page
	{
		/// <summary>
		/// The type that must be passed as a parameter when navigating to ShellPage.
		/// </summary>
		/// <param name="ViewModel">The view model to set.</param>
		/// <param name="StartPage">The page to navigate to by default.</param>
		public record OnNavigatedToParameter(ShellPageViewModel ViewModel, Type StartPage);

		public ShellPageViewModel ViewModel { get; private set; }

		public ShellPage()
		{
			this.InitializeComponent();

			// Register top menu button handlers
			BtnHome.Click += (sender, e) => SetActiveTab(typeof(HomePage));
			BtnEditor.Click += (sender, e) => SetActiveTab(typeof(EditorPage));
			BtnPublish.Click += (sender, e) => SetActiveTab(typeof(PublishPage));
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			// Load the view model and default page
			(ViewModel, var page) = e.Parameter as OnNavigatedToParameter;
			SetActiveTab(page);
		}

		/// <summary>
		/// Sets the active page to the specified <paramref name="page"/>.
		/// </summary>
		/// <param name="page">The page to set as active.</param>
		/// <exception cref="ArgumentException">Thrown when the <paramref name="page"/> is not a valid tab.</exception>
		public void SetActiveTab(Type page)
		{
			// Make all the buttons inactive (removes the active state from the currently active button)
			BtnHome.Active = BtnEditor.Active = BtnPublish.Active = false;

			// Mark the appropriate button as active and navigate to the corresponding page
			if (page == typeof(HomePage))
			{
				BtnHome.Active = true;
				var vm = new HomePageViewModel
				{
					ShellPage = this,
					ActiveGroupPath = new(new[] { ViewModel.Config.GroupBrowserRoot })
				};
				ContentFrame.Navigate(typeof(HomePage), vm);
			}
			else if (page == typeof(EditorPage))
			{
				BtnEditor.Active= true;
				var vm = new EditorPageViewModel
				{
					ShellPage = this,
					ActiveScreenplay = ViewModel.ActiveScreenplay
				};
				ContentFrame.Navigate(typeof(EditorPage), vm);
			}
			else if (page == typeof(PublishPage))
			{
				BtnPublish.Active = true;
				var vm = new PublishPageViewModel
				{
					ShellPage = this,
					ActiveScreenplay = ViewModel.ActiveScreenplay
				};
				ContentFrame.Navigate(typeof(PublishPage), vm);
			}
			else
			{
				throw new ArgumentException($"{nameof(page)} is not a valid page.");
			}
		}

		/// <summary>
		/// Sets the navbar content to a given <see cref="UIElement"/>.
		/// </summary>
		/// <param name="content">The element to render in the navbar.</param>
		public void SetNavbarCenterContent(UIElement content) => NavbarCenterContent.Content = content;

		/// <summary>
		/// The callback for pressing the theme button.
		/// Toggles Scene Scribe between light and dark mode.
		/// </summary>
		private void ThemeToggle(object sender, RoutedEventArgs e)
		{
			this.RequestedTheme = RequestedTheme == ElementTheme.Dark
				? ElementTheme.Light : ElementTheme.Dark;
		}
	}
}
