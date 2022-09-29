using System.Data;
using UnityEngine;
using UnityEngine.UI;


// Checking gallery from an input uid.

public class CheckGalleryFromUID : MonoBehaviour
{
    private string host = "120.77.148.135";
    private string port = "3306";
    private string userName = "root";
    private string password = "penguinspy123456";
    private string databaseName = "penguintest";
    private MySqlAccess mysql;

     // required input data for this script
    public string myUid;
    public int[] myPenguins = new int[10];

    private void Start()
    {
        myUid = GameObject.FindGameObjectWithTag("RegistrationTag").GetComponent<GalleryManager>().uid;
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
    }
    public void CheckGalleryClick()
    {
        mysql.OpenSql();
        DataSet queryResult = mysql.Select("userinfo", new string[] { "penguin1", "penguin2", "penguin3", "penguin4", "penguin5", "penguin6" }, new string[] { "`" + "userid" + "`" }, new string[] { "=" }, new string[] { myUid });
        if (queryResult != null)
        {
            DataTable table = queryResult.Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    myPenguins[i] = (int)table.Rows[0][i];
                }
                mysql.CloseSql();
            }
            for (int i = 0; i < 6; i++)
            {
                Debug.Log(myPenguins[i]);
            }
        }
        mysql.CloseSql();
    }
}
