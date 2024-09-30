using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Collections;

// REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service" dans le code, le fichier svc et le fichier de configuration.
public class Service : IService
{
	public string GetData(int value)
	{
		return string.Format("You entered: {0}", value);
	}

	public CompositeType GetDataUsingDataContract(CompositeType composite)
	{
		if (composite == null)
		{
			throw new ArgumentNullException("composite");
		}
		if (composite.BoolValue)
		{
			composite.StringValue += "Suffix";
		}
		return composite;
	}

	public string Meteo_GetTemperature()
	{

		string result = null;
		string db = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\aybou\source\repos\TP3 partie 2\TP3 partie 2\App_Data\Database.mdf;Integrated Security=True;";
		string query = "SELECT * FROM [T_Data] WHERE [DateReleve] = (SELECT MAX([DateReleve]) FROM [T_Data])";


        SqlConnection sqlConnection = new SqlConnection(db);
		sqlConnection.Open();

		using (SqlConnection databaseConnection = new SqlConnection(db))
		{
			try
			{
				databaseConnection.Open();
				SqlCommand databaseCommand = new SqlCommand(query, databaseConnection);
                SqlDataReader reader = databaseCommand.ExecuteReader();
				reader.Read();
				result = reader.GetDouble(reader.GetOrdinal("Temp")).ToString();
                databaseConnection.Close();
			}
			catch (SqlException sqlEx)
			{
				throw new Exception(sqlEx.Message);
			}
		}

		return result;
	}

	public  void Meteo_GetTemperatureByDate(string dateTempSearch, out string temp)
    {
        // format of date is	2023-09-23T13:12:02.000
		//						yyyy-mm-ddThh:m:ss.ms
        string db = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\aybou\source\repos\TP3 partie 2\TP3 partie 2\App_Data\Database.mdf;Integrated Security=True;";
        string query = "SELECT * FROM [T_Data] WHERE [DateReleve] = (SELECT MAX([DateReleve]) FROM [T_Data] WHERE DateReleve < CAST(N'"+dateTempSearch+"' AS DateTime))";


        SqlConnection sqlConnection = new SqlConnection(db);
        sqlConnection.Open();

        using (SqlConnection databaseConnection = new SqlConnection(db))
        {
            try
            {
                databaseConnection.Open();
                SqlCommand databaseCommand = new SqlCommand(query, databaseConnection);
                SqlDataReader reader = databaseCommand.ExecuteReader();
                reader.Read();
                temp = reader.GetString(reader.GetOrdinal("DateReleveVC"));
                databaseConnection.Close();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message);
            }
        }

    }
}
