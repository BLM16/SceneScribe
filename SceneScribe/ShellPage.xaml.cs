using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SceneScribe.Engine;
using SceneScribe.ViewModels;
using SceneScribe.Views;
using System;

namespace SceneScribe
{
	public sealed partial class ShellPage : Page
	{
		public ShellPageViewModel ViewModel { get; private set; }

		public ShellPage()
		{
			this.InitializeComponent();
			ViewModel = new ShellPageViewModel
			{
				ActiveScreenplay = TestScreenplay
			};

			// Register top menu button handlers
			BtnHome.Click += (sender, e) => SetActiveTab(typeof(HomePage));
			BtnEditor.Click += (sender, e) => SetActiveTab(typeof(EditorPage));
			BtnPublish.Click += (sender, e) => SetActiveTab(typeof(PublishPage));
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			// Load the home page initially
			SetActiveTab(typeof(HomePage));
		}

		/// <summary>
		/// Sets the active page to the specified <paramref name="page"/>.
		/// </summary>
		/// <param name="page">The page to set as active.</param>
		/// <exception cref="ArgumentException">Thrown when the <paramref name="page"/> is not a valid tab.</exception>
		private void SetActiveTab(Type page)
		{
			// Make all the buttons inactive (removes the active state from the currently active button)
			BtnHome.Active = BtnEditor.Active = BtnPublish.Active = false;

			// Mark the appropriate button as active and navigate to the corresponding page
			if (page == typeof(HomePage))
			{
				BtnHome.Active = true;
				ContentFrame.Navigate(typeof(HomePage));
			}
			else if (page == typeof(EditorPage))
			{
				BtnEditor.Active= true;
				ContentFrame.Navigate(typeof(EditorPage), ViewModel.ActiveScreenplay);
			}
			else if (page == typeof(PublishPage))
			{
				BtnPublish.Active = true;
				ContentFrame.Navigate(typeof(PublishPage), ViewModel.ActiveScreenplay);
			}
			else
			{
				throw new ArgumentException($"{nameof(page)} is not a valid page.");
			}
		}

		/// <summary>
		/// The callback for pressing the theme button.
		/// Toggles Scene Scribe between light and dark mode.
		/// </summary>
		private void ThemeToggle(object sender, RoutedEventArgs e)
		{
			this.RequestedTheme = RequestedTheme == ElementTheme.Dark
				? ElementTheme.Light : ElementTheme.Dark;
		}

		private readonly Screenplay TestScreenplay = new()
		{
			Title = "Example Screenplay",
			Pages = new()
			{
				new ScreenplayPageContent
				{
					Components = new ()
					{
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Directive,
							Text = "FADE IN:"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.SceneHeading,
							Text = "EXT. SCARY FOREST - NIGHT"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Action,
							Text = "It's dark and stormy. AMSHU sits on a rock in a forest, pondering existence contemplatively."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Action,
							Text = "Enter AARDVARK."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Character,
							Text = "AMSHU"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Dialogue,
							Text = "Ah! What devil is this to disturb my slumber? Cursed thou be'st!"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Character,
							Text = "AARDVARK"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Dialogue,
							Text = "Where once I was't in the company of the horn'ed one, our ways hath parted, ne'er to cross again."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Character,
							Text = "AMSHU"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Dialogue,
							Text = "Thou art a blight on the face of this earth I say. Thine stench offends mine nostrils."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Character,
							Text = "AARDVARK"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Dialogue,
							Text = "Thou art and evermore shall be, a bully to me."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Action,
							Text = "AARDVARK scuttles off screen in a sulk."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Character,
							Text = "AARDVARK (CONT'D) (O.S.)"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Dialogue,
							Text = "Ere the dawn, thou shalt perish."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Transition,
							Text = "FADE OUT."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Directive,
							Text = "FADE IN:"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.SceneHeading,
							Text = "INT. AMSHU'S BED - NIGHT"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Action,
							Text = "AMSHU is asleep, soundlessly and peacefully."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Action,
							Text = "Enter AARDVARK, carrying a needle in his mouth."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Character,
							Text = "AARDVARK"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Parenthetical,
							Text = "(Muttering)"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Dialogue,
							Text = "Thou shalt perish, thou shalt perish. Ere the dawn thou shalt."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Action,
							Text = "AARDVARK injects AMSHU who promptly dies."
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Transition,
							Text = "FADE OUT."
						},
					}
				},
				new ScreenplayPageContent
				{
					Components = new()
					{
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.SceneHeading,
							Text = "INT. SETTING 2 - DAY"
						},
						new ScreenplayComponent
						{
							Type = ScreenplayComponentType.Action,
							Text = "The aardvark scuttles left."
						},
					}
				},
			}
		};
	}
}
