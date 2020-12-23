using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CountriesDemo.WPF
{
	public class CountryProcessor
	{
		/// <summary>
		/// Makes API request to find country details.
		/// </summary>
		/// <param name="searchWord">Search word, supposed to be a full name of the desired country.</param>
		/// <returns></returns>
		public static async Task<CountryModel> GetCountryByAPIRequest(string searchWord)
		{
			try
			{
				using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(searchWord + "?fullText=true");

				if (response.IsSuccessStatusCode)
				{
					try
					{
						var json = await response.Content.ReadAsStringAsync();
						List<CountryModel> result = JsonConvert.DeserializeObject<List<CountryModel>>(json);

						// Possible results from "accidental" partial name search 
						// (short search words like domains/codes may return unwanted results).

						if (string.Equals(result[0].Name, searchWord, StringComparison.OrdinalIgnoreCase))
						{
							// Valid result.
							return result[0];
						}
						else
						{
							// "Accidental" partial name search.
							return new CountryModel() { IsAccidentalPartialNameSearch = true };
						}
					}
					catch (Exception ex)
					{
						Helper.ShowExceptionMessage("Reading / deserializing API response", ex);
						return null;
					}
				}
				else if (response.StatusCode is HttpStatusCode.NotFound)
				{
					// Not Found.
					return new CountryModel() { IsNotFoundAPIRequestMessage = true };
				}
				else
				{
					// Request failed.
					return null;
				}
			}
			catch (Exception ex)
			{
				Helper.ShowExceptionMessage("ApiClient.GetAsync(countryNameForSearch) - HttpResponseMessage couldn't be created", ex);
				return null;
			}
		}
	}
}
