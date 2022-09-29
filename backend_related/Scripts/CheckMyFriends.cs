using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;

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

public class CheckMyFriends : MonoBehaviour
{
    private string host = "120.77.148.135";
    private string port = "3306";     
    private string userName = "root";
    private string password = "penguinspy123456";
    private string databaseName = "penguintest";
    private MySqlAccess mysql;

    // required input data for this script  
    public string myUid; 
    public Friends myFriends = new Friends();
    
    private void Start()
    {
        myUid = GameObject.FindGameObjectWithTag("RegistrationTag").GetComponent<Login>().uid.ToString();
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
        //Debug.Log("Current scene: FriendList. Current userid: " + myUid.ToString());
    }

    public void CheckMyFriendsClick()
    {
        Debug.Log("Moo");
        if (myUid != "-1")
        {
            mysql.OpenSql();
            DataSet queryResult = mysql.Select("friendinfo", new string[] { "userid2" }, new string[] {"`" + "userid1" + "`"}, new string[] { "=" }, new string[] { myUid });
            if (queryResult != null)
            {
                DataTable table = queryResult.Tables[0];
                if (table.Rows.Count > 0)        
                {
                    myFriends.friends = new Friend[table.Rows.Count];
                    for (int i = 0; i < table.Rows.Count; i++) 
                    {
                        DataSet queryFriend = mysql.Select("userinfo", new string[] { "username", "picid" }, new string[] {"`" + "userid" + "`"}, new string[] { "=" }, new string[] { table.Rows[i][0].ToString() });
                        if (queryFriend != null) 
                        {
                            Friend friend = new Friend();
                            //uid
                            friend.uid = (int)table.Rows[i][0];
                            //username
                            friend.friendName = queryFriend.Tables[0].Rows[0][0].ToString();
                            //picid
                            friend.picid = (int)queryFriend.Tables[0].Rows[0][1];
                            myFriends.friends[i] = friend;
                        }
                    }
                    // see console output to make sure myFriends stores the current information
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        Debug.Log(myFriends.friends[i].uid);
                        Debug.Log(myFriends.friends[i].friendName);
                        Debug.Log(myFriends.friends[i].picid);
                    }
                }
                else
                {
                    Debug.Log("No friends have been added.");
                }
            }
            mysql.CloseSql();
        }
        else
        {
            Debug.Log("Please login first.");
        }
    }
}

