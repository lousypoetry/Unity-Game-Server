using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SearchUser : MonoBehaviour
{
    private string host = "120.77.148.135";
    private string port = "3306";     
    private string userName = "root";
    private string password = "penguinspy123456";
    private string databaseName = "penguintest";
    private MySqlAccess mysql; 
    
    // required input data for this script
    public string myUid;
    public Friend searchFriend = new Friend();
    public InputField searchUid;
 
    private void Start()
    {
        myUid = GameObject.FindGameObjectWithTag("RegistrationTag").GetComponent<Login>().uid.ToString();
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
    }
    public void SearchUserClick()
    {
        if (searchUid.text != null && myUid != "-1")
        {
            mysql.OpenSql();
            DataSet queryResult = mysql.Select("userinfo", new string[] { "userid", "username", "picid" }, new string[] { "`" + "userid" + "`" }, new string[] { "=" }, new string[] { searchUid.text });
            if (queryResult!= null)
            {
                DataTable table = queryResult.Tables[0];
                if (table.Rows.Count > 0)        
                {
                    //uid
                    searchFriend.uid = (int)table.Rows[0][0];
                    //username
                    searchFriend.friendName = table.Rows[0][1].ToString();
                    //picid
                    searchFriend.picid = (int)table.Rows[0][2];
                    // see console output to make sure searchFriend stores the current information
                    Debug.Log(searchFriend.uid);
                    Debug.Log(searchFriend.friendName);
                    Debug.Log(searchFriend.picid);
                }
                else
                {
                    Debug.Log("No search results. Please enter the correct uid. ");
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
