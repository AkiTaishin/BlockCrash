using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー情報管理クラス
/// System.SerializableはJsonUtilityを使用して変換するうえで必須になる
/// </summary>
[System.Serializable]
public class PlayerData
{
    public string ID = "";
    public string Name = "";
    public int HighScore = 0;
    public PlayerData(string name)
    {
        Name = name;
    }
}

/// <summary>
/// 全プレイヤー情報管理クラス
/// System.SerializableはJsonUtilityを使用して変換するうえで必須になる
/// </summary>
[System.Serializable]
public class PlayerDataList
{
    public  PlayerData[] list = null;

    public  List<PlayerData> sort()
    {
        var data = new List<PlayerData>();

        data.AddRange(list);
        data.Sort((a, b) => b.HighScore - a.HighScore);

        Debug.Log(data);

        return data;
    }

}
