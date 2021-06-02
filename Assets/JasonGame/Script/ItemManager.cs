using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO; 
public class ItemManager : MonoBehaviour
{   //記得先去下載SimpleJSON
    Action<string> ItemCreateCallback;
    // Start is called before the first frame update
    void Start()
    {   //定義委派方法
        ItemCreateCallback = (JsonArryString) => {
            StartCoroutine(CreateItemRoutine(JsonArryString)); 
        };
        //執行
        CreateItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateItems() {
        string userId = JaosnGameNet.NetManager.ins.userData.ReturnUserId();
        StartCoroutine(JaosnGameNet.NetManager.ins.CorGetItemsIds(userId, ItemCreateCallback)); 
    }
    //得到json 後座的事情
    IEnumerator CreateItemRoutine(string jsonArryString) {
        //解析Json arry 轉成 arry 
        SimpleJSON.JSONArray jsonArry = SimpleJSON.JSON.Parse(jsonArryString) as SimpleJSON.JSONArray;
        
        for (int i = 0; i < jsonArry.Count; i++)
        {
            bool isDone = false; ;//判斷是不是下載中
            //根據  ItemId(key) 抓取 value ,此作用為 輸入userId得到一組 Json 陣列然後 拆解成兩個object 然後 根據 AsObject["key"] 抓取 value
            string itemId = jsonArry[i].AsObject["ItemId"];
            Debug.Log(itemId);
            //定義一個jsonobj 負責裝 NetManager調用的訊息
            SimpleJSON.JSONObject itemInfoJson = new SimpleJSON.JSONObject();

            //定義一個 callback 區域方法從 NetManager調用 item的資訊 (name 說明等等)
            Action<string> getIttemInfo = (itemInfo) => {
                isDone = true;//資料傳遞進來了
                SimpleJSON.JSONArray tempArry = SimpleJSON.JSON.Parse(itemInfo) as SimpleJSON.JSONArray;
                itemInfoJson = tempArry[0].AsObject; 
            };

            StartCoroutine(JaosnGameNet.NetManager.ins.CorGetItemsInfo(itemId, getIttemInfo));//這裡執行才讓isdone = true ,執行後才可跳至第二迴圈
            yield return new WaitUntil(()=>isDone==true);// 等待isdone 執行完成 傳入必須為委派

        }


        yield return null;
    
    }
}
