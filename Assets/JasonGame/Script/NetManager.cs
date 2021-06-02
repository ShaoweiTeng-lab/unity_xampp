using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace JaosnGameNet { 
    public class NetManager : MonoBehaviour
    {
        public static NetManager ins;
        public JaosnGameNet.Login userLogin;
        public UserData userData;
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

        /// <summary>
        /// 登入後 用 玩家編號 取得該使用者的所有道具
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerator CorGetItemsIds(string userID ,System.Action<string> callBack)
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
                    Debug.Log("上傳表單成功");
                    //取得連線資訊
                    // Debug.Log( www.downloadHandler.text);

                    string JsonArry = www.downloadHandler.text;//取得玩家 編號
                    Debug.Log(JsonArry);
                    ///連線後做什麼


                    callBack(JsonArry);//傳入Json arry並執行
                }

            }

        }

        /// <summary>
        /// 輸入 itemId 後得到 Item相關資訊
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public IEnumerator CorGetItemsInfo(string itemID, System.Action<string> callBack)
        {//輸入 userid得到 該使用者的所有道具
            string GetItemsIds = "http://localhost/UnityGame_PHP/GetItemID.php";
            WWWForm form = new WWWForm();
            form.AddField("itemID", itemID);
            using (UnityWebRequest www = UnityWebRequest.Post(GetItemsIds, form))
            {
                yield return www.SendWebRequest();
                if (www.isHttpError || www.isNetworkError)//沒連上網路
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("上傳表單成功");  
                    string JsonArry = www.downloadHandler.text;//取得item id 
                    ///連線後做什麼 
                    callBack(JsonArry);//傳入Json arry並執行
                }

            }

        }
    } 
     
}
