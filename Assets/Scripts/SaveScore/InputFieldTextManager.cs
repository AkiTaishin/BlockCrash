using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldTextManager : MonoBehaviour
{
    #region グローバル変数

    private InputField inputField = null;
    public Text alertText = null;
    private string inputValue;

    #endregion


    /// <summary>
    /// InputFieldコンポーネントの取得および初期化メソッドの実行
    /// </summary>
    void Start()
    {
        inputField = GetComponent<InputField>();

        InitInputField();
    }


    /// <summary>
    /// Log出力用メソッド
    /// 入力値を取得してLogに出力し初期化
    /// </summary>
    public void InputName()
    {
        inputValue = inputField.text;

        if (string.IsNullOrEmpty(inputValue))
        {
            // 警告を出して再度入力させる(保存しない)
            alertText.text = ("※１文字以上入力してください");
            alertText.GetComponent<Text>().enabled = true;
        }
        else
        {
            //alertText.text = ("Thankyou for playing " + inputValue + " !!");
            //alertText.GetComponent<Text>().enabled = true;

            // ここに保存処理
            SaveClass.SaveUserName(inputValue);
        }

        // Debug.Log(inputValue);
    }

    /// <summary>
    /// 名前確定前の入力中処理
    /// 空欄になりそうになったら警告を出す
    /// </summary>
    public void ChangeName()
    {
        inputValue = inputField.text;

        if (!string.IsNullOrEmpty(inputValue))
        {
            // 警告を消す
            alertText.GetComponent<Text>().enabled = false;
        }
        else
        {
            // また空白に戻った時は警告を出す
            alertText.text = ("※１文字以上入力してください");
            alertText.GetComponent<Text>().enabled = true;
        }
    }



    /// <summary>
    /// InputFieldの初期化
    /// 入力値をリセットする
    /// </summary>
    void InitInputField()
    {
        // リセット
        inputField.text = "";

        // アクティブに設定する
        inputField.ActivateInputField();
    }
}
