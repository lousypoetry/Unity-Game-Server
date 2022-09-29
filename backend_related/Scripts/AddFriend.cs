using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;

public class AddFriend : MonoBehaviour
{
    private string host = "120.77.148.135";
    private string port = "3306";     
    private string userName = "root";
    private string password = "penguinspy123456";
    private string databaseName = "penguintest";
    private MySqlAccess mysql;

    // required input data for this script  
    public string myUid; 
    public InputField friendUid;
    
    private void Start()
    {
        myUid = GameObject.FindGameObjectWithTag("RegistrationTag").GetComponent<Login>().uid.ToString();
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
        //Debug.Log("Current scene: FriendList. Current userid: " + myUid.ToString());
    }

    // Update is called once per frame
    public void AddFriendClick()
    {
        if (myUid != "-1" && friendUid.text != null)
        {
            mysql.OpenSql();
            DataSet queryResult = mysql.Select("friendinfo", new string[] { "userid2" }, new string[] {"`" + "userid1" + "`", "`" + "userid2" + "`"}, new string[] { "=", "=" }, new string[] { myUid, friendUid.text});
            if (queryResult!= null)
            {
                DataTable table = queryResult.Tables[0];
                if (table.Rows.Count > 0)
                // check if friend already added           
                {
                    Debug.Log("This friend has already been added.");
                }
                else
                {
                    string query1 = mysql.Insert("friendinfo", new string[] { "userid1", "userid2" }, new string[] { myUid, friendUid.text });
                    MySqlCommand cmd1 = new MySqlCommand(query1, mysql.mySqlConnection);
                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                    dataReader1.Close();
                    
                    string query2 = mysql.Insert("friendinfo", new string[] { "userid1", "userid2" }, new string[] { friendUid.text, myUid });
                    MySqlCommand cmd2 = new MySqlCommand(query2, mysql.mySqlConnection);
                    MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                    dataReader2.Close();
                    Debug.Log("Successful added.");
                }
            }
            mysql.CloseSql();
        }
        else
        {
            Debug.Log("Wrong input format.");
        }
    }
}
