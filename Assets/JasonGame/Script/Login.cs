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
                        SendRequest.text = "成功登入";
                       isLogin = true;
                        LoginObj.SetActive(false);
                        AfterLoginObj.SetActive(true); 
                        NetManager.ins.userData = new UserData(UserName, www.downloadHandler.text, Password);
                        yield return new WaitForSeconds(1.5f); 
                        SendRequest.gameObject.SetActive(false);

                    }
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