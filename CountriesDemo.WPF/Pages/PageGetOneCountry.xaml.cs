using System;
using System.Windows;
using System.Windows.Controls;

namespace CountriesDemo.WPF
{
	/// <summary>
	/// Interaction logic for Page_SearchWithAPI.xaml
	/// </summary>
	public partial class PageGetOneCountry : Page
	{
		public PageGetOneCountry()
		{
			InitializeComponent();

			this.Name = "pageGetOneCountry";
			App.pageGetOneCountry = this;

			ApiHelper.InitializeClient();
		}

		// Container for country details for saving/updating to/in Db.
		private CountryModel _countryForSave;

		private void TbCountryNameForSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (this.IsLoaded)
			{
				btnSearchCountry.IsEnabled = !string.IsNullOrEmpty((sender as TextBox).Text.ToString());
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			tblTipsStatus.Text = "Please enter FULL name of the desired country for API request";
			tbCountryNameForSearch.IsEnabled = true;
			tbCountryNameForSearch.Text = "";

			btnSearchCountry.IsEnabled = false;
			HideAllResultGrids();
		}

		private async void BtnSearchCountry_Click(object sender, RoutedEventArgs e)
		{
			btnSearchCountry.IsEnabled = false;
			HideAllResultGrids();

			// Clear container for new search.
			_countryForSave = null;

			var resultApi = await CountryProcessor.GetCountryByAPIRequest(tbCountryNameForSearch.Text);
			DisplayAPIResult(resultApi);

			btnSearchCountry.IsEnabled = true;
		}

		/// <summary>
		/// Displays result from API request (if valid) or a message for user, otherwise.
		/// </summary>
		private void DisplayAPIResult(CountryModel result)
		{
			if (result is null)
			{
				Notify("API request failed", true);
				ShowResultGrid(gridNullResult);
			}
			else if (result.IsNotFoundAPIRequestMessage)
			{
				// API request successful, 0 matches.
				Notify("API request didn't retrieve any matches", true);
				ShowResultGrid(gridNoResults);
			}
			else if (result.IsAccidentalPartialNameSearch)
			{
				Notify("API request returned one or more entries with country names not matching the search word\n" +
					"Make sure to use FULL name of the desired country!", true);
				ShowResultGrid(gridNoResults);
			}
			else
			{
				// Valid result.

				// Unicode superscript km^2 -> km².
				tblAreaKM2Unicode.Text = "Area, km²: ";

				gridOfferToSaveOrUpdate.Visibility = Visibility.Visible;

				// Save result to container for future possible saving/updating to/in Db.
				_countryForSave = result;

				PopulateCountryInfoTextBoxes(_countryForSave);

				Notify("Returned single entry. Please decide if you want to save/update it to/in Db", true);
				ShowResultGrid(gridSingleResult);
			}
		}

		private async void BtnSaveOrUpdateCountryInfoToDb_Click(object sender, RoutedEventArgs e)
		{
			gridOfferToSaveOrUpdate.Visibility = Visibility.Collapsed;

			var result = await DatabaseAccess.SaveOrUpdateCountryInfoToDb(_countryForSave);

			switch (result)
			{
				case "INSERTED":
					Notify("Successfully SAVED country information (new entry) to database", false);
					break;

				case "UPDATED":
					Notify("Successfully UPDATED country information in database", false);
					break;

				default:
					// Error establishing Db connection / awaiting query result.
					Notify("Failed to save/update country information to/in database!\n" +
						"Please check the connection string / check your (server / local database) and try again.", false);
					ShowResultGrid(gridNullResult);
					return;
			}
		}

		private void BtnRejectSaveCountryInfoToDb_Click(object sender, RoutedEventArgs e)
		{
			gridOfferToSaveOrUpdate.Visibility = Visibility.Collapsed;
			Notify("Rejected saving/updating country information to/in database", true);
		}

		#region Page Helpers

		/// <summary>
		/// Shows specified result grid and hides other result grids.
		/// </summary>
		/// <param name="grid">Result grid to show.</param>
		private void ShowResultGrid(Grid grid)
		{
			HideAllResultGrids();

			// Show required grid.
			grid.Visibility = Visibility.Visible;
		}

		/// <summary>
		/// Hides all possible result grids to prepares the page for new result.
		/// </summary>
		private void HideAllResultGrids()
		{
			gridNullResult.Visibility = Visibility.Collapsed;
			gridNoResults.Visibility = Visibility.Collapsed;
			gridSingleResult.Visibility = Visibility.Collapsed;
		}

		private void PopulateCountryInfoTextBoxes(CountryModel country)
		{
			tbSingleCountryName.Text = country.Name;

			// Null values from API request result will be displayed as "--null--".
			tbSingleCountryAlphaThreeCode.Text = country.Alpha3Code ?? "--null--";
			tbSingleCountryCapital.Text = country.Capital ?? "--null--";
			tbSingleCountryArea.Text = country.Area ?? "--null--";
			tbSingleCountryPopulation.Text = country.Population ?? "--null--";
			tbSingleCountryRegion.Text = country.Region ?? "--null--";
		}

		/// <summary>
		/// Simple notification to TextBlock.
		/// </summary>
		/// <param name="notificationMessage">Message for user.</param>
		/// <param name="isAPIRequest">Whether the message is coming from API request (true) or from Database (false).</param>
		private void Notify(string notificationMessage, bool isAPIRequest)
		{
			tblTipsStatus.Text = $"[{ DateTime.Now:HH:mm:ss.fff}]";
			tblTipsStatus.Text += isAPIRequest ? " [API] " + notificationMessage : " [DB] " + notificationMessage;
		}

		#endregion
	}
}
