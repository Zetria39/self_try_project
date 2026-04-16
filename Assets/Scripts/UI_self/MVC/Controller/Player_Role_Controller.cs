using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Role_Controller : MonoBehaviour
{
    private Player_Role_View role_View;
    private static Player_Role_Controller role_Controller = null;
    public static Player_Role_Controller Role_Controller
    {
        get
        {
            return role_Controller;
        }
    }

    public static void ShowMe()
    {
        if (role_Controller == null)
        {
            GameObject role_panel = Resources.Load<GameObject>("UI/Role_Panel");
            GameObject obj = Instantiate(role_panel);
            obj.transform.SetParent(GameObject.Find("Canvas").transform, false);

            role_Controller = obj.GetComponent<Player_Role_Controller>();
        }
        role_Controller.gameObject.SetActive(true);
    }

    public static void HideMe()
    {
        if (role_Controller != null){
            role_Controller.gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        role_View = this.GetComponent<Player_Role_View>();
        role_View.UpdataInfo(Player_Data_Model.Data);

        role_View.btnClose.onClick.AddListener(ClickCloseBtn);
        role_View.btnLevUp.onClick.AddListener(ClickLevUpBtn);
        Player_Data_Model.Data.AddEventListener(UpdataInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void ClickCloseBtn()
    {
        HideMe();
    }


    private void ClickLevUpBtn()
    {
        Player_Data_Model.Data.LevUp();
    }

    private void UpdataInfo(Player_Data_Model data)
    {
        if (role_View != null)
            role_View.UpdataInfo(data);
    }

    //有加就有减（加在start里）
    private void OnDestroy()
    {
        Player_Data_Model.Data.RemoveEventListener(UpdataInfo); 
    }
}
