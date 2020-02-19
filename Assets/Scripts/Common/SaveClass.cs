using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 値を格納しておくためのClass
/// </summary>
public class SaveClass
{
    #region グローバル変数

    private static bool IsClear = false;
    /// <summary>
    /// バーを書けるか書けないか
    /// </summary>
    private static bool IsDraw = false;

    private static bool IsGameOver = false;

    /// <summary>
    /// ゲームモードの管理
    /// trueでNormalモード
    /// falseでInfiniteモード
    /// </summary>
    private static bool bNowGameMode = true;

    private static readonly string KeyPlayerName = "key";

    private static List<PlayerData> data = new List<PlayerData>();

    #endregion


    public static void SetGameMode(bool Mode)
    {
        // ゲームモードを保存
        bNowGameMode = Mode;
    }

    public static bool GetGameMode()
    {
        // ゲームモードの取得
        return bNowGameMode;
    }

    /// <summary>
    /// GetIsClear
    /// </summary>
    /// <returns>IsClear</returns>
    public static bool GetIsClear()
    {
        return IsClear;
    }

    /// <summary>
    /// SetIsClear = true;
    /// </summary>
    public static void SetIsClear(bool Pram)
    {
        IsClear = Pram;
    }

    public static bool GetIsDraw()
    {
        return IsDraw;
    }

    public static void SetIsDraw(bool Pram)
    {
        IsDraw = Pram;
    }

    public static void SetGameOver( bool Pram )
    {
        IsGameOver = Pram;
    }

    public static bool GetGameOver()
    {
        return IsGameOver;
    }

    public static void AllPlayerStatus(List<PlayerData> sort)
    {
        data = sort;
    }

    public static List<PlayerData> GetAllPlayerStatus()
    {
        return data;
    }

    /// <summary>
    /// タイトル後の名前設定
    /// </summary>
    public static void SaveUserName(string name)
    {
        PlayerData currentPlayerData = SaveClass.LoadPlayerData();

        if (currentPlayerData == null)
        {
            // 存在しない場合は新たに生成
            currentPlayerData = new PlayerData(name);
        }
        else
        {
            // 存在する場合は名前の上書き
            currentPlayerData.Name = name;
        }

        // プレイヤーデータを保存
        SaveClass.SavePlayerData(currentPlayerData);

    }

    /// <summary>
    /// 引数で受け取った値(PlayerDataクラス)をJsonに変換
    /// プレイヤー情報の保存
    /// </summary>
    /// <param name="saveData"></param>
    public static void SavePlayerData(PlayerData saveData)
    {
        string Json;

        // PlayerDataClassをJsonに変換
        Json = JsonUtility.ToJson(saveData);
        // PlayerPrefsに登録(Json変換必須)
        PlayerPrefs.SetString(KeyPlayerName, Json);

        Debug.Log("登録できてるJson?_：" + Json);

    }

    /// <summary>
    /// 保存していたプレイヤーデータの取得
    /// キーが設定されていなかったらなにもしない
    /// </summary>
    /// <returns></returns>
    public static PlayerData LoadPlayerData()
    {
        var data = null as PlayerData;

        // キーで指定した場所に情報が入っていたらTが返ってくるでっす
        if (PlayerPrefs.HasKey(KeyPlayerName))
        {
            // 保存していたプレイヤーデータをJsonに格納
            var Json = PlayerPrefs.GetString(KeyPlayerName);

            // ここでJsonからPlayerDataに変換している
            data = JsonUtility.FromJson<PlayerData>(Json);

            Debug.Log("ロードしたプレイヤー情報:" + Json);
        }
        else
        {
            Debug.Log("キーがないよ");
        }

       
        return data;
    }

    public static void DeletePlayerData()
    {
        PlayerPrefs.DeleteKey(KeyPlayerName);
    }

    /// <summary>
    /// KeyPkayerNameが格納されているか確認する
    /// 外側からみないようにしよう
    /// </summary>
    /// <returns></returns>
    public static bool HasPlayerData()
    {
        return PlayerPrefs.HasKey(KeyPlayerName);
    }

    public static void DeleteAllKey()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void StorePlayerScore(int Score)
    {
        // 一時的に上書き保存
        var data = LoadPlayerData();

        // ハイスコアが出たら
        if (!(data.HighScore > Score))
        {
            data.HighScore = Score;

            // 更新してセーブ
            SavePlayerData(data);
        }       
    }



#if UNITY_EDITOR

    [UnityEditor.MenuItem("Debug/Delete all save data")]
    static void DoDeleteAllSaveData()
    {
        DeleteAllKey();
    }

#endif

}
