using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;

public class AcceptRequestAndDestroy : MonoBehaviour
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
        //friendRequest = GameObject.FindGameObjectWithTag("FriendTag").GetComponent<CheckFriendRequests>().friendRequest;

        mysql = new MySqlAccess(host, port, userName, password, databaseName);
        //Debug.Log("Current scene: FriendRequest. Current userid: " + myUid.ToString());
        if (friendRequest.uid != 0) {
            Debug.Log("You have the following friend request, the friend class is: ");
            Debug.Log(friendRequest.uid);
            Debug.Log(friendRequest.friendName);
            Debug.Log(friendRequest.picid);
        }
        else
        {
            Debug.Log("No friend requests.");
        }
    }

    // Update is called once per frame
    public void AcceptRequestClick()
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
                    string query1 = mysql.Insert("friendinfo", new string[] { "userid1", "userid2" }, new string[] { myUid, friendRequest.uid.ToString() });
                    MySqlCommand cmd1 = new MySqlCommand(query1, mysql.mySqlConnection);
                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                    dataReader1.Close();

                    string query2 = mysql.Insert("friendinfo", new string[] { "userid1", "userid2" }, new string[] { friendRequest.uid.ToString(), myUid });
                    MySqlCommand cmd2 = new MySqlCommand(query2, mysql.mySqlConnection);
                    MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                    dataReader2.Close();
                    Debug.Log("Successful added.");

                    //string query3 = mysql.Update("userinfo", "friendrequest", "0", new string[] { "userid" }, new string[] { "=" }, new string[] { myUid });
                    //Debug.Log(query);
                    string query3 = mysql.Delete("friendrequest", new string[] {"`" + "fromuid" + "`", "`" + "touid" + "`"}, new string[] { "=", "=" }, new string[] { friendRequest.uid.ToString(), myUid });
                    MySqlCommand cmd3 = new MySqlCommand(query3, mysql.mySqlConnection);
                    MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                    dataReader3.Close();
                    Debug.Log("Friend request has been accepted successfully! " + "Uid" +friendRequest.uid.ToString() + " has been your friend now.");
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
