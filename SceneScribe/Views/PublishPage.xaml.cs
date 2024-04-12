using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SceneScribe.Engine;

namespace SceneScribe.Views
{
	/// <summary>
	/// The page containing the controls to publish the screenplay to different formats.
	/// </summary>
	public sealed partial class PublishPage : Page
	{
		public PublishPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			TmpTextBlock.Text = ((Screenplay)e.Parameter).SerializeToXML();
		}
	}
}
