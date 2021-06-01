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
    } 

}
