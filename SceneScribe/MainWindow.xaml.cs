using Microsoft.UI.Xaml;
using SceneScribe.ViewModels;
using System;

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
		}

		/// <summary>
		/// Initializes the window by setting the view model
		/// and start page for the shell page.
		/// </summary>
		/// <param name="vm">The view model for the shell page.</param>
		/// <param name="pageType">The page to default to.</param>
		public void Initialize(ShellPageViewModel vm, Type pageType)
			=> ShellPage.Navigate(typeof(ShellPage),
				new ShellPage.OnNavigatedToParameter(vm, pageType));
	}
}
