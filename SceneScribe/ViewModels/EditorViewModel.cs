using SceneScribe.Engine;
using System.Collections.ObjectModel;

namespace SceneScribe.ViewModels;

public class EditorViewModel
{
	public ObservableCollection<ScreenplayPageContent> Pages { get; set; }
}
