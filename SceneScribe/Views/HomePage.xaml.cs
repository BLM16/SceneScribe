using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SceneScribe.Engine;
using SceneScribe.Engine.Helpers;
using SceneScribe.Strings;
using SceneScribe.ViewModels;
using SceneScribe.Views.Dialogs;
using System;
using System.IO;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SceneScribe.Views
{
	/// <summary>
	/// The page containing the controls to open a screenplay or create a new one.
	/// </summary>
	public sealed partial class HomePage : Page
	{
		public HomePageViewModel ViewModel { get; private set; }

		public HomePage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModel = e.Parameter as HomePageViewModel;
			RefreshCurrentGroupBrowser();
		}

		/// <summary>
		/// Navigates to a child group from the active group.
		/// </summary>
		/// <param name="group">The child group to navigate to.</param>
		public void NavigateToSubGroup(SceneScribeGroup group)
		{
			ViewModel.ActiveGroupPath.Push(group);
			RefreshCurrentGroupBrowser();
		}

		/// <summary>
		/// Refreshes the GroupBrowser to reload the navbar and the child groups and projects.
		/// </summary>
		private void RefreshCurrentGroupBrowser()
		{
			var pathNavbar = new TextBlock { VerticalAlignment = VerticalAlignment.Center };

			// Loop through all groups in the path (reverse since it is a stack, skip the root group)
			// Create a link to navigate back to the corresponding group
			foreach (var (group, i) in ViewModel.ActiveGroupPath.Reverse().Skip(1).Select((g, i) => (g, i)))
			{
				var link = new Hyperlink { UnderlineStyle = UnderlineStyle.None };
				link.Inlines.Add(new Run { Text = group.Name });
				link.Click += (sender, e) =>
				{
					// Calculates the number of groups that need to be popped so the target is active
					var timesToPop = ViewModel.ActiveGroupPath.TakeWhile(g => g != group).Count();
					for (int j = 0; j < timesToPop; j++)
					{
						ViewModel.ActiveGroupPath.Pop();
					}
					
					RefreshCurrentGroupBrowser();
				};

				// Add the link to the navbar
				pathNavbar.Inlines.Add(new Run { Text = " > " });
				pathNavbar.Inlines.Add(link);
            }

			// Create a link to the root group
			var hb = new HyperlinkButton
			{
				Padding = new Thickness(0),
				Content = new FontIcon
				{
					Glyph = "\uE80F",
					FontSize = 14,
					FontFamily = this.Resources["SymbolThemeFontFamily"] as FontFamily
				}
			};
			hb.Click += NavigateToRootGroup_Click;

			// Place the root link and other links inline
			var sp = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			sp.Children.Add(hb);
			sp.Children.Add(pathNavbar);

			// TODO: Ensure the max width doesn't exceed the column width with long paths (enable scroll?)
			// Set the navbar content to contain the links
			ViewModel.ShellPage.SetNavbarCenterContent(sp);

			// Set the GroupBrowser content to show the active group
			CurrentGroupBrowser.Navigate(typeof(GroupBrowser), new GroupBrowserViewModel
			{
				ShellPage = ViewModel.ShellPage,
				HomePage = this,
				ActiveGroup = ViewModel.ActiveGroup
			});
		}

		/// <summary>
		/// Navigates to the root group.
		/// </summary>
		private void NavigateToRootGroup_Click(object sender, RoutedEventArgs e)
		{
			// Need to pop all groups except the root group
			var timesToPop = ViewModel.ActiveGroupPath.Count - 1;
			for (int i = 0; i < timesToPop; i++)
			{
				ViewModel.ActiveGroupPath.Pop();
			}

			RefreshCurrentGroupBrowser();
		}
		
		/// <summary>
		/// Opens a file from disk as a screenplay.
		/// </summary>
		private async void OpenOther_Click(object sender, RoutedEventArgs e)
		{
			var picker = new FileOpenPicker
			{
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
				ViewMode = PickerViewMode.List
			};
			picker.FileTypeFilter.Add(".sscrproj");
			
			// Associate the FileOpenPicker with the current window
			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Window);
			WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

			// Await the user's choice
			var fileToOpen = await picker.PickSingleFileAsync();

			// User cancelled instead of selecting file
			if (fileToOpen is null) return;

			try
			{
				// Load the screenplay from the chosen file
				var screenplay = Screenplay.FromXML(fileToOpen.Path);

				// Add the screenplay to the active group
				ViewModel.ActiveGroup.Projects.Add(new()
				{
					Name = screenplay.Title,
					FilePath = fileToOpen.Path
				});
			}
			catch
			{
				// Create a dialog to inform user the file could not be loaded
				var dlg = new ContentDialog
				{
					Title = I18n.GetString("DialogFileLoadError/Title"),
					Content = I18n.GetString("DialogFileLoadError/Content"),
					CloseButtonText = I18n.GetString("DialogFileLoadError/CloseButtonText"),
					DefaultButton = ContentDialogButton.Close,
					XamlRoot = XamlRoot
				};
				await dlg.ShowAsync();
			}
		}

		/// <summary>
		/// Prompts the user for a project name and filepath and creates
		/// a new group from them. Adds the project to the active group.
		/// </summary>
		private async void NewProject_Click(object sender, RoutedEventArgs e)
		{
			// Create a dialog to get the new project information
			var dlgNewProject = new NewProjectDialog(SceneScribeConfig.DefaultScreenplaySavePath) { XamlRoot = XamlRoot };
			var dlgNewProjectResult = await dlgNewProject.ShowAsync();

			// User cancelled the creation
			if (dlgNewProjectResult != ContentDialogResult.Primary) return;

			var (projectName, projectDir) = (dlgNewProject.ProjectName, dlgNewProject.ProjectFolder);
			var filename = $"{projectName}.sscrproj";
			var filePath = Path.Combine(projectDir.Path, filename);

			// Create the project file and save an empty screenplay to it
			// Must create file this way rather than with FileHelper due to UWP sandbox restrictions
			await projectDir.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
			FileHelper.SaveAsXML(filePath, new Screenplay { Title = projectName });

			// Add the project to the active group
			ViewModel.ActiveGroup.Projects.Add(new()
			{
				Name = projectName,
				FilePath = filePath
			});
		}

		/// <summary>
		/// Prompts the user for a group name and adds it to the active group.
		/// </summary>
		private async void NewGroup_Click(object sender, RoutedEventArgs e)
		{
			// Create a dialog to get the group name
			var dlgNewGroup = new NameOnlyDialog
			{
				Title = I18n.GetString("DialogNewGroup/Title"),
				Placeholder = I18n.GetString("DialogNewGroup/Placeholder"),
				CloseButtonText = I18n.GetString("DialogNewGroup/CloseButtonText"),
				PrimaryButtonText = I18n.GetString("DialogNewGroup/PrimaryButtonText"),
				DefaultButton = ContentDialogButton.Primary,
				XamlRoot = XamlRoot
			};
			var dlgNewGroupResult = await dlgNewGroup.ShowAsync();

			// User cancelled the creation
			if (dlgNewGroupResult != ContentDialogResult.Primary) return;

			// Add the group to the active group
			ViewModel.ActiveGroup.Groups.Add(new()
			{
				Name = dlgNewGroup.EnteredText
			});
		}
	}
}
