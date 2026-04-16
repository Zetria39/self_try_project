using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("**my first script");
        //GameObject obj = this.gameObject;
        //string name = obj.name;
        //Debug.Log("**name:" + name);
        //Transform trans = obj.transform;
        //Vector3 pos = trans.position;
        //Debug.Log("**where:  " + pos);
        //this.gameObject.transform.position = new Vector3(1.5f, 2.5f, 3f);
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("**move:   ");
        //Vector3 pos = this.gameObject.transform.position;
        //pos.x += 0.001f;
        //this.gameObject.transform.position = pos;
        this.transform.Translate(0.01f, 0, 0, Space.Self);
        this.transform.Rotate(0, 1f, 0, Space.Self);
    }
}
