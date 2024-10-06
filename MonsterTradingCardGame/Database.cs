using Npgsql;
using System;
using System.Threading.Tasks;
/*
 Use sql to insert and extract data from database
 */
public class Database
{
	private static string connectionString = "Host=localhost;Port=5432;Username=toukage;Password=mtcgserver;Database=MTCG_DB";

	public static NpgsqlConnection Connection()
	{
        NpgsqlConnection connection = new NpgsqlConnection(connectionString);//erstellt eine verbindung zur datenbank aus den infos von connectionString
		connection.Open(); // opend die connection zur datenbank
		return connection;
	}
   
	
}
