using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System;


// public class Player_Data_Model
// {
//     //������ʹ���ⲿ�ɵò��ɸ�
//     private string playerName;
//     public string PlayerName
//     {
//         get
//         {
//             return playerName;
//         }
//     }
    
//     private int lev;
//     public int Lev
//     {
//         get { return lev; }
//     }

//     private int money;
//     public int Money
//     {
//         get { return money; }
//     }

//     private int gem; 
//     public int Gem
//     {
//         get { return gem; }
//     }

//     private int power; 
//     public int Power
//     {
//         get { return power; }
//     }

//     private int hp; 
//     public int Hp
//     {
//         get { return hp; }
//     }

//     private int atk; 
//     public int Atk
//     {
//         get { return atk; }
//     }

//     private int def; 
//     public int Def
//     {
//         get { return def; }
//     }

//     private int crit; 
//     public int Crit
//     {
//         get { return crit; }
//     }

//     private int miss; 
//     public int Miss
//     {
//         get { return miss; }
//     }

//     private int luck; 
//     public int Luck
//     {
//         get { return luck; }
//     }

//     private static Player_Data_Model data = null;

//     public static Player_Data_Model Data
//     {
//         get
//         {
//             if (data == null)
//             {
//                 data = new Player_Data_Model();
//                 data.Init();
//             }
//             return data;
//         }
//     }

//     //֪ͨ�ⲿ���ݸ��µ��¼�
//     //ͨ��������ⲿ������ϵ
//     private event UnityAction<Player_Data_Model> updateEvent;

//     public void Init()
//     {
//         playerName = PlayerPrefs.GetString("PlayerName", "ZZZZ");
//         lev = PlayerPrefs.GetInt("PlayerLev", 1);
//         money = PlayerPrefs.GetInt("PlayerMoney", 9999);
//         gem = PlayerPrefs.GetInt("PlayerGem", 8888);
//         power = PlayerPrefs.GetInt("PlayerPower", 99);

//         hp = PlayerPrefs.GetInt("PlayerHp", 100);
//         atk = PlayerPrefs.GetInt("PlayerAtk", 20);
//         def = PlayerPrefs.GetInt("PlayerDef", 10);
//         crit = PlayerPrefs.GetInt("PlayerCrit", 20);
//         miss = PlayerPrefs.GetInt("PlayerMiss", 10);
//         luck = PlayerPrefs.GetInt("PlayerLuck", 40);
//     }

//     public void LevUp()
//     {
//         lev++;
//         hp += 100;
//         atk += 10;
//         def += 10;
//         crit += 5;
//         miss += 2;
//         luck += 1;
//         Save();
//     }

//     public void Save()
//     {
//         PlayerPrefs.SetString("PlayerName", playerName);
//         PlayerPrefs.SetInt("PlayerLev", lev);
//         PlayerPrefs.SetInt("PlayerMoney", money);
//         PlayerPrefs.SetInt("PlayerGem", gem);
//         PlayerPrefs.SetInt("PlayerPower", power);

//         PlayerPrefs.SetInt("PlayerHp", hp);
//         PlayerPrefs.SetInt("PlayerAtk", atk);
//         PlayerPrefs.SetInt("PlayerDef", def);
//         PlayerPrefs.SetInt("PlayerCrit", crit);
//         PlayerPrefs.SetInt("PlayerMiss", miss);
//         PlayerPrefs.SetInt("PlayerLuck", luck);
//         UpdateInfo();
//     }

//     public void AddEventListener(UnityAction<Player_Data_Model> function)
//     {
//         updateEvent += function;
//     }
//     public void RemoveEventListener(UnityAction<Player_Data_Model> function)
//     {
//         updateEvent -= function;
//     }

//     private void UpdateInfo()
//     {
//         if(updateEvent != null)
//         {
//             updateEvent(this);
//         }
//     }
// }

public class Player_Data_Model
{
    // 内部可序列化数据结构
    [Serializable]
    private class PlayerData
    {
        public string playerName = "ZZZZ";
        public int lev = 1;
        public int money = 9999;
        public int gem = 8888;
        public int power = 99;
        public int hp = 100;
        public int atk = 20;
        public int def = 10;
        public int crit = 20;
        public int miss = 10;
        public int luck = 40;
    }

    private PlayerData dataObj = new PlayerData();

    // 外部只读属性（保留原接口）
    private string playerName;
    public string PlayerName { get { return playerName; } }

    private int lev;
    public int Lev { get { return lev; } }

    private int money;
    public int Money { get { return money; } }

    private int gem;
    public int Gem { get { return gem; } }

    private int power;
    public int Power { get { return power; } }

