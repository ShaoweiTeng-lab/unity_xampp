using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
public class ImageManager : MonoBehaviour
{   //持續更新圖片
    //0.設定路徑
    //1.判斷圖片已存在
    //2.儲存圖片方法
    //3.載入圖片方法
    //4.get 圖片
    public static ImageManager ins;
    string basePth;
    // Start is called before the first frame update
    void Start()
    {
        if (ins == null)
        {
            ins = this;
        }
        else
            Destroy(this);
        //創建資料夾
        basePth = Application.persistentDataPath + "/Images/";
       
        if (!Directory.Exists(basePth))
            Directory.CreateDirectory(basePth);
        Debug.Log(basePth);

    }
    /// <summary>
    /// 判斷本地有無圖片
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    bool ImageExits(string name)
    {
        return File.Exists(basePth + name);
    }
    /// <summary>
    /// 儲存本地圖片
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ImageBytes"></param>
    public  void SaveImage(string name, byte[] ImageBytes)
    {
        Debug.Log(basePth + name);
        File.WriteAllBytes(basePth + name, ImageBytes);

    }
    public byte[] LoadImage(string name)
    {
        byte[] bytes = new byte[0];
        if (ImageExits(name))
        {
            bytes = File.ReadAllBytes(basePth + name);//如果有資料 讀資料 沒有則傳 空的 byte;
        }
        return bytes;

    }

}
