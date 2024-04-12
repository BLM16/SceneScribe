using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SceneScribe.Engine;
using SceneScribe.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace SceneScribe.Views
{
	/// <summary>
	/// The page containing the actual script editor.
	/// </summary>
	public sealed partial class EditorPage : Page
	{
		public EditorViewModel ViewModel { get; private set; }

		public EditorPage()
		{
			this.InitializeComponent();
			ViewModel = new EditorViewModel
			{
				Pages = new()
			};
			ViewModel.Pages.CollectionChanged += (sender, e) => RenderPages();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModel.Pages = new ObservableCollection<ScreenplayPageContent>((e.Parameter as Screenplay).Pages);
			RenderPages();
		}

		/// <summary>
		/// Render all the screenplay's pages as pages in the list view.
		/// </summary>
		private void RenderPages()
		{
			const double canvasWidth = 850;
			const double canvasHeight = 1100;

			var sceneCount = 0;

			foreach (var (page, i) in ViewModel.Pages.Select((p, i) => (p, i + 1)))
			{
				// The actual page element
				var pageElement = new StackPanel
				{
					Background = new SolidColorBrush { Color = Colors.White },
					Width = canvasWidth,
					MinWidth = canvasWidth,
					MaxWidth = canvasWidth,
					Height = canvasHeight,
					MinHeight = canvasHeight,
					MaxHeight = canvasHeight,
					Padding = new Thickness(0, 50, 0, 100)
				};

				// The page number in the top right
				var pageNumElem = new TextBlock
				{
					Text = $"{i}.",
					Foreground = new SolidColorBrush { Color = Colors.Black },
					TextAlignment = TextAlignment.End,
					FontSize = 16, // 12pt == 16px
					Margin = new Thickness(0, 0, 130, 50)
				};
				pageElement.Children.Add(pageNumElem);

				foreach (var component in page.Components)
				{
					// The element to render the component
					var elem = new TextBlock
					{
						Text = component.Text,
						Foreground = new SolidColorBrush { Color = Colors.Black },
						MaxWidth = canvasWidth - component.MarginLeft - component.MarginRight,
						TextWrapping = TextWrapping.Wrap,
						TextAlignment = component.IsRightAligned ? TextAlignment.End : TextAlignment.Start,
						FontSize = 16, // 12pt == 16px
						Margin = new Thickness(component.MarginLeft, component.MarginTop, component.MarginRight, component.MarginBottom),
						Padding = new Thickness(0)
					};

					// Scene headings need scene numbers and get rendered differently
					if (component.Type == ScreenplayComponentType.SceneHeading)
					{
						// The canvas 
						var sceneHeading = new Canvas()
						{
							Height = 16, // 12pt == 16px (set the height to the line height of elem)
							Margin = new Thickness(0, component.MarginTop, 0, component.MarginBottom)
						};

						// The outer x-margins for the scene numbers
						const double xMargin = 100;

						// Remove the top and bottom margins as the sceneHeading canvas accounts for them
						elem.Margin = new Thickness(component.MarginLeft, 0, component.MarginRight, 0);

						// The left scene number
						var leftNum = new TextBlock
						{
							Text = (++sceneCount).ToString(),
							Foreground = new SolidColorBrush { Color = Colors.Black },
							TextAlignment = TextAlignment.Start,
							FontSize = 16, // 12pt == 16px
							Margin = new Thickness(xMargin, 0, 0, 0)
						};

						// The right scene number
						var rightNum = new TextBlock
						{
							Text = sceneCount.ToString(),
							Foreground = new SolidColorBrush { Color = Colors.Black },
							SelectionHighlightColor = new SolidColorBrush { Color = Colors.Green },
							Width = canvasWidth - xMargin,
							TextAlignment = TextAlignment.End,
							FontSize = 16, // 12pt == 16px
							Margin = new Thickness(0, 0, xMargin, 0)
						};

						sceneHeading.Children.Add(leftNum);
						sceneHeading.Children.Add(elem);
						sceneHeading.Children.Add(rightNum);

						pageElement.Children.Add(sceneHeading);
					}
					else
					{
						pageElement.Children.Add(elem);
					}
				}

				// Adds a small vertical space between pages
				var stack = new StackPanel();
				stack.Children.Add(new Border { Height = 4 });
				stack.Children.Add(pageElement);
				stack.Children.Add(new Border { Height = 4 });

				DocumentListView.Items.Add(new ListViewItem { Content = stack });
			}
		}
	}
}
