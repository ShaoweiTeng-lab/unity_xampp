using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace JaosnGameNet
{
    public class Login : MonoBehaviour
    { 
        [SerializeField] Text SendRequest;
        string url = "http://localhost/UnityGame_PHP/Login.php";
        [SerializeField] Button SendLogin;
        [SerializeField] Button RegeistLogin;
        [SerializeField] InputField InputUserName, InputPassword; 
        public bool isLogin;
        public GameObject LoginObj;
        public GameObject AfterLoginObj;
        // Start is called before the first frame update
        void Start()
        {
            SendLogin.onClick.AddListener(() => Loginfunction());
            RegeistLogin.onClick.AddListener(() => SceneManager.LoadScene("Regist"));


        }
        void Loginfunction() {
            StartCoroutine(CorLogin(InputUserName.text, InputPassword.text)); 
          


        }
        /// <summary>
        /// 連線
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        IEnumerator CorLogin(string UserName, string Password)
        {

            WWWForm form = new WWWForm();
            form.AddField("loginUser", UserName);
            form.AddField("loginPassword", Password);
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
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
                    SendRequest.text = www.downloadHandler.text;
                    ///連線後做什麼
                    Debug.Log("ID  :  " + www.downloadHandler.text);
                    if (www.downloadHandler.text != "登入錯誤" && www.downloadHandler.text != "使用者 不存在" && www.downloadHandler.text != "請輸入帳密") {//判斷回傳是否成功登入
                        isLogin = true;
                        LoginObj.SetActive(false);
                        AfterLoginObj.SetActive(true);
                        NetManager.ins.userData = new UserData(UserName, www.downloadHandler.text, Password);
                    }
                }

            }

            if (NetManager.ins.userData != null) {
                StartCoroutine(GetItemsIds(NetManager.ins.userData.ReturnUserId())); 
            }

        }

        /// <summary>
        /// 登入後 用 玩家編號 取得該使用者的所有道具
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IEnumerator GetItemsIds(string userID) {//輸入 userid得到 該使用者的所有道具
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

                    string JsonArry = www.downloadHandler.text;
                    Debug.Log(JsonArry);
                    ///連線後做什麼
                    ///
                }

            }

        }
        

    }
}
/// <summary>
/// 使用者所有資訊存成class
/// </summary>
[SelectionBase]
public class UserData {
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
    public void ChangeInfo(string UserName, string UserPassword) {
        this.UserName = UserName;
        this.UserPassword = UserPassword;
    }
    public string ReturnUserId() {
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