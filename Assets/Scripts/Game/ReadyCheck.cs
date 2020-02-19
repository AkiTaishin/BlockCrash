using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム開始前の待機処理
/// モード共通
/// </summary>
public class ReadyCheck : MonoBehaviour
{
    #region グローバル変数

    /// <summary>
    /// テキスト用
    /// </summary>
    [SerializeField] private Text CountdownText = null;

    /// <summary>
    /// カウントダウンコルーチンを走らせる
    /// </summary>
    private Coroutine Count = null;

    /// <summary>
    /// ボール移動開始フラグ
    /// </summary>
    private bool GameStart = false;

    #endregion


    private void Start()
    {
        // すでにコルーチンが入っていたらとめる
        if(Count != null)
        {
            StopCoroutine(Count);
        }

        // コルーチンスタート
        Count = StartCoroutine(Countdown());
    }

    /// <summary>
    /// カウントダウンを管理するコルーチン
    /// </summary>
    /// <returns>カウント</returns>
    private IEnumerator Countdown()
    {
        CountdownText.text = ("GetReady?");
        yield return new WaitForSeconds(1.0f);

        CountdownText.text = ("3");
        yield return new WaitForSeconds(1.0f);

        CountdownText.text = ("2");
        yield return new WaitForSeconds(1.0f);

        CountdownText.text = ("1");
        yield return new WaitForSeconds(1.0f);

        CountdownText.text = ("GO!!");
        yield return new WaitForSeconds(1.0f);

        // ゲーム開始
        GameStart = true;

        // 線を引けるようにする
        SaveClass.SetIsDraw(true);

        // 時間経過でテキストを消す
        CountdownText.GetComponent<Text>().enabled = false;
    }

    public bool IsGameStart()
    {
        return GameStart;
    }
}
