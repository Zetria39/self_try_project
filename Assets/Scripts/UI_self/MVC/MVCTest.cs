using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVCTest : MonoBehaviour
{

    bool is_show = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !is_show)
        {
            Player_Main_Controller.ShowMe();
            is_show = true;
        }
        else if (Input.GetKeyDown(KeyCode.K) && is_show)
        {
            Player_Main_Controller.HideMe();
            is_show = false;
        }
    }
}