    private int hp;
    public int Hp { get { return hp; } }

    private int atk;
    public int Atk { get { return atk; } }

    private int def;
    public int Def { get { return def; } }

    private int crit;
    public int Crit { get { return crit; } }

    private int miss;
    public int Miss { get { return miss; } }

    private int luck;
    public int Luck { get { return luck; } }

    private static Player_Data_Model data = null;
    public static Player_Data_Model Data
    {
        get
        {
            if (data == null)
            {
                data = new Player_Data_Model();
                data.Init();
            }
            return data;
        }
    }

    private event UnityAction<Player_Data_Model> updateEvent;

    // 初始化：优先从 json 文件加载；若无文件则从 PlayerPrefs 迁移（兼容老数据）并保存为 json
    public void Init()
    {
        if (!LoadFromFile())
        {
            // 读取 PlayerPrefs（兼容老存储），然后写入文件
            dataObj.playerName = PlayerPrefs.GetString("PlayerName", dataObj.playerName);
            dataObj.lev = PlayerPrefs.GetInt("PlayerLev", dataObj.lev);
            dataObj.money = PlayerPrefs.GetInt("PlayerMoney", dataObj.money);
            dataObj.gem = PlayerPrefs.GetInt("PlayerGem", dataObj.gem);
            dataObj.power = PlayerPrefs.GetInt("PlayerPower", dataObj.power);

            dataObj.hp = PlayerPrefs.GetInt("PlayerHp", dataObj.hp);
            dataObj.atk = PlayerPrefs.GetInt("PlayerAtk", dataObj.atk);
            dataObj.def = PlayerPrefs.GetInt("PlayerDef", dataObj.def);
            dataObj.crit = PlayerPrefs.GetInt("PlayerCrit", dataObj.crit);
            dataObj.miss = PlayerPrefs.GetInt("PlayerMiss", dataObj.miss);
            dataObj.luck = PlayerPrefs.GetInt("PlayerLuck", dataObj.luck);

            SaveToFile();
        }

        // 把 dataObj 的值同步到字段并通知监听者
        SyncFromDataObj();
        UpdateInfo();
    }

    public void LevUp()
    {
        dataObj.lev++;
        dataObj.hp += 100;
        dataObj.atk += 10;
        dataObj.def += 10;
        dataObj.crit += 5;
        dataObj.miss += 2;
        dataObj.luck += 1;
        Save();
    }

    public void Save()
    {
        // 将字段保存到 dataObj（如果有外部直接修改字段的场景）
        // 这里我们以 dataObj 为主，确保字段同步
        SaveToFile();
        SyncFromDataObj();
        UpdateInfo();
    }

    private string GetFilePath()
    {
        return Path.Combine(Application.dataPath, "player_data.json");
    }

    private bool LoadFromFile()
    {
        try
        {
            string path = GetFilePath();
            if (!File.Exists(path)) return false;
            string json = File.ReadAllText(path);
            if (string.IsNullOrEmpty(json)) return false;
            dataObj = JsonUtility.FromJson<PlayerData>(json) ?? new PlayerData();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Load player data failed: " + e);
            return false;
        }
    }

    private void SaveToFile()
    {
        try
        {
            string json = JsonUtility.ToJson(dataObj, true);
            string path = GetFilePath();
            File.WriteAllText(path, json);
            Debug.Log("Player data saved to: " + path);
        }
        catch (Exception e)
        {
            Debug.LogError("Save player data failed: " + e);
        }
    }

    // 把 dataObj 的值推到暴露的字段
    private void SyncFromDataObj()
    {
        playerName = dataObj.playerName;
        lev = dataObj.lev;
        money = dataObj.money;
        gem = dataObj.gem;
        power = dataObj.power;
        hp = dataObj.hp;
        atk = dataObj.atk;
        def = dataObj.def;
        crit = dataObj.crit;
        miss = dataObj.miss;
        luck = dataObj.luck;
    }

    // 事件系统
    public void AddEventListener(UnityAction<Player_Data_Model> function)
    {
        updateEvent += function;
    }
    public void RemoveEventListener(UnityAction<Player_Data_Model> function)
    {
        updateEvent -= function;
    }

    private void UpdateInfo()
    {
        if (updateEvent != null)
        {
            updateEvent(this);
        }
    }

    // 可选：外部修改数据的简单接口（保持封装性）
    public void SetPlayerName(string name) { dataObj.playerName = name; Save(); }
    public void AddMoney(int delta) { dataObj.money += delta; Save(); }
    public void SetMoney(int v) { dataObj.money = v; Save(); }
    // 根据需要添加更多修改接口...
}
