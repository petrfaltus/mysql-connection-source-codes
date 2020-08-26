using System;
using MySql.Data.MySqlClient;

public class MySQLclient
{
    private const string db_host = "localhost";
    private const int db_port = 3306;
    private const string db_name = "testdb";
    private const string db_username = "testuser";
    private const string db_password = "T3stUs3r!";

    private const string db_table = "people";

    public static void Main(string[] args)
    {
        // Build the connection string
        MySqlConnectionStringBuilder connBuilder = new MySqlConnectionStringBuilder();
        connBuilder.Server = db_host;
        connBuilder.Port = db_port;
        connBuilder.UserID = db_username;
        connBuilder.Password = db_password;
        connBuilder.Database = db_name;

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connBuilder.ConnectionString))
            {
                // Connect the database
                conn.Open();

                Console.WriteLine("Data source: {0}", conn.DataSource);
                Console.WriteLine("Server version: {0}", conn.ServerVersion);
                Console.WriteLine("Server thread: {0}", conn.ServerThread);
                Console.WriteLine("Database: {0}", conn.Database);
                Console.WriteLine("Connection timeout: {0}", conn.ConnectionTimeout);
                Console.WriteLine();

                // Full SELECT statement
                string sql1 = String.Format("select * from {0}", db_table);
                using (var cmd = new MySqlCommand(sql1, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int columns = reader.FieldCount;
                        Console.WriteLine("Total columns: {0}", columns);

                        for (int ii = 0; ii < columns; ii++)
                        {
                            Console.WriteLine(" - {0} {1}", reader.GetName(ii), reader.GetDataTypeName(ii));
                        }

                        int number = 0;
                        while (reader.Read())
                        {
                            number++;
                            Console.Write(number);

                            for (int ii = 0; ii < columns; ii++)
                            {
                                string type = reader.GetDataTypeName(ii);

                                string value = "?";
                                if (!reader.IsDBNull(ii))
                                {
                                    if (type.EndsWith("CHAR"))
                                    {
                                        value = reader.GetString(ii);
                                    }
                                    else if (type.Equals("DATETIME"))
                                    {
                                        value = reader.GetDateTime(ii).ToString();
                                    }
                                    else if (type.EndsWith("INT"))
                                    {
                                        value = reader.GetInt32(ii).ToString();
                                    }
                                }
                                else
                                {
                                    value = "(null)";
                                }

                                Console.Write(" '{0}'", value);
                            }

                            Console.WriteLine();
                        }
                    }

            }
        }
        catch (MySqlException mex)
        {
            Console.WriteLine("MySQL error {0}: {1}", mex.Number, mex.Message);
        }

    }
}
