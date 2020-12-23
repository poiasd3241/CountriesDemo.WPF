using System;
using System.Configuration;
using System.Windows;

namespace CountriesDemo.WPF
{
	public static class Helper
	{
		/// <summary>
		/// Gets value of ConnectionString from app.config.
		/// </summary>
		/// <param name="conStrName">Name of ConnectionString in app.config.</param>
		/// <returns></returns>
		public static string GetConStrValueByName(string conStrName) => ConfigurationManager.ConnectionStrings[conStrName].ConnectionString;

		/// <summary>
		/// Sets initial value of a <see langword="static"/> ConnectionString container to the value from app.config at the start of application.
		/// </summary>
		public static void InitializeStaticConStrValue() => App.conStrValue = GetConStrValueByName("CountriesInfoDb");

		/// <summary>
		/// Saves changes to app.config (ConnectionString value).
		/// </summary>
		public static void SaveConfigFile()
		{
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			configuration.ConnectionStrings.ConnectionStrings["CountriesInfoDb"].ConnectionString = App.conStrValue;

			ConfigurationManager.RefreshSection("connectionStrings");
			configuration.Save(ConfigurationSaveMode.Minimal);
		}

		/// <summary>
		/// Shows a MessageBox with info about Exception and where it occurred.
		/// </summary>
		/// <param name="context">Exception in [...] (describe current code block).</param>
		/// <param name="ex"></param>
		public static void ShowExceptionMessage(string context, Exception ex)
		{
			MessageBox.Show("Exception in " + context + ".\n\n\nFull Exception message:" + "\n\n" + ex.Message,
				"Exception details", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
