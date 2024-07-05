using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SceneScribe.ViewModels;

namespace SceneScribe.Views
{
	/// <summary>
	/// The page containing the controls to publish the screenplay to different formats.
	/// </summary>
	public sealed partial class PublishPage : Page
	{
		public PublishPageViewModel ViewModel { get; private set; }

		public PublishPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModel = e.Parameter as PublishPageViewModel;
			TmpTextBlock.Text = ViewModel.ActiveScreenplay.ToXML();
		}
	}
}
