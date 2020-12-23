using System;
using System.Windows;
using System.Windows.Controls;

namespace CountriesDemo.WPF
{
	/// <summary>
	/// Interaction logic for Page_DisplayAllStored.xaml
	/// </summary>
	public partial class PageViewAllStored : Page
	{
		public PageViewAllStored()
		{
			InitializeComponent();
			this.Name = "pageViewAllStored";
			App.pageViewAllStored = this;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			// Clear page.
			tblTipValidResult.Visibility = Visibility.Collapsed;
			dataGrid_ViewAllDBCountries.Visibility = Visibility.Collapsed;
			gridNoOrNullResult.Visibility = Visibility.Collapsed;

			try
			{
				var data = DatabaseAccess.ViewAllDbItems();

				if (data is null)
				{
					// Show Null result message.
					spNullResultMessage.Visibility = Visibility.Visible;
					spNoResultMessage.Visibility = Visibility.Collapsed;
					gridNoOrNullResult.Visibility = Visibility.Visible;

				}
				else if (data.Rows.Count == 0)
				{
					// Show No results message.
					spNoResultMessage.Visibility = Visibility.Visible;
					spNullResultMessage.Visibility = Visibility.Collapsed;
					gridNoOrNullResult.Visibility = Visibility.Visible;
				}
				else
				{
					// Show valid results.
					dataGrid_ViewAllDBCountries.ItemsSource = data.DefaultView;
					tblTipValidResult.Visibility = Visibility.Visible;
					dataGrid_ViewAllDBCountries.Visibility = Visibility.Visible;
				}

			}
			catch (Exception ex)
			{
				Helper.ShowExceptionMessage("Getting all countries from Db / Populating DataGrid with query result", ex);

				// Show Null result message.

				spNullResultMessage.Visibility = Visibility.Visible;
				spNoResultMessage.Visibility = Visibility.Collapsed;
				gridNoOrNullResult.Visibility = Visibility.Visible;
			}
		}
	}
}
