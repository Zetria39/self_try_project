// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using System;
// using UnityEngine.SceneManagement;

// public class LoginClass : MonoBehaviour
// {
//     //进入前变量
//     public InputField username, password, confirmPassword;
//     public Text reminderText;
//     public int errorsNum;
//     public Button loginButton;
//     public GameObject hallSetUI, loginUI;
//     //进入后变量
//     public static string myUsername;

//     public void Register()
//     {
//         if (PlayerPrefs.GetString(username.text) == "")
//         {
//             if (password.text == confirmPassword.text)
//             {
//                 PlayerPrefs.SetString(username.text, username.text);
//                 PlayerPrefs.SetString(username.text + "password", password.text);
//                 reminderText.text = "注册成功！";
//             }
//             else
//             {
//                 reminderText.text = "两次密码输入不一致";
//             }
//         }
//         else
//         {
//             reminderText.text = "用户已存在";
//         }

//     }
//     private void Recovery()
//     {
//         loginButton.interactable = true;
//     }
//     public void Login()
//     {
//         if (PlayerPrefs.GetString(username.text) != "")
//         {
//             if (PlayerPrefs.GetString(username.text + "password") == password.text)
//             {
//                 reminderText.text = "登录成功";

//                 myUsername = username.text;
//                 hallSetUI.SetActive(true);
//                 loginUI.SetActive(false);
//                 SceneManager.LoadScene(1);
//             }
//             else
//             {
//                 reminderText.text = "密码错误";
//                 errorsNum++;
//                 if (errorsNum >= 3)
//                 {
//                     reminderText.text = "连续错误3次,请30秒后再试!";
//                     loginButton.interactable = false;
//                     Invoke("Recovery", 5);
//                     errorsNum = 0;
//                 }
//             }
//         }
//         else
//         {
//             reminderText.text = "账号不存在";
//         }
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class LoginClass : MonoBehaviour
{
    //进入前变量
    public InputField username, password, confirmPassword;
    public Text reminderText;
    public int errorsNum;
    public Button loginButton;
    public GameObject hallSetUI, loginUI;
    //进入后变量
    public static string myUsername;

    public void Register()
    {
        var users = LoadUsers();
        string nameKey = username.text.Trim();
        if (string.IsNullOrEmpty(nameKey))
        {
            reminderText.text = "用户名不能为空";
            return;
        }

        if (users.ContainsKey(nameKey))
        {
            reminderText.text = "用户已存在";
            return;
        }

        if (password.text != confirmPassword.text)
        {
            reminderText.text = "两次密码输入不一致";
            return;
        }

        users[nameKey] = HashPassword(password.text);
        SaveUsers(users);
        reminderText.text = "注册成功！";

    }
    private void Recovery()
    {
        loginButton.interactable = true;
    }
    public void Login()
    {
        var users = LoadUsers();
        string nameKey = username.text.Trim();
        if (!users.ContainsKey(nameKey))
        {
            reminderText.text = "账号不存在";
            return;
        }

        if (users[nameKey] == HashPassword(password.text))
        {
            reminderText.text = "登录成功";
            myUsername = nameKey;
            hallSetUI.SetActive(true);
            loginUI.SetActive(false);
            SceneManager.LoadScene("SampleScene",LoadSceneMode.Additive);
        }
        else
        {
            reminderText.text = "密码错误";
            errorsNum++;
            if (errorsNum >= 3)
            {
                reminderText.text = "连续错误3次,请30秒后再试!";
                loginButton.interactable = false;
                Invoke("Recovery", 5);
                errorsNum = 0;
            }
        }
    }

    // helpers: 文件路径 / 读写 / 密码散列
    string GetUserFilePath()
    {
        return Path.Combine(Application.dataPath, "users.json");
    }

    Dictionary<string, string> LoadUsers()
    {
        string path = GetUserFilePath();
        if (!File.Exists(path)) return new Dictionary<string, string>();
        try
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SerializableDict>(json)?.ToDictionary() ?? new Dictionary<string, string>();
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

    void SaveUsers(Dictionary<string, string> users)
    {
        var serial = new SerializableDict(users);
        string json = JsonUtility.ToJson(serial);
        File.WriteAllText(GetUserFilePath(), json);
        Debug.Log("Users saved to: " + GetUserFilePath());
    }

    string HashPassword(string pwd)
    {
        if (pwd == null) return "";
        using (var sha = SHA256.Create())
        {
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            StringBuilder sb = new StringBuilder();
            foreach (var b in data) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    [Serializable]
    public class SerializableDict
    {
        public List<string> keys = new List<string>();
        public List<string> values = new List<string>();

        public SerializableDict() { }

        public SerializableDict(Dictionary<string, string> dict)
        {
            foreach (var kv in dict)
            {
                keys.Add(kv.Key);
                values.Add(kv.Value);
            }
        }

        public Dictionary<string, string> ToDictionary()
        {
            var d = new Dictionary<string, string>();
            for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
                d[keys[i]] = values[i];
            return d;
        }
    }
}