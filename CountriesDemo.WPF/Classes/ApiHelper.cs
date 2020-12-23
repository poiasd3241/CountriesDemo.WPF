using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CountriesDemo.WPF
{
	public class ApiHelper
	{
		public static HttpClient ApiClient { get; set; }

		/// <summary>
		/// Opens HttpClient once for future API requests.
		/// </summary>
		public static void InitializeClient()
		{
			try
			{
				ApiClient = new HttpClient
				{
					// Full Uri for search by [country full name]: https://restcountries.eu/rest/v2/name/full_country_name?fullText=true

					BaseAddress = new Uri("https://restcountries.eu/rest/v2/name/")
				};

				ApiClient.DefaultRequestHeaders.Accept.Clear();
				ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			}
			catch (Exception ex)
			{
				Helper.ShowExceptionMessage("Initializing Http Client", ex);
			}
		}
	}
}
