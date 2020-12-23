using System.Windows;

namespace CountriesDemo.WPF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		// Instance of the MainWindow for accessing its Controls.
		public static MainWindow parentWindow;

		// Instances of pages for fast page navigation.
		public static PageGetOneCountry pageGetOneCountry;
		public static PageViewAllStored pageViewAllStored;

		// ConnectionString container for simple and fast access to ConnectionString value throughout application.
		public static string conStrValue;
	}
}
