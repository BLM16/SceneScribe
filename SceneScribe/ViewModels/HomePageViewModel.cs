using SceneScribe.Engine;
using System.Collections.Generic;

namespace SceneScribe.ViewModels
{
    public class HomePageViewModel
    {
        public ShellPage ShellPage { get; set; }

        public Stack<SceneScribeGroup> ActiveGroupPath { get; set; }

        public SceneScribeGroup ActiveGroup => ActiveGroupPath.Peek();
    }
}
