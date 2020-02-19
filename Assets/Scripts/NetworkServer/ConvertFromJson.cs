using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertFromJson : MonoBehaviour
{
    #region グローバル変数

    /// <summary>
    /// 外部からしか触りたくない
    /// </summary>
    [SerializeField] private NetworkManager networkManager = null;


    #endregion

    /// <summary>
    /// 呼び出し時に矢印の塊を引数actとして渡す
    /// </summary>
    private void Start()
    {
        // プレイヤーの情報を上書きする（ラムダ記法）
        // str = プレイヤーデータの各情報
        networkManager.SetOnFinishedSetUserInfo(

            (string str) =>
            {
                var data = JsonUtility.FromJson<PlayerData>(str);

                SaveClass.SavePlayerData(data);
            }

        );

        // プレイヤーの情報を上書きする（ラムダ記法）
        // str = プレイヤーデータの各情報
        networkManager.SetOnFinishedGetUserInfo(

            (string str)=>
            {
                var data = JsonUtility.FromJson<PlayerData>(str);




                SaveClass.SavePlayerData(data);
            }

        );

        // 全プレイヤーの情報を引っこ抜く
        // str = 全プレイヤーデータが格納されている配列
        networkManager.SetOnFinishedGetAllUserInfo(

            (string str)=>
            {
                var dataListString = ("{\"list\": " + str + "}");
                var data = JsonUtility.FromJson<PlayerDataList>(dataListString);

                var newData = data.sort();

                SaveClass.AllPlayerStatus(newData);

                Debug.Log(data);

            }

        );
    }

   
    /// <summary>
    /// プレイヤーデータ(中身全部)をJson形式に変換して返す
    /// </summary>
    /// <returns>Json形式にしたプレイヤーデータdata</returns>
    public string ConvertPlayerData()
    {
        string data = JsonUtility.ToJson( SaveClass.LoadPlayerData());

        return data;
    }

    /// <summary>
    /// ぷれいやーデータのIDを返す
    /// </summary>
    /// <returns></returns>
    public string ConvertPlayerId()
    {
        string data = SaveClass.LoadPlayerData().ID;

        return data;
    }
}


#region 通信中は何も受け付けないように

// bool hogeIsBusy = false;
//private IEnumerator hoge()
//{
//    hogeIsBusy = true;
//    SaveClass.LoadPlayerData();

//    yield return network.RequestGetAllUserInfo();

//    SceneManager.LoadScene("Menu");
//    hogeIsBusy = false;
//}

#endregion
