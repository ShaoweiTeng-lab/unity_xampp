using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Networking;
public class Login : MonoBehaviour
{   string url = "http://localhost/upload/Login.php";
    [SerializeField] Button SendLogin;
    [SerializeField] InputField InputUserName, InputPassword;
    // Start is called before the first frame update
    void Start()
    {
        SendLogin.onClick.AddListener(() => StartCoroutine(CorLogin(InputUserName.text, InputPassword.text)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator CorLogin(string UserName, string Password) {
       
        WWWForm form = new WWWForm();
        form.AddField("loginUser", UserName);
        form.AddField("loginPassword", Password);
        using (UnityWebRequest www = UnityWebRequest.Post(url,form)) {
            yield return www.SendWebRequest(); 
            if (www.isHttpError || www.isNetworkError)//沒連上網路
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                 
                Debug.Log(www.downloadHandler.text);
            }

        }

    }
}
