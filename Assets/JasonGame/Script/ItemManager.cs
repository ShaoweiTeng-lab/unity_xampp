using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;
using System.IO; 
public class ItemManager : MonoBehaviour
{   //記得先去下載SimpleJSON
    Action<string> ItemCreateCallback;
    public Transform ItemParent;
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
    //得到json 後做的事情
    IEnumerator CreateItemRoutine(string jsonArryString) {
        //解析Json arry 轉成 arry 
        Debug.Log("jsonArryString :  "+jsonArryString);
        SimpleJSON.JSONArray jsonArry = SimpleJSON.JSON.Parse(jsonArryString) as SimpleJSON.JSONArray;
        //Debug.Log("jsonArry.Count : " + jsonArry.Count);
        Debug.Log(jsonArry[0]["ItemID"]);
        if (jsonArry != null)
        {
            for (int i = 0; i < jsonArry.Count; i++)
            {
                bool isDone = false; //判斷是不是下載中
                                     //根據  ItemId(key) 抓取 value ,此作用為 輸入userId得到一組 Json 陣列然後 拆解成兩個object 然後 根據 AsObject["key"] 抓取 value
                string itemId = jsonArry[i]["ItemID"];
                //Debug.Log("ItemID: " + itemId);
                //Debug.Log(itemId);
                //定義一個jsonobj 負責裝 NetManager調用的訊息
                SimpleJSON.JSONObject itemInfoJson = new SimpleJSON.JSONObject();

                //定義一個 callback 區域方法從 NetManager調用 item的資訊 (name 說明等等)
                Action<string> getIttemInfo = (itemInfo) =>
                {
                    isDone = true;//資料傳遞進來了
                    SimpleJSON.JSONArray tempArry = SimpleJSON.JSON.Parse(itemInfo) as SimpleJSON.JSONArray;
                    itemInfoJson = tempArry[0].AsObject;
                };

                StartCoroutine(JaosnGameNet.NetManager.ins.CorGetItemsInfo(itemId, getIttemInfo));//這裡執行才讓isdone = true ,執行後才可跳至第二迴圈
                yield return new WaitUntil(() => isDone == true);// 等待isdone 執行完成 傳入必須為委派
                                                                 // Debug.Log("itemInfoJson : " + itemInfoJson.Count);
                                                                 //生成item物件
                GameObject prefeb = Resources.Load("Prefeb/ItemPrefeb") as GameObject;
                GameObject item = Instantiate(prefeb);
                item.transform.SetParent(ItemParent);
                //放入item資訊
                item.transform.Find("ItemName").GetComponent<Text>().text = itemInfoJson["Name"];
                item.transform.Find("ItemDetals").GetComponent<Text>().text = itemInfoJson["Description"];
                item.transform.Find("Price").GetComponent<Text>().text = itemInfoJson["Price"];
                ///抓 itemid 會有重複 因為 玩家可以有重複道具 所以必須抓唯一值(流水號)
                string onlyId = jsonArry[i].AsObject["Id"];
                //Debug.Log(onlyId);
                string userID= JaosnGameNet.NetManager.ins.userData.ReturnUserId();
                //定義賣出 button
                item.transform.Find("Sell").GetComponent<Button>().onClick.AddListener(() => { 

                    StartCoroutine(JaosnGameNet.NetManager.ins.CorSellItem(onlyId, userID, itemId));
                    
                    item.gameObject.SetActive(false);
                });

                //放入image
                //定義委派
                Action<Sprite> setImage = (itemImageSprite) => {
                    item.transform.Find("ItemImage").GetComponent<Image>().sprite = itemImageSprite; 
                };
                StartCoroutine(JaosnGameNet.NetManager.ins.CorGetItemImage(itemId, setImage));
            }

        }
        yield return null;
    
    }
}
