using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CountriesDemo.WPF
{
	public class DatabaseAccess
	{
		/// <summary>
		/// Gets all countries from database and puts them in DataTable for use by DataGrid.
		/// </summary>
		/// <returns></returns>
		public static DataTable ViewAllDbItems()
		{
			string query =
				@$"
                SELECT  co.Name, co.Alpha3Code, ci.Name as 'Capital', co.Area as 'Area (km²)', co.Population, re.Name as 'Region'
                FROM Countries co 
                inner join Cities ci on ci.Id = co.CapitalId 
                inner join Regions re on re.Id = co.RegionId
                ";
			try
			{
				using var connection = new SqlConnection(App.conStrValue);
				using var adapter = new SqlDataAdapter(query, connection);

				var table = new DataTable();
				adapter.Fill(table);
				return table;
			}
			catch (Exception ex)
			{
				Helper.ShowExceptionMessage("Establishing Db connection / SqlDataAdapter (ViewAllDbItems)", ex);
				return null;
			}
		}

		/// <summary>
		/// Save/Update database entry (by alpha-3 code).
		/// </summary>
		/// <param name="c">Country (its details) that needs to be saved/updated.</param>
		/// <returns></returns>
		public static async Task<string> SaveOrUpdateCountryInfoToDb(CountryModel c)
		{
			// Prepare country name, capital(name) and region for insert/update: replace ' with ''.
			c.Name = c.Name.Replace("'", "''");
			c.Capital = c.Capital.Replace("'", "''");
			c.Region = c.Region.Replace("'", "''");

			string query =
				$@"
                DECLARE @CapitalIdForInsert INT
                DECLARE @RegionIdForInsert INT
                
                IF NOT EXISTS (SELECT * FROM Cities WHERE Name = '{ c.Capital }')
                INSERT INTO Cities VALUES ('{ c.Capital }')
                
                IF NOT EXISTS (SELECT * FROM Regions WHERE Name = '{ c.Region }')
                INSERT INTO Regions VALUES ('{ c.Region }')

                SET @CapitalIdForInsert = (SELECT Id FROM Cities WHERE Name = '{ c.Capital }')
                SET @RegionIdForInsert = (SELECT Id FROM Regions WHERE Name = '{ c.Region }')

                IF NOT EXISTS (SELECT * FROM Countries WHERE Alpha3Code = '{ c.Alpha3Code }')
                BEGIN
                INSERT INTO Countries (Name, Alpha3Code, CapitalId, Area, Population, RegionId) 
                VALUES ('{ c.Name }', '{ c.Alpha3Code }', @CapitalIdForInsert, '{ c.AreaDecimal }', '{ c.PopulationInt }', @RegionIdForInsert)
                SELECT 'INSERTED'
                END

                ELSE
                BEGIN
                UPDATE Countries 
                SET Name = '{ c.Name }', CapitalId = @CapitalIdForInsert, Area = '{ c.AreaDecimal }', Population = '{ c.PopulationInt }', RegionId = @RegionIdForInsert
                WHERE Alpha3Code = '{ c.Alpha3Code }'
                SELECT 'UPDATED'
                END
                ";
			try
			{
				using IDbConnection connection = new SqlConnection(App.conStrValue);

				// Expecting result to be either "INSERTED" or "UPDATED" as a type of completed operation.
				string result = await connection.QueryFirstAsync<string>(query);
				return result;
			}
			catch (Exception ex)
			{
				Helper.ShowExceptionMessage("Establishing Db connection / Awaiting query result (SaveOrUpdateCountryInfoToDb)", ex);
				return null;
			}
		}
	}
}
