using MySql.Data.MySqlClient;
using System;
using System.Data;
using UnityEngine;

/// <summary>
///  test doc
/// </summary>
public class MySqlAccess
{
    public MySqlConnection mySqlConnection; 
    // see mysql documentation for MySqlConnection: https://dev.mysql.com/doc/dev/connector-net/6.10/html/T_MySql_Data_MySqlClient_MySqlConnection.htm
    private static string host;  
    private static string port; 
    private static string userName;  
    private static string password;
    private static string databaseName;
   
    // construction method
    public MySqlAccess(string _host, string _port, string _userName, string _password, string _databaseName)
    {
        host = _host;
        port = _port;
        userName = _userName;
        password = _password;
        databaseName = _databaseName;
    }

    // open mysql method
    public void OpenSql()
    {
        try
        {
            string myConnectionString = string.Format("Database={0};Host={1};Username={2};Password={3};port={4}", databaseName, host, userName, password, port);
            // see mysql documentation for ConnectionString: https://dev.mysql.com/doc/dev/connector-net/6.10/html/P_MySql_Data_MySqlClient_MySqlConnection_ConnectionString.htm
            Debug.Log(myConnectionString);
            mySqlConnection = new MySqlConnection(myConnectionString);
            mySqlConnection.Open();
            Debug.Log("Connection succeeded.");
        }
        catch (Exception e)
        {
            throw new Exception("Connection failed." + e.Message.ToString());
        }
    }

    // close mysql method
    public void CloseSql()
    {
        if (mySqlConnection != null)
        {
            mySqlConnection.Close();
            mySqlConnection.Dispose();
            Debug.Log("Connection closed.");
        }
    }
/**
this is documentation test 
*/
    public DataSet Select(string tableName, string[] selectedItems, string[] columnNames,
        string[] operations, string[] requiredValues)
    // select method
    // return: a DataSet containing selected results
    // tableName: name of the target table in the database
    // selectedItems: name(s) of the returned columns
    // columnNames: name(s) of the columns where restricted conditions are applied
    // operations: name(s) of the operations which are applied 
    // requiredValues: value(s) of the restricted conditions
    {
        if (columnNames.Length != operations.Length || operations.Length != requiredValues.Length)
        // check input length 
        {
            throw new Exception("Wrong input format: " + "please make sure the number of columns, operations and required values are the same.");
        }
        string query = "Select " + selectedItems[0];  
        // build query command
        for (int i = 1; i < selectedItems.Length; i++)
        {
            query += "," + selectedItems[i];
        }
        query += " FROM " + tableName + " WHERE " + columnNames[0] + " " + operations[0] + " '" + requiredValues[0] + "'";
        for (int i = 1; i < columnNames.Length; i++)
        {
            query += " and " + columnNames[i] + " " + operations[i] + " '" + requiredValues[i] + "'";
        }
        return QuerySet(query);
    }

    // can be merged with Select
    

    public string Insert(string tableName, string[] columnNames, string[] insertedValues)
    // insert method
    // tableName: name of the target table in the database
    // columnNames: name(s) of the columns which new values will be inserted into
    // insertedValues: value(s) which will be inserted
    {
        if (columnNames.Length != insertedValues.Length)
        // check input length
        {
            throw new Exception("Wrong input format: " + "please make sure the number of columns and inserted values are the same.");
        }
        // build query command
        string query = "Insert Into " + tableName + '(';
        if (columnNames.Length == 1) 

        {
            query = query + columnNames[0] + ')' + "Values(" + "'" + insertedValues[0] + "'" + ");";
        }
        else
        {
            for (int i = 0; i < columnNames.Length; i++)
            {
                if (i != columnNames.Length - 1)
                {
                    query += columnNames[i] + ",";
                }
                else
                {
                    query += columnNames[i];
                }

            }
            query += ")Values(";
            for (int j = 0; j < insertedValues.Length; j++)
            {
                if (j != insertedValues.Length - 1)
                {
                    query = query + "'" + insertedValues[j] + "'" + ",";
                }
                else
                {
                    query = query + "'" + insertedValues[j] + "'";
                }
            }
            query += ");";
        }
        return query;
    }

    public string Delete(string tableName, string[] columnNames, string[] operations, string[] requiredValues)
    // Delete method
    // tableName: name of the target table in the database
    // columnNames: name(s) of the columns where delete condition is associated with
    // requiredValues: values of the columns where delete condition is associated with
    {
        if (columnNames.Length != operations.Length || operations.Length != requiredValues.Length)
        // check input length
        {
            throw new Exception("Wrong input format: " + "please make sure the number of columnsm, operations and required values are the same.");
        }
        // build query command
        string query = "DELETE FROM " + tableName + " WHERE " + columnNames[0] + " " + operations[0] + " '" + requiredValues[0] + "'";
        for (int i = 1; i < columnNames.Length; i++)
        {
            query += " and " + columnNames[i] + " " + operations[i] + " '" + requiredValues[i] + "'";
        }
        return query;
    }

    public string Update(string tableName, string updateColumn, string updateValue, string[] columnNames, string[] operations, string[] requiredValues)
    {
        if (columnNames.Length != operations.Length || operations.Length != requiredValues.Length)
        // check input length 
        {
            throw new Exception("Wrong input format: " + "please make sure the number of columns, operations and required values are the same.");
        }
        string query = "UPDATE " + "`" + tableName + "`" + " SET " + "`" + updateColumn + "`" + " = " + updateValue + " WHERE " + columnNames[0] + " " + operations[0] + " '" + requiredValues[0] + "'";
        for (int i = 1; i < columnNames.Length; i++)
        {
            query += " and " + columnNames[i] + " " + operations[i] + " '" + requiredValues[i] + "'";
        }
        return query;
    }
    
    private DataSet QuerySet(string sqlString)
    {
        if (mySqlConnection.State == ConnectionState.Open)
        // check the connection status
        {
            DataSet myDataSet = new DataSet();
            try
            {
                MySqlDataAdapter mySqlAdapter = new MySqlDataAdapter(sqlString, mySqlConnection); 
                // see mysql document for MySqlDataAdapter: https://dev.mysql.com/doc/dev/connector-net/6.10/html/T_MySql_Data_MySqlClient_MySqlDataAdapter.htm
                mySqlAdapter.Fill(myDataSet); 
            }
            catch (Exception e)
            {
                throw new Exception("SQL:" + sqlString + "/n" + e.Message.ToString());
            }
            return myDataSet;
        }
        return null;
    }
}
