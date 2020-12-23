using System;

namespace CountriesDemo.WPF
{
	public class CountryModel
	{
		public string Name { get; set; }
		public string Alpha3Code { get; set; }
		public string Capital { get; set; }
		public string Area { get; set; }
		public string Population { get; set; }
		public string Region { get; set; }
		public bool IsNotFoundAPIRequestMessage { get; set; } = false;
		public bool IsAccidentalPartialNameSearch { get; set; } = false;

		/// <summary>
		/// For use in SQL queries.
		/// </summary>
		public decimal AreaDecimal
		{
			get
			{
				if (decimal.TryParse(Area, out decimal decimalValue))
				{
					return decimalValue;
				}
				else
				{
					Helper.ShowExceptionMessage(@"Converting Area value to decimal",
						new Exception("Area is not decimal. Value of AreaDecimal will be set to 0."));
					return 0;
				}
			}
			set { }
		}

		/// <summary>
		/// For use in SQL queries.
		/// </summary>
		public int PopulationInt
		{
			get
			{
				if (int.TryParse(Population, out int intValue))
				{
					return intValue;
				}
				else
				{
					Helper.ShowExceptionMessage(@"Converting Population value to int",
						new Exception("Population is not int. Value of PopulationInt will be set to 0."));
					return 0;
				}
			}
			set { }
		}
	}
}
