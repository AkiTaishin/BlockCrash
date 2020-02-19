using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    #region グローバル変数

    private static readonly string serverUrl = "localhost:8080";
    private static readonly string setUserInfo = "/saveUserData";
    private static readonly string getUserInfo = "/sendUserData";
    private static readonly string getAllUser = "/sendAllUserData";
   // private static readonly string serverUrl = "http://ygp0035-pc:8000";
    //private static readonly string setUserInfo = "/setUserInfo";
    //private static readonly string getUserInfo = "/sendUserInfo";
    //private static readonly string getAllUser = "/sendAllUser";

    private System.Action<string> onFinishedSetUserInfo = null;
    private System.Action<string> onFinishedGetUserInfo = null;
    private System.Action<string> onFinishedGetAllUserInfo = null;

    #endregion

    #region　変換
    /// <summary>
    /// 帰ってきたJson形式のデータをPlayerData形式に変換するためのもの
    /// </summary>
    /// <param name="act"></param>
    public void SetOnFinishedSetUserInfo(System.Action<string> act)
    {
        onFinishedSetUserInfo = act;
    }

    /// <summary>
    /// 帰ってきたJson形式のデータをPlayerData形式に変換するためのもの
    /// </summary>
    /// <param name="act"></param>
    public void SetOnFinishedGetUserInfo(System.Action<string> act)
    {
        onFinishedGetUserInfo = act;
    }


    /// <summary>
    /// ユーザーリスト（PlayerData配列）
    /// </summary>
    /// <param name="act"></param>
    public void SetOnFinishedGetAllUserInfo(System.Action<string> act)
    {
        onFinishedGetAllUserInfo = act;
    }
    #endregion

    #region　リクエストの管理

    /// <summary>
    /// PlayerDataをToJsonしたものを渡す
    /// ユーザー情報の書き換え（Id, name, scoreをまとめて）
    /// 書き換え、新規登録
    /// </summary>
    /// <param name="sendMess">PlayerDataのToJsonしたやつ</param>
    /// <returns></returns>
    public Coroutine RequestSetUserInfo(string sendMess)
    {
        return StartCoroutine(WebRequestPost(serverUrl+ setUserInfo, sendMess, onFinishedSetUserInfo));
    }

    /// <summary>
    /// 引数のIDに該当したプレイヤー情報を取得する
    /// 取得するだけ{ }
    /// IDが存在しない場合はここでは何もしない。エラーになる。
    /// </summary>
    /// <param name="sendMess">プレイヤーID</param>
    /// <returns></returns>
    public Coroutine RequestGetUserInfo(string sendMess)
    {
        return StartCoroutine(WebRequestPost(serverUrl + getUserInfo, sendMess, onFinishedGetUserInfo));
    }

    /// <summary>
    /// PlayerData配列をそのまま取得する[{}. {}. {}]
    /// </summary>
    /// <returns></returns>
    public Coroutine RequestGetAllUserInfo()
    {
        return StartCoroutine(WebRequestGet(serverUrl + getAllUser, onFinishedGetAllUserInfo));
    }

    #endregion

   

    #region 通信の共通処理

    /// <summary>
    /// データを参照するための関数
    /// ランキング処理とかで使うかもしれない
    /// </summary>
    /// <param name="uri">固定のURI</param>
    /// <param name="onRecieved">中身こそ違うがサーバーから受け取った各種情報をもとに行う処理(action)</param>
    /// <returns></returns>
    private IEnumerator WebRequestGet(string uri, System.Action<string> onRecieved)
    {
        var WebRequest = new UnityWebRequest(uri, "GET");
        WebRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return WebRequest.SendWebRequest();

        if (onRecieved != null)
        {
            onRecieved(WebRequest.downloadHandler.text);
        }


        Debug.Log("GET_Received: " + WebRequest.downloadHandler.text);
    }

    /// <summary>
    /// データを保存したり上書きするための関数
    /// </summary>
    /// <param name="uri">固定</param>
    /// <param name="sendMess">送りたい内容（1..*）</param>
    /// <param name="onRecieved">中身こそ違うがサーバーから受け取った各種情報をもとに行う処理(action)</param>
    /// <returns></returns>
    private IEnumerator WebRequestPost(string uri, string sendMess, System.Action<string> onRecieved)
    {
        if (!string.IsNullOrEmpty(sendMess))
        {
            var WebRequest = new UnityWebRequest(uri, "POST");
            var bytes = System.Text.Encoding.UTF8.GetBytes(sendMess);
            WebRequest.uploadHandler = new UploadHandlerRaw(bytes);
            WebRequest.downloadHandler = new DownloadHandlerBuffer();

            WebRequest.SetRequestHeader("Content-Type", "application/json");

            yield return WebRequest.SendWebRequest();

            if (onRecieved != null)
            {
                onRecieved(WebRequest.downloadHandler.text);

            }

            Debug.Log("POST_Received: " + WebRequest.downloadHandler.text);
        }
    }

    #endregion
}
