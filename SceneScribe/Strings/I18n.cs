using Windows.ApplicationModel.Resources;

namespace SceneScribe.Strings
{
	/// <summary>
	/// Provides a wrapper for i18n tasks in Scene Scribe.
	/// </summary>
	internal static class I18n
	{
		private static readonly ResourceLoader ResourceLoader = ResourceLoader.GetForViewIndependentUse();

		/// <summary>
		/// Gets the localized string for a given resource.
		/// </summary>
		/// <param name="resource">The name of the resource to localize.</param>
		/// <returns>The localized string.</returns>
		public static string GetString(string resource)
			=> ResourceLoader.GetString(resource);
	}
}
