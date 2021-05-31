using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using UnityEngine.UI;
public class SelectUser : MonoBehaviour
{
   
    string url = "http://localhost/upload/userSelect.php";
    [SerializeField] string[] userdata;
   
    void Start() {
        //StartCoroutine(start());
        StartCoroutine(GetRequest());
    }

    /// <summary>
    /// 用 www的方式
    /// </summary>
    /// <returns></returns>
    IEnumerator start() {
        WWW users = new WWW(url);
        yield return users;
        string usersdatastring = users.text;
        userdata = usersdatastring.Split(';');
        print(GetValueData(userdata[0], "username :"));
    }

    string GetValueData(string data, string index) {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    /// <summary>
    /// 用 get的方式
    /// </summary>
    /// <returns></returns>
    IEnumerator GetRequest()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url); //创建UnityWebRequest对象
        yield return uwr.SendWebRequest();                                 //等待返回请求的信息
        if (uwr.isHttpError || uwr.isNetworkError)                         //如果其 请求失败，或是 网络错误
        {

            print(uwr.error); //打印错误原因
        }
        else //请求成功
        {
            string usersdatastring = uwr.downloadHandler.text;
            userdata = usersdatastring.Split(';');
            print(usersdatastring);
        }
    }
}
