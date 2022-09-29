using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;

public class AddFriendRequest : MonoBehaviour
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
    public void AddFriendRequestClick()
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
                    /*string query = mysql.Update("userinfo", "friendrequest", myUid, new string[] { "userid" }, new string[] { "=" }, new string[] { friendUid.text });
                    //Debug.Log(query);
                    Debug.Log("Friend request sent successfully! " + "Uid" + myUid.ToString() + " wants to add " + "Uid" + friendUid.text);
                    MySqlCommand cmd = new MySqlCommand(query, mysql.mySqlConnection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    dataReader.Close();*/

                    string query = mysql.Insert("friendrequest", new string[] { "fromuid", "touid" }, new string[] { myUid, friendUid.text });
                    MySqlCommand cmd = new MySqlCommand(query, mysql.mySqlConnection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    Debug.Log("Friend request sent successfully! " + "Uid" + myUid.ToString() + " wants to add " + "Uid" + friendUid.text);
                    dataReader.Close();
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

