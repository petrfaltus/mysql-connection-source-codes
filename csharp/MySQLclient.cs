using System;
using System.Data;
using MySql.Data.MySqlClient;

public class MySQLclient
{
    private const string db_host = "localhost";
    private const int db_port = 3306;
    private const string db_name = "testdb";
    private const string db_username = "testuser";
    private const string db_password = "T3stUs3r!";

    private const string db_table = "people";

    private const string db_update_column = "remark";
    private const string db_update_column_variable = "@updatevar";

    private const string db_column = "id";
    private const string db_column_variable = "@var";
    private const int db_column_value = 1;

    private const string db_factorial_variable = "@n";
    private const int db_factorial_value = 4;

    private const string db_add_and_subtract_a_variable = "@a";
    private const int db_add_and_subtract_a_value = 12;
    private const string db_add_and_subtract_b_variable = "@b";
    private const int db_add_and_subtract_b_value = 5;

    private const string db_add_and_subtract_x_variable = "@x";
    private const string db_add_and_subtract_y_variable = "@y";

    private static string GetNow()
    {
        DateTime dateTimeNow = DateTime.Now;
        return dateTimeNow.ToString();
    }

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

                // UPDATE statement
                string new_comment = "C# " + GetNow();

                string sql0 = String.Format("update {0} set {1}={2} where {3}!={4}", db_table, db_update_column, db_update_column_variable, db_column, db_column_variable);
                using (var cmd = new MySqlCommand(sql0, conn))
                {
                    cmd.Parameters.AddWithValue(db_update_column_variable, new_comment);
                    cmd.Parameters.AddWithValue(db_column_variable, db_column_value);

                    int updatedRows = cmd.ExecuteNonQuery();
                    Console.WriteLine("Total updated rows: {0}", updatedRows);
                }

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

                Console.WriteLine();

                // SELECT WHERE statement
                string sql2 = String.Format("select count(*) from {0} where {1}!={2}", db_table, db_column, db_column_variable);
                using (var cmd = new MySqlCommand(sql2, conn))
                {
                    cmd.Parameters.AddWithValue(db_column_variable, db_column_value);

                    Object result = cmd.ExecuteScalar();
                    Console.WriteLine("Result: {0}", result);
                }

                Console.WriteLine();

                // SELECT function statement
                string sql3 = String.Format("select factorial({0})", db_factorial_variable);
                using (var cmd = new MySqlCommand(sql3, conn))
                {
                    cmd.Parameters.AddWithValue(db_factorial_variable, db_factorial_value);

                    Object result = cmd.ExecuteScalar();
                    Console.WriteLine("Result: {0}", result);
                }

                Console.WriteLine();

                // CALL procedure statement
                string sql4 = String.Format("add_and_subtract");
                using (var cmd = new MySqlCommand(sql4, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(db_add_and_subtract_a_variable, db_add_and_subtract_a_value);
                    cmd.Parameters.AddWithValue(db_add_and_subtract_b_variable, db_add_and_subtract_b_value);
                    MySqlParameter par3 = new MySqlParameter(db_add_and_subtract_x_variable, MySqlDbType.Int32);
                    par3.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(par3);
                    MySqlParameter par4 = new MySqlParameter(db_add_and_subtract_y_variable, MySqlDbType.Int32);
                    par4.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(par4);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Result x: {0}", cmd.Parameters[db_add_and_subtract_x_variable].Value);
                    Console.WriteLine("Result y: {0}", cmd.Parameters[db_add_and_subtract_y_variable].Value);
                }
            }
        }
        catch (MySqlException mex)
        {
            Console.WriteLine("MySQL error {0}: {1}", mex.Number, mex.Message);
        }

    }
}
