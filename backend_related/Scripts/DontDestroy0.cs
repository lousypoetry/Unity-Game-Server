using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy0 : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GalleryTag");

        if (objs.Length > 1) 
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
