using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;

public class DeclineRequestAndDestroy : MonoBehaviour
{
    private string host = "120.77.148.135";
    private string port = "3306";
    private string userName = "root";
    private string password = "penguinspy123456";
    private string databaseName = "penguintest";
    private MySqlAccess mysql;

    // required input data for this script
    public string myUid;
    public Friend friendRequest = new Friend();
    public GameObject requestToDestroy;

    private void Start()
    {
        myUid = GameObject.FindGameObjectWithTag("RegistrationTag").GetComponent<Login>().uid.ToString();
        friendRequest = requestToDestroy.GetComponent<RequestItem>().curRequestFriend;

        mysql = new MySqlAccess(host, port, userName, password, databaseName);
        //Debug.Log("Current scene: FriendRequest. Current userid: " + myUid.ToString());
        //Debug.Log("You have the following friend request, the friend class is: ");
        //Debug.Log(friendRequest.uid);
        //Debug.Log(friendRequest.friendName);
        //Debug.Log(friendRequest.picid);
    }

    // Update is called once per frame
    public void DeclineRequestClick()
    {
        if (myUid != "-1")
        {
            mysql.OpenSql();
            DataSet queryResult = mysql.Select("friendinfo", new string[] { "userid2" }, new string[] {"`" + "userid1" + "`", "`" + "userid2" + "`"}, new string[] { "=", "=" }, new string[] { myUid, friendRequest.uid.ToString()});
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
                    Debug.Log("You have declined the request.");
                    //string query = mysql.Update("userinfo", "friendrequest", "0", new string[] { "userid" }, new string[] { "=" }, new string[] { myUid });
                    string query = mysql.Delete("friendrequest", new string[] {"`" + "fromuid" + "`", "`" + "touid" + "`"}, new string[] { "=", "=" }, new string[] { friendRequest.uid.ToString(), myUid });
                    MySqlCommand cmd = new MySqlCommand(query, mysql.mySqlConnection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    //Debug.Log(query);
                    //Debug.Log("Friend request has been declined! ");
                    dataReader.Close();
                }
            }
            mysql.CloseSql();
        }
        else
        {
            Debug.Log("Wrong input format.");
        }
        Destroy (requestToDestroy);
    }
}
