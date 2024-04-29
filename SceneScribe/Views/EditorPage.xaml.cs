using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SceneScribe.Engine;
using SceneScribe.ViewModels;
using System.Collections.ObjectModel;

namespace SceneScribe.Views
{
	/// <summary>
	/// The page containing the actual script editor.
	/// </summary>
	public sealed partial class EditorPage : Page
	{
		public EditorViewModel ViewModel { get; private set; }

		private const double canvasWidth = 850;
		private const double canvasHeight = 1100;

		private const int lineHeight = 18; // A few extra pixels for any ascenders and descenders in the font
		private const int fontSize = 16; // 12pt == 16px

		private static readonly SolidColorBrush blackSolidColorBrush = new(Colors.Black);
		private static readonly SolidColorBrush whiteSolidColorBrush = new(Colors.White);

		public EditorPage()
		{
			this.InitializeComponent();
			ViewModel = new EditorViewModel
			{
				Components = new()
			};
			ViewModel.Components.CollectionChanged += (sender, e) => RenderPages();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModel.Components = new ObservableCollection<ScreenplayComponent>((e.Parameter as Screenplay).Components);
			RenderPages();
		}

		/// <summary>
		/// Renders all the screenplay's pages as pages in the document list view.
		/// </summary>
		private void RenderPages()
		{
			var pageNumber = 0;
			var sceneCount = 0;
			var nextComponentIndex = 0;

			while (nextComponentIndex < ViewModel.Components.Count)
			{
				var pageElement = RenderPage(ref pageNumber, ref sceneCount, ref nextComponentIndex);

				// Adds a small vertical space between pages
				var stack = new StackPanel();
				stack.Children.Add(new Border { Height = 4 });
				stack.Children.Add(pageElement);
				stack.Children.Add(new Border { Height = 4 });

				DocumentListView.Items.Add(new ListViewItem { Content = stack });
			}
		}

		/// <summary>
		/// Creates a <see cref="UIElement"/> to render a page with all the components that fit on it.
		/// </summary>
		/// <param name="pageNumber">This page number for this page.</param>
		/// <param name="sceneCount">The current number of scenes.</param>
		/// <param name="nextComponentIndex">The index of the next component to render.</param>
		/// <returns>A <see cref="UIElement"/> that represents a page.</returns>
		private UIElement RenderPage(ref int pageNumber, ref int sceneCount, ref int nextComponentIndex)
		{
			var currentPageHeight = 0;

			// The actual page element
			var pageElement = new StackPanel
			{
				Background = whiteSolidColorBrush,
				Width = canvasWidth,
				MinWidth = canvasWidth,
				MaxWidth = canvasWidth,
				Height = canvasHeight,
				MinHeight = canvasHeight,
				MaxHeight = canvasHeight,
				Padding = new Thickness(0, 50, 0, 100)
			};
			currentPageHeight += 50 + 100;

			// The page number in the top right
			var pageNumElem = new TextBlock
			{
				Text = $"{++pageNumber}.",
				Foreground = blackSolidColorBrush,
				TextAlignment = TextAlignment.End,
				FontSize = fontSize,
				Margin = new Thickness(0, 0, 130, 50)
			};
			pageElement.Children.Add(pageNumElem);
			currentPageHeight += 50;

			while (currentPageHeight < canvasHeight && nextComponentIndex < ViewModel.Components.Count)
			{
				// Post-increment nextComponentIndex ref to always point to the next component
				var component = ViewModel.Components[nextComponentIndex++];

				var spacingTop = new Border { Height = component.MarginTop };
				var spacingBtm = new Border { Height = component.MarginBottom };
				currentPageHeight += (int)component.MarginBottom + (int)component.MarginTop;

				var elem = new TextBox
				{
					Text = component.Text,
					MaxWidth = canvasWidth - component.MarginLeft - component.MarginRight,
					TextAlignment = component.IsRightAligned ? TextAlignment.Right : TextAlignment.Left,
					Margin = new Thickness(component.MarginLeft, 0, component.MarginRight, 0),
				};
				elem.Height = GetLineCount(elem) * lineHeight;
				currentPageHeight += (int)elem.Height;


				// Scene headings need scene numbers and get rendered differently
				UIElement uiElem = component.Type == ScreenplayComponentType.SceneHeading
					? RenderSceneHeading(elem, ++sceneCount)
					: elem;

				pageElement.Children.Add(spacingTop);
				pageElement.Children.Add(uiElem);
				pageElement.Children.Add(spacingBtm);
			}

			return pageElement;
		}

		/// <summary>
		/// Creates a <see cref="UIElement"/> to render a scene heading along with its corresponding
		/// scene numbers on the sides of the page.
		/// </summary>
		/// <remarks>
		/// Does not increment currentPageHeight as this just wraps the heading element.
		/// It is expected that the heading element has already dealt with it as this does
		/// not change the vertical size of the element.
		/// </remarks>
		/// <param name="heading">The text box containing the heading content.</param>
		/// <param name="sceneNumber">The current scene number to render.</param>
		/// <returns>A <see cref="UIElement"/> that represents the scene heading and scene numbers.</returns>
		private static UIElement RenderSceneHeading(UIElement heading, int sceneNumber)
		{
			// The outer x-margins for the scene numbers
			const double xMargin = 100;

			var sceneHeading = new Canvas
			{
				Height = lineHeight
			};

			var leftSceneNum = new TextBlock
			{
				Text = sceneNumber.ToString(),
				Foreground = blackSolidColorBrush,
				TextAlignment = TextAlignment.Start,
				FontSize = fontSize,
				Margin = new Thickness(xMargin, 0, 0, 0)
			};

			var rightSceneNum = new TextBlock
			{
				Text = sceneNumber.ToString(),
				Foreground = blackSolidColorBrush,
				Width = canvasWidth - xMargin,
				TextAlignment = TextAlignment.End,
				FontSize = fontSize,
				Margin = new Thickness(0, 0, xMargin, 0)
			};

			sceneHeading.Children.Add(leftSceneNum);
			sceneHeading.Children.Add(heading);
			sceneHeading.Children.Add(rightSceneNum);

			return sceneHeading;
		}

		/// <summary>
		/// Gets the number of lines that a text box takes up with its text wrapping.
		/// </summary>
		/// <param name="box">The text box to count the lines for.</param>
		/// <returns>The number of lines the text box occupies.</returns>
		private static int GetLineCount(TextBox box)
		{
			var charWidth = 10;
			var charCount = box.Text.Length;

			var neededHorizSpace = charWidth * charCount;
			var actualHorizSpace = box.MaxWidth;

			return (int)System.Math.Ceiling(neededHorizSpace / actualHorizSpace);
		}
	}
}
