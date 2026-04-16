using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Main_Controller : MonoBehaviour
{
    private Player_Main_View main_View;

    //�����������ٿ�����ô�ã��򵥷���
    private static Player_Main_Controller controller = null;
    public static Player_Main_Controller Controller
    {
        get { return controller; }
    }
    //1.�������Ӱ
    public static void ShowMe()
    {
        if (controller == null)
        {
            GameObject res = Resources.Load<GameObject>("UI/Panel");
            GameObject obj = Instantiate(res);
            obj.transform.SetParent(GameObject.Find("Canvas").transform, false);

            controller = obj.GetComponent<Player_Main_Controller>();
        }
        controller.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public static void HideMe()
    {
        if (controller != null)
        {
            controller.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //��ȡͬ������ͬһ�������ϵ�view�ű�
        main_View = this.GetComponent<Player_Main_View>();
        //��һ�ν������
        main_View.UpdataInfo(Player_Data_Model.Data);

        //2.���� �¼��ļ��� ��������Ӧ��ҵ���߼�
        main_View.btnRole.onClick.AddListener(ClickRoleBtn);

        Player_Data_Model.Data.AddEventListener(UpdataInfo);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void ClickRoleBtn()
    {
        Player_Role_Controller.ShowMe();
    }

    //3.�������
    private void UpdataInfo(Player_Data_Model data)
    {
        if(main_View!=null)
            main_View.UpdataInfo(data);
    }


    //�мӾ��м�������start�
    private void OnDestroy()
    {
        Player_Data_Model.Data.RemoveEventListener(UpdataInfo);
    }
}
