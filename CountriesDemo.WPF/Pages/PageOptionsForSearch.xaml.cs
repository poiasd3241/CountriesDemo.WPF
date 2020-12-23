using System.Windows;
using System.Windows.Controls;

namespace CountriesDemo.WPF
{
	/// <summary>
	/// Interaction logic for Page_OptionsForSearch.xaml
	/// </summary>
	public partial class PageOptionsForSearch : Page
	{
		public PageOptionsForSearch() => InitializeComponent();

		private void BtnSelectMode_GetOneCountry_Click(object sender, RoutedEventArgs e)
		{
			App.parentWindow.dockPanelCurrentModeInfo.Visibility = Visibility.Visible;
			App.parentWindow.frame.Navigate(new PageGetOneCountry());
			App.parentWindow.btnSwitchMode.Content = " view all stored countries info (database) ";
			App.parentWindow.tblCurrentModeName.Text = "get country info by name";
		}

		private void BtnSelectMode_ViewAllStoredDB_Click(object sender, RoutedEventArgs e)
		{
			App.parentWindow.dockPanelCurrentModeInfo.Visibility = Visibility.Visible;
			App.parentWindow.frame.Navigate(new PageViewAllStored());
			App.parentWindow.btnSwitchMode.Content = " get country info by name ";
			App.parentWindow.tblCurrentModeName.Text = "view all stored countries info (database)";
		}
	}
}
