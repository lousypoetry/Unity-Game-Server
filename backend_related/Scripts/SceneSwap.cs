using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    private void OnGUI()
    {
        int xCenter = (Screen.width / 4);
        int yCenter = (Screen.height / 4);
        int width = 400;
        int height = 120;

        GUIStyle fontSize = new GUIStyle(GUI.skin.GetStyle("button"));
        fontSize.fontSize = 32;

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "RegistrationTest")
        {
            // Show a button to allow scene2 to be switched to.
            if (GUI.Button(new Rect(xCenter - width / 4, yCenter - height / 4, width, height), "Load FriendList", fontSize))
            {
                SceneManager.LoadScene("FriendList");
            }
        }
    }
}
