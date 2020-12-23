using System;
using System.Windows;
using System.Windows.Controls;

namespace CountriesDemo.WPF
{
	/// <summary>
	/// Interaction logic for Window_EditConfig.xaml
	/// </summary>
	public partial class WindowEditConfig : Window
	{
		public WindowEditConfig() => InitializeComponent();

		/// <summary>
		/// Prepare the window for display.
		/// </summary>
		private void Window_Initialized(object sender, EventArgs e)
		{
			tbConStrValue.Text = App.conStrValue ?? "Null ConnectionString";
			tblUnsavedChangesNotification.Visibility = Visibility.Collapsed;
			btnSaveConfig.IsEnabled = false;
		}

		private void BtnSaveConfig_Click(object sender, RoutedEventArgs e)
		{
			App.conStrValue = tbConStrValue.Text;
			Helper.SaveConfigFile();
			this.Close();
		}

		private void BtnCancelEditConfig_Click(object sender, RoutedEventArgs e) => this.Close();

		private void TbConStrValue_TextChanged(object sender, TextChangedEventArgs e)
		{
			// Avoid redundant changes to controls before the window is loaded (displayed).
			if (this.IsLoaded)
			{
				// Check if there are unsaved changes to app.config.
				if (tbConStrValue.Text != App.conStrValue)
				{
					tblUnsavedChangesNotification.Visibility = Visibility.Visible;
					btnSaveConfig.IsEnabled = true;
				}
				else
				{
					tblUnsavedChangesNotification.Visibility = Visibility.Collapsed;
					btnSaveConfig.IsEnabled = false;
				}
			}
		}
	}
}
