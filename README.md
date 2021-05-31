# unity_xampp
 unity + xampp 連線至 資料庫 ，製作 登入註冊等功能demo
 
 
UnityWebRequest
UnityWebRequest 類別設計方式分成三塊：

UnityWebRequest：負責處理 HTTP 協定的傳輸處理

UploadHandler：負責將資源匯整 (Marshal) 成二進位資料傳給遠端伺服器

DownloadHandler：處理下載的二進位資料，以及最後將資料處理成應用層可用的資源。

使用 UnityWebRequest 的流程如下：

建立 UnityWebRequest
設定 UploadHandler 傳給遠端伺服器的內容（可以不傳）
設定 DownloadHandler 處理遠端伺服器回傳的資料（可以不收，例如心跳封包 Acknowledgement, ACK）
呼叫 Send() 等待回應 (yield return SendWebRequest)
檢查是否有錯誤 isError
最後從 DownloadHandler 取得回應資料處理
UML 圖:
![image](https://user-images.githubusercontent.com/50354880/120175216-8b8a8400-c238-11eb-99d9-a13c989251a6.png)


userId 代表使用者編號(唯一 )  IteID代表 道具編號  
可透過 查詢 UserId獲得所有該使用者擁有道具

![image](https://user-images.githubusercontent.com/50354880/120174759-0901c480-c238-11eb-9b9f-6c3e20931edf.png)

