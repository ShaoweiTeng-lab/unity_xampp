using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
namespace JaosnGameNet
{
    public class NetManager : MonoBehaviour
    {
        public static NetManager ins;
        public JaosnGameNet.Login userLogin;
        public UserData userData;
        public GameObject userGameObject;
        private void Awake()
        {
            if (ins != null)
                Destroy(this);
            else
                ins = this;
        }
        void Start()
        {
            userLogin = transform.GetComponent<JaosnGameNet.Login>();

        }

        // Update is called once per frame
        void Update()
        {
             
        }

        #region 登入後取得玩家編號
        /// <summary>
        /// 登入後 用 玩家編號 取得該使用者的所有道具
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerator CorGetItemsIds(string userID, System.Action<string> callBack)
        {//輸入 userid得到 該使用者的所有道具
            string GetItemsIds = "http://localhost/UnityGame_PHP/GetUserItem.php";
            WWWForm form = new WWWForm();
            form.AddField("userId", userID);
            using (UnityWebRequest www = UnityWebRequest.Post(GetItemsIds, form))
            {
                yield return www.SendWebRequest();
                if (www.isHttpError || www.isNetworkError)//沒連上網路
                {
                    Debug.Log(www.error);
                }
                else
                {

                    //取得連線資訊
                    // Debug.Log( www.downloadHandler.text);

                    string JsonArry = www.downloadHandler.text;//取得玩家 編號
                    //Debug.Log(JsonArry);
                    ///連線後做什麼


                    callBack(JsonArry);//傳入Json arry並執行
                }

            }

        }
        #endregion
        #region 取得道具資訊
        /// <summary>
        /// 輸入 itemId 後得到 Item相關資訊
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public IEnumerator CorGetItemsInfo(string itemID, System.Action<string> callBack)
        {//輸入 userid得到 該使用者的所有道具
            string GetItemsIdsUrl = "http://localhost/UnityGame_PHP/GetItemID.php";
            WWWForm form = new WWWForm();
            form.AddField("itemid", itemID);
            using (UnityWebRequest www = UnityWebRequest.Post(GetItemsIdsUrl, form))
            {
                yield return www.SendWebRequest();
                if (www.isHttpError || www.isNetworkError)//沒連上網路
                {
                    Debug.Log(www.error);
                }
                else
                {

                    string JsonArry = www.downloadHandler.text;//取得item id 
                    Debug.Log(JsonArry);
                    ///連線後做什麼 
                    callBack(JsonArry);//傳入Json arry並執行
                }

            }

        }

        #endregion

        #region 取得玩家資訊
        /// <summary>
        /// 拿到該玩家的所有資訊
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IEnumerator CorGetUserDataInfo(string username)
        {
            string GetItemsIdsUrl = "http://localhost/UnityGame_PHP/userSelect.php";
            WWWForm form = new WWWForm();
            form.AddField("userName", username);
            using (UnityWebRequest www = UnityWebRequest.Post(GetItemsIdsUrl, form))
            {
                yield return www.SendWebRequest();
                if (www.isHttpError || www.isNetworkError)//沒連上網路
                {
                    Debug.Log(www.error);
                }
                else
                {
                    string JsonArry = www.downloadHandler.text;
                    ////解析Json arry 轉成 arry 
                    SimpleJSON.JSONArray jsonArry = SimpleJSON.JSON.Parse(JsonArry) as SimpleJSON.JSONArray;
                    //Debug.Log("jsonArry count : " +jsonArry.Count);
                    //定義一個jsonobj
                    SimpleJSON.JSONObject UserInfoJson = new SimpleJSON.JSONObject(); 
                    UserInfoJson = jsonArry[0].AsObject;//將arry放入obj
                    //Debug.Log("UserInfoJson count: "+UserInfoJson.Count);
                    //放入item資訊
                    userGameObject.transform.Find("UserName").GetComponent<Text>().text = "Name : "+UserInfoJson["username"];
                    userGameObject.transform.Find("Level").GetComponent<Text>().text = "Level : " + UserInfoJson["Level"];
                    userGameObject.transform.Find("UserCoint").GetComponent<Text>().text = "Coins : " + UserInfoJson["coins"];
                }

            }
        }
        #endregion


        /// <summary>
        /// 刪除道具
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ///抓 itemid 會有重複 因為 玩家可以有重複道具 所以必須抓唯一值(流水號)
        public IEnumerator CorSellItem(string onlyId, string userID, string itemId )//防止玩家刪掉瞭個一樣的道具(玩家有兩個相同道具的情況下)
        {
            string SellItemUrl = "http://localhost/UnityGame_PHP/SellItm.php";
            WWWForm form = new WWWForm();
            Debug.Log(onlyId + " " + userID + " " + itemId);
            form.AddField("userId", userID);
            form.AddField("onlyId", onlyId);
            form.AddField("itemId", itemId);
            bool isupdate = false;
            using (UnityWebRequest www = UnityWebRequest.Post(SellItemUrl, form))
            {
                yield return www.SendWebRequest();
                isupdate = true;
                if (www.isHttpError || www.isNetworkError)//沒連上網路
                {
                    Debug.Log(www.error);
                }
                else
                {
                    yield return new WaitUntil(() => isupdate == true); 
                    StartCoroutine(CorGetUserDataInfo(userData.ReturnUserName()));//更新所有資料
                    Debug.Log(www.downloadHandler.text);
                }

            }



        }
 
    }
    
   
}


/// <summary>
/// 使用者所有資訊存成class
/// </summary>
[SelectionBase]
public class UserData
{
    [SerializeField]
    string UserName;
    [SerializeField]
    string UserID;
    [SerializeField]
    string UserPassword;
    [SerializeField]
    int UserCoins;
    [SerializeField]
    int Level;
    public UserData(string UserName, string UserId, string UserPassword)
    {
        this.UserName = UserName;
        this.UserID = UserId;
        this.UserPassword = UserPassword;

    }
    public void ChangeInfo(string UserName, string UserPassword)
    {
        this.UserName = UserName;
        this.UserPassword = UserPassword;
    }
    public string ReturnUserId()
    {
        return UserID;

    }
    public string ReturnUserName()
    {
        return UserName;
    }
    public string ReturnUserPasswordd()
    {
        return UserPassword;

    }
    public int ReturnUserCoins()
    {
        return UserCoins;

    }
    public int ReturnLevel()
    {
        return Level;

    }


}
