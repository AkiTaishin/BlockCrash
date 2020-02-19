using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    #region グローバル変数

    [SerializeField] private GameObject inputFieldObject = null;
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private ConvertFromJson convert = null;
    [SerializeField] private NetworkManager network = null;

    private bool ServerIsBusy = false;

    #endregion

    public void LoadtoMenu()
    {
        if (!SaveClass.HasPlayerData()) {
            //@todo１回かぎりのBOOL

            // ボタンより後にInputFieldの生成
            var obj = Instantiate(inputFieldObject, canvas.transform);

            if (obj != null)
                obj.GetComponent<ActiveInputField>().EnableInputField();
        }
        else 
        {

            // 通信中は何も受け付けないように
            if (!ServerIsBusy)
                StartCoroutine(WaitServerProcess()); 
        } 
    }

    private IEnumerator WaitServerProcess()
    {
        // 通信中は何も受け付けないように
        ServerIsBusy = true;

        // 通信処理を待つ
        yield return network.RequestSetUserInfo(convert.ConvertPlayerData());

        var data = SaveClass.LoadPlayerData();

        if(data.ID == "error")
        {
            // 不正なプレイヤーデータがローカルに入っているので削除
            SaveClass.DeletePlayerData();
            Debug.Log("たこやき");
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }

        ServerIsBusy = false;
    }
}
