using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Main_View : MonoBehaviour
{
    //1.找控件
    public Button btnRole;
    public Button btnSkill;

    public Text txtName;
    public Text txtLev;
    public Text txtMoney;
    public Text txtGem;
    public Text txtPower;

    //提供面板更新的相关方法给外部
    public void UpdataInfo(Player_Data_Model data)
    {
        txtName.text = data.PlayerName;
        txtLev.text = "Lev." + data.Lev.ToString();
        txtMoney.text = data.Money.ToString();
        txtGem.text = data.Gem.ToString();
        txtPower.text = data.Power.ToString();
    }

}
