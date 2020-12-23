using System;
using System.Windows;
using System.Windows.Controls;

namespace CountriesDemo.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow() => InitializeComponent();

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			App.parentWindow = this;
			dockPanelCurrentModeInfo.Visibility = Visibility.Collapsed;
			Helper.InitializeStaticConStrValue();

			// Go to starting page.
			frame.Navigate(new PageOptionsForSearch());
		}

		private void BtnEditConfig_Click(object sender, RoutedEventArgs e)
		{
			WindowEditConfig windowEditConfig = new WindowEditConfig { Owner = this };
			windowEditConfig.ShowDialog();
		}

		private void BtnSwitchMode_Click(object sender, RoutedEventArgs e)
		{
			var currentPageName = (frame.Content as Page).Name;

			switch (currentPageName)
			{
				case "pageGetOneCountry":
					frame.Navigate(App.pageViewAllStored ??= new PageViewAllStored());
					tblCurrentModeName.Text = "view all stored countries info (database)";
					btnSwitchMode.Content = " get country info by name ";
					break;

				case "pageViewAllStored":
					frame.Navigate(App.pageGetOneCountry ??= new PageGetOneCountry());
					tblCurrentModeName.Text = "get country info by name";
					btnSwitchMode.Content = " view all stored countries info (database) ";
					break;

				default:
					// In case Page initialization fails and current page name doesn't get set
					// (extra measure to avoid application crash without error info message).
					Helper.ShowExceptionMessage(
						$"({ (sender as Button).Name } tries to navigate FROM a page with a Name = { currentPageName })",
						new Exception("message"));
					throw new NotImplementedException();
			}
		}
	}
}
