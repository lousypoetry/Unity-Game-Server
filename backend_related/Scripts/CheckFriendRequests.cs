using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;
using UnityEngine.SceneManagement;

// has already defined in Friend.cs
// public class Friend
// {
//    public int uid;
//    public string friendName;
//    public int picid;
//}
// has already defined in Friends.cs
//public class Friends
//{
//    public Friend[] friends;
//}

public class CheckFriendRequests : MonoBehaviour
{
    private string host = "120.77.148.135";
    private string port = "3306";
    private string userName = "root";
    private string password = "penguinspy123456";
    private string databaseName = "penguintest";
    private MySqlAccess mysql;

    // required input data for this script
    public string myUid;
    //public Friend friendRequest;
    public Friends friendRequests;

    private void Start()
    {
        myUid = GameObject.FindGameObjectWithTag("RegistrationTag").GetComponent<Login>().uid.ToString();
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
        //Debug.Log("Current scene: FriendList. Current userid: " + myUid.ToString());
    }

    public void CheckFriendRequestsClick()
    {
        if (myUid != "-1")
        {
            mysql.OpenSql();
            //DataSet queryResult = mysql.Select("userinfo", new string[] { "friendrequest" }, new string[] {"`" + "userid" + "`"}, new string[] { "=" }, new string[] { myUid });
            DataSet queryResult = mysql.Select("friendrequest", new string[] { "fromuid" }, new string[] {"`" + "touid" + "`"}, new string[] { "=" }, new string[] { myUid });
            if (queryResult != null)
            {
                DataTable table = queryResult.Tables[0];
                if (table.Rows.Count > 0)
                {
                    friendRequests.friends = new Friend[table.Rows.Count];
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        Debug.Log("now is friend uid:" + table.Rows[i][0].ToString());
                        DataSet queryFriend = mysql.Select("userinfo", new string[] { "username", "picid" }, new string[] {"`" + "userid" + "`"}, new string[] { "=" }, new string[] { table.Rows[i][0].ToString() });
                        if (queryFriend != null)
                        {
                            Friend friendRequest = new Friend();
                            //uid
                            friendRequest.uid = (int)table.Rows[i][0];
                            //username
                            friendRequest.friendName = queryFriend.Tables[0].Rows[0][0].ToString();
                            //picid
                            friendRequest.picid = (int)queryFriend.Tables[0].Rows[0][1];
                            friendRequests.friends[i] = friendRequest;
                        }
                        // see console output to make sure myFriends stores the current information
                    }
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        Debug.Log(friendRequests.friends[i].uid);
                        Debug.Log(friendRequests.friends[i].friendName);
                        Debug.Log(friendRequests.friends[i].picid);
                    }
                }
                else
                {
                    Debug.Log("No friend requests.");
                    //Debug.Log((friendRequest.uid).ToString());
                }
            }
            mysql.CloseSql();
        }
        else
        {
            Debug.Log("Please login first.");
        }
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "FriendList")
        {
            SceneManager.LoadScene("FriendRequest");
        }
        return;
    }
}
