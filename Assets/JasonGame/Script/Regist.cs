using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace JaosnGameNet {

    public class Regist : MonoBehaviour
    {
        string url = "http://localhost/UnityGame_PHP/userInsert.php";
        [SerializeField] Button SendAdduser;
        [SerializeField] InputField InputUserName, InputEmail, InputPassword;
        [SerializeField] Text SendMessage;
        // Start is called before the first frame update
        void Start()
        {
            SendAdduser.onClick.AddListener(() => StartCoroutine(AddUser(InputUserName.text, InputEmail.text, InputPassword.text)));
        }

         /// <summary>
          /// 新增使用者
          /// </summary>
          /// <param name="UserName"></param>
          /// <param name="email"></param>
          /// <param name="password"></param>
          /// <returns></returns>
        // Update is called once per frame
        IEnumerator AddUser(string UserName, string email, string password)
        {
            WWWForm form = new WWWForm();//產生 form格式
            form.AddField("addUsername", UserName);
            form.AddField("addEmail", email);
            form.AddField("addPassword", password);
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))//用post方式上傳
            {
                yield return www.SendWebRequest();//等待上傳

                if (www.isHttpError || www.isNetworkError)//沒連上網路
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Form upload complete!");
                    SendMessage.text=www.downloadHandler.text;

                    if (SendMessage.text.Equals( "創建成功")) {
                        yield return new WaitForSeconds(1.5f);
                        SceneManager.LoadScene("Login");
                    }
                         

                }
            }
        }
    } 

}
