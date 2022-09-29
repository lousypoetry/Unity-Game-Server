using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Login : MonoBehaviour
{
    private string host = "120.77.148.135";
    private string port = "3306";     
    private string userName = "root";
    private string password = "penguinspy123456";
    private string databaseName = "penguintest";
    private MySqlAccess mysql;

     // required input data for this script
    public GameObject loginname;
    public GameObject loginpassword;
    public GameObject loginError;
    public int uid = -1;

    private void Start()
    {
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
    }
    public void LoginClick()
    {
        mysql.OpenSql();
        DataSet queryResult = mysql.Select("userinfo", new string[] { "userid" }, new string[] { "`" + "username" + "`", "`" + "password" + "`" }, new string[] { "=", "=" }, new string[] { loginname.GetComponent<TMP_InputField>().text,loginpassword.GetComponent<TMP_InputField>().text });
        if (queryResult != null)
        {
            DataTable table = queryResult.Tables[0];
            if (table.Rows.Count > 0)
            {
                // if successfully login, store the current uid
                uid = (int)table.Rows[0][0];
                Debug.Log("Current scene: Login Page. Login successfully." + " Userid: " + table.Rows[0][0]);
                mysql.CloseSql();
                // if successfully login, jump to next scene
                Scene scene = SceneManager.GetActiveScene();
                if (scene.name == "Login")
                {   
                    SceneManager.LoadScene("Homepage");
                }
                return;
            }
        }
        Debug.Log("Current scene: Login Page. Login failed.");
        loginError.SetActive(true);
        mysql.CloseSql();
    }
}