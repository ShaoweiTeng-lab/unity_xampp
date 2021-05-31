using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class Webtest : MonoBehaviour
{
    UnityWebRequest w_texture;
    
    public RawImage image;
    public Text message;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SendRequest());
        StartCoroutine(LoadRawImageFile());
        StartCoroutine(LoadTextFile());
        StartCoroutine(SendRequest());
    }

    // Update is called once per frame
    void Update()
    {

    }
    //IEnumerator SendRequest()
    //{
    //    Uri uri = new Uri("http://www.google.com"); //Uri 是 System 命名空间下的一个类,注意引用该命名空间
    //    UnityWebRequest uwr = new UnityWebRequest(uri);        //创建UnityWebRequest对象
    //    yield return uwr.SendWebRequest();                     //等待返回请求的信息
    //    if (uwr.isHttpError || uwr.isNetworkError)             //如果其 请求失败，或是 网络错误
    //    {
    //        print(uwr.error); //打印错误原因
    //    }
    //    else //请求成功
    //    {
    //        print("成功");
    //    }
    //}
    #region 抓圖片
    /// <summary>
    /// 抓取圖片用
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadRawImageFile() {
        #region 方法一
        w_texture = UnityWebRequestTexture.GetTexture("http://127.0.0.1/img/01.jpg");
        yield return w_texture.SendWebRequest();
        if (w_texture.isNetworkError || w_texture.isHttpError)
            Debug.LogError("沒抓到");
        else
        {

            image.texture = DownloadHandlerTexture.GetContent(w_texture);
            //image.SetNativeSize();
        }
        #endregion
        #region 方法二
        //WWW www = new WWW("http://127.0.0.1/img/01.jpg");
        //if (www.error != "null") {
        //    yield return www;
        //    image.texture = www.texture;

        //} 
        #endregion
    }
    #endregion

    #region 抓文字
    /// <summary>
    /// 抓取文字用
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadTextFile()
    {
        w_texture = UnityWebRequest.Get("http://127.0.0.1/txt/01.txt");
        yield return w_texture.SendWebRequest();
        if (w_texture.isNetworkError || w_texture.isHttpError)
            Debug.LogError("沒抓到");
        else
        {
            message.text = w_texture.downloadHandler.text;
            //image.SetNativeSize();
        }
    }
    #endregion

    /// <summary>
    /// 开启一个协程，发送请求
    /// </summary>
    /// <returns></returns>
    IEnumerator SendRequest()

    {
        Uri uri = new Uri("http://127.0.0.1/img/01.jpg"); //Uri 是 System 命名空间下的一个类,注意引用该命名空间
        UnityWebRequest uwr = new UnityWebRequest(uri);        //创建UnityWebRequest对象
        yield return uwr.SendWebRequest();                     //等待返回请求的信息
        if (uwr.isHttpError || uwr.isNetworkError)             //如果其 请求失败，或是 网络错误
        {
            print(uwr.error); //打印错误原因
        }
        else //请求成功
        {
            print("UnityWebRequest:請求成功");
            print(uwr.downloadedBytes);
        }

        print("--------------------");
        yield return SendRequest1(); //等待返回请求的信息
    }


    /// <summary>
    /// 开启一个协程，发送请求
    /// </summary>
    /// <returns></returns>
    IEnumerator SendRequest1()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://127.0.0.1/img/01.jpg"); //创建UnityWebRequest对象
        yield return uwr.SendWebRequest();                                 //等待返回请求的信息
        if (uwr.isHttpError || uwr.isNetworkError)                         //如果其 请求失败，或是 网络错误
        {
            
            print(uwr.error); //打印错误原因
        }
        else //请求成功
        { 
            print("Get::請求成功");
            print(uwr.downloadedBytes);
        }
    }
 
}
