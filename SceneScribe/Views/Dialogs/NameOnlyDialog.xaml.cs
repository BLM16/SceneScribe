using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SceneScribe.Strings;

namespace SceneScribe.Views.Dialogs
{
	public sealed partial class NameOnlyDialog : ContentDialog
	{
		/// <summary>
		/// The placeholder text for the name textbox.
		/// </summary>
		public string Placeholder { get; set; } = I18n.GetString("NameOnlyDialog/Placeholder");
		
		/// <summary>
		/// The default text for the name textbox.
		/// </summary>
		public string DefaultText { get; set; } = string.Empty;

		/// <summary>
		/// Gets the entered text from the name textbox.
		/// </summary>
		public string EnteredText => txtName.Text;

		public NameOnlyDialog()
		{
			this.InitializeComponent();
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);

			// Place cursor at end of text
			txtName.SelectionStart = txtName.Text.Length;
			txtName.SelectionLength = 0;
		}

		/// <summary>
		/// Toggles the state of the primary button, enabling it only when the name is not empty.
		/// </summary>
		private void txtName_TextChanged(object sender, TextChangedEventArgs e)
			=> IsPrimaryButtonEnabled = txtName.Text != string.Empty;
	}
}
