using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SceneScribe.Views.Dialogs
{
	public sealed partial class NewProjectDialog : ContentDialog
	{
		/// <summary>
		/// Gets the name of the project from the corresponding textbox.
		/// </summary>
		public string ProjectName => txtProjectName.Text;

		/// <summary>
		/// The selected folder to save the project to.
		/// </summary>
		public StorageFolder ProjectFolder { get; private set; }

		public NewProjectDialog(string defaultPath)
		{
			this.InitializeComponent();

			// Set the default project path
			txtProjectFilePath.Text = defaultPath;
			StorageFolder.GetFolderFromPathAsync(defaultPath).AsTask()
						 .ContinueWith(folder => ProjectFolder = folder.Result);
		}

		/// <summary>
		/// Opens the folder picker for the project file save path.
		/// Places the selected folder path into the corresponding textbox.
		/// </summary>
		private async void FolderPicker_Click(object sender, RoutedEventArgs e)
		{
			var picker = new FolderPicker
			{
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
				ViewMode = PickerViewMode.List
			};
			picker.FileTypeFilter.Add("*");

			// Associate the FolderPicker with the current window
			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Window);
			WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

			// Await the user's choice
			ProjectFolder = await picker.PickSingleFolderAsync();

			// User cancelled instead of selecting folder
			if (ProjectFolder is null) return;

			// Place the selected path into the corresponding textbox
			txtProjectFilePath.Text = ProjectFolder.Path;
        }

		/// <summary>
		/// Toggles the state of the primary button, enabling it only when the project name is not empty.
		/// </summary>
		private void txtProjectName_TextChanged(object sender, TextChangedEventArgs e)
			=> IsPrimaryButtonEnabled = txtProjectName.Text != string.Empty;

		/// <summary>
		/// Scrolls the project file path textbox to the end when the text changes.
		/// </summary>
		/// <remarks>
		/// Adapted from <see href="https://stackoverflow.com/questions/40114620/uwp-c-sharp-scroll-to-the-bottom-of-textbox"/>.
		/// </remarks>
		private void txtProjectFilePath_TextChanged(object sender, TextChangedEventArgs e)
		{
			// Get the TextBox internal grid child
			var grid = (Grid)VisualTreeHelper.GetChild(txtProjectFilePath, 0);

			// Loop through all the grid's children to find the ScrollViewer element
			for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
			{
				if (VisualTreeHelper.GetChild(grid, i) is ScrollViewer viewer)
				{
					// Force the ScrollViewer to scroll to the textbox width
					viewer.ChangeView(viewer.ExtentWidth, 0, 1.0f, true);
					break;
				}
			}
		}
	}
}
