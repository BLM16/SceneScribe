using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SceneScribe.Engine;
using SceneScribe.Strings;
using SceneScribe.Views.Dialogs;
using System;
using System.Linq;

namespace SceneScribe.Views
{
	public sealed partial class GroupBrowser : Page
	{
		public GroupBrowserViewModel ViewModel { get; private set; }

		public GroupBrowser()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModel = e.Parameter as GroupBrowserViewModel;
		}

		/// <summary>
		/// Navigates the GroupBrowser to the clicked group.
		/// </summary>
		private void Group_Click(object sender, RoutedEventArgs e)
		{
			// Get the clicked group
			var groupID = (Guid)(sender as FrameworkElement).Tag;
			var group = ViewModel.ActiveGroup.Groups.First(g => g.ID == groupID);
			
			// Navigate the GroupBrowser to the clicked group
			ViewModel.HomePage.NavigateToSubGroup(group);
        }

		/// <summary>
		/// Loads the clicked project in the editor.
		/// </summary>
		private void Project_Click(object sender, RoutedEventArgs e)
		{
			// Get the clicked project
			var projectID = (Guid)(sender as FrameworkElement).Tag;
			var project = ViewModel.ActiveGroup.Projects.First(p => p.ID == projectID);

			// Load the screenplay and switch to the editor
			ViewModel.ShellPage.ViewModel.ActiveScreenplay = Screenplay.FromXML(project.FilePath);
			ViewModel.ShellPage.SetActiveTab(typeof(EditorPage));
		}

		/// <summary>
		/// Prompts the user for a new group name and renames the appropriate group.
		/// </summary>
		private async void RenameGroup_Click(object sender, RoutedEventArgs e)
		{
			// Get the group
			var groupID = (Guid)(sender as FrameworkElement).Tag;
			var group = ViewModel.ActiveGroup.Groups.First(g => g.ID == groupID);

			// Create a dialog to get the new group name
			var dlgRenameGroup = new NameOnlyDialog
			{
				Title = I18n.GetString("DialogRenameGroup/Title"),
				DefaultText = group.Name,
				Placeholder = I18n.GetString("DialogRenameGroup/Placeholder"),
				CloseButtonText = I18n.GetString("DialogRenameGroup/CloseButtonText"),
				PrimaryButtonText = I18n.GetString("DialogRenameGroup/PrimaryButtonText"),
				DefaultButton = ContentDialogButton.Primary,
				XamlRoot = XamlRoot
			};
			var dlgRenameGroupResult = await dlgRenameGroup.ShowAsync();

			// User cancelled the renaming
			if (dlgRenameGroupResult != ContentDialogResult.Primary) return;

			// Rename the group
			group.Name = dlgRenameGroup.EnteredText;
		}

		/// <summary>
		/// Removes the appropriate group from the GroupBrowser.
		/// Leaves the project files on the computer untouched.
		/// Prompts the user for confirmation.
		/// </summary>
		private async void RemoveGroup_Click(object sender, RoutedEventArgs e)
		{
			// Get the group
			var groupID = (Guid)(sender as FrameworkElement).Tag;
			var group = ViewModel.ActiveGroup.Groups.First(g => g.ID == groupID);

			// Create a dialog to confirm the removal
			var confirmDlg = new ContentDialog
			{
				Title = I18n.GetString("DialogRemoveGroup/Title"),
				Content = I18n.GetString("DialogRemoveGroup/Content"),
				CloseButtonText = I18n.GetString("DialogRemoveGroup/CloseButtonText"),
				PrimaryButtonText = I18n.GetString("DialogRemoveGroup/PrimaryButtonText"),
				DefaultButton = ContentDialogButton.Close,
				XamlRoot = XamlRoot
			};
			var confirmDlgResult = await confirmDlg.ShowAsync();

			// User cancelled the removal
			if (confirmDlgResult != ContentDialogResult.Primary) return;

			// Remove the group
			ViewModel.ActiveGroup.Groups.Remove(group);
		}

		/// <summary>
		/// Prompts the user for a new project name and renames the appropriate project.
		/// </summary>
		private async void RenameProject_Click(object sender, RoutedEventArgs e)
		{
			// Get the project
			var projectID = (Guid)(sender as FrameworkElement).Tag;
			var project = ViewModel.ActiveGroup.Projects.First(p => p.ID == projectID);

			// Create a dialog to get the new project name
			var dlgRenameProject = new NameOnlyDialog
			{
				Title = I18n.GetString("DialogRenameProject/Title"),
				DefaultText = project.Name,
				Placeholder = I18n.GetString("DialogRenameProject/Placeholder"),
				CloseButtonText = I18n.GetString("DialogRenameProject/CloseButtonText"),
				PrimaryButtonText = I18n.GetString("DialogRenameProject/PrimaryButtonText"),
				DefaultButton = ContentDialogButton.Primary,
				XamlRoot = XamlRoot
			};
			var dlgRenameProjectResult = await dlgRenameProject.ShowAsync();

			// User cancelled the renaming
			if (dlgRenameProjectResult != ContentDialogResult.Primary) return;

			// Rename the project
			project.Name = dlgRenameProject.EnteredText;

			// TODO: Consider changing filename and screenplay title
		}

		/// <summary>
		/// Removes the appropriate project from the GroupBrowser.
		/// Leaves the project file on the computer untouched.
		/// Prompts the user for confirmation.
		/// </summary>
		private async void RemoveProject_Click(object sender, RoutedEventArgs e)
		{
			// Get the project
			var projectID = (Guid)(sender as FrameworkElement).Tag;
			var project = ViewModel.ActiveGroup.Projects.First(p => p.ID == projectID);

			// Create a dialog to confirm the removal
			var confirmDlg = new ContentDialog
			{
				Title = I18n.GetString("DialogRemoveProject/Title"),
				Content = I18n.GetString("DialogRemoveProject/Content"),
				CloseButtonText = I18n.GetString("DialogRemoveProject/CloseButtonText"),
				PrimaryButtonText = I18n.GetString("DialogRemoveProject/PrimaryButtonText"),
				DefaultButton = ContentDialogButton.Close,
				XamlRoot = XamlRoot
			};
			var confirmDlgResult = await confirmDlg.ShowAsync();

			// User cancelled the removal
			if (confirmDlgResult != ContentDialogResult.Primary) return;

			// Remove the project
			ViewModel.ActiveGroup.Projects.Remove(project);
		}
	}
}
