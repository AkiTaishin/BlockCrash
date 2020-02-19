using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameToResult : MonoBehaviour
{
    #region グローバル変数

    [SerializeField] private Ball ball = null;
    [SerializeField] private Text gameOverText = null;
    [SerializeField] private Text gameOverTextInfinite = null;
    [SerializeField] private Text gameClearText = null;
    [SerializeField] private GameObject loadToNormalGame = null;
    [SerializeField] private GameObject loadToInfiniteGame = null;
    [SerializeField] private GameObject loadToMenu = null;
    [SerializeField] private GameObject blocks = null;
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private NetworkManager network = null;
    [SerializeField] private ConvertFromJson convert = null;

    private bool ServerIsBusy = false;

    #endregion



    /// <summary>
    /// なんちゃってリザルト画面
    /// </summary>
    public void GotoResultScene()
    {
        // ゲームモードがNormalの場合
        if (SaveClass.GetGameMode())
        {
            // ブロックをすべて消したら
            if (SaveClass.GetIsClear())
            {
                Destroy(ball.gameObject);

                // ゲームクリアテキストの表示
                gameClearText.GetComponent<Text>().enabled = true;

                // ボタンを出す
                CreateButton();
            }

            // 玉が画面外に出たら
            if (!ball.GetComponent<Renderer>().isVisible)
            {
                // 玉を削除
                Destroy(ball.gameObject);
                // ゲームオーバーテキストの表示
                gameOverText.GetComponent<Text>().enabled = true;

                // ボタンを出す
                CreateButton();
            }
        }
        // ゲームモードがInfiniteの場合
        else
        {
            var ResultScore = ball.DeleteCount * 100;
            bool CanOnlyOnce = false;

            if (!SaveClass.GetGameOver())
            {
                // 玉が画面外に出たら
                if (!ball.GetComponent<Renderer>().isVisible && !CanOnlyOnce)
                {
                    // 一度きりフラグを折る
                    CanOnlyOnce = true;

                    // ゲームオーバーテキストの表示
                    gameOverTextInfinite.text = ("Game Over\n\nYour Score:" + ResultScore + "\n\nTap to Retry!!");
                    gameOverTextInfinite.GetComponent<Text>().enabled = true;

                }

                foreach (Transform childTransform in blocks.transform)
                {
                    // アクティブなブロックが外に出たら
                    if (!childTransform.GetComponent<Renderer>().isVisible && childTransform.GetComponent<PrefabStatus>().GetIsActive() && !CanOnlyOnce)
                    {
                        // 一度きりフラグを折る
                        CanOnlyOnce = true;

                        // ゲームオーバーテキストの表示
                        gameOverTextInfinite.text = ("Game Over\n\nYour Score:" + ResultScore + "\n\nTap to Retry!!");
                        gameOverTextInfinite.GetComponent<Text>().enabled = true;
                    }
                }
            }

            // 一度だけ通ってほしいゲーム終了処理
            if (CanOnlyOnce)
            {
                SaveClass.SetGameOver(true);

                // 玉を削除
                Destroy(ball.gameObject);

                // save
                SaveClass.StorePlayerScore(ResultScore);

                // 通信中は何も受け付けないように
                if (!ServerIsBusy)
                    StartCoroutine(WaitServerProcess());

            }
        }
    }

    private IEnumerator WaitServerProcess()
    {
        // 通信中は何も受け付けないように
        ServerIsBusy = true;

        // 通信処理を待つ
        yield return network.RequestSetUserInfo(convert.ConvertPlayerData());

        CreateButton();

        ServerIsBusy = false;
    }


    // バー生成NGとボタン表示関数を作る
    private void CreateButton()
    {
        // ノーマルモード
        if (SaveClass.GetGameMode())
        {
            // バーを生成できなくする
            SaveClass.SetIsDraw(false);

            // ボタンを出す
            Instantiate(loadToNormalGame, canvas.transform);
            Instantiate(loadToMenu, canvas.transform);

        }
        // Infiniteモード
        else
        {
            // バーを生成できなくする
            SaveClass.SetIsDraw(false);

            // ボタンを出す
            Instantiate(loadToInfiniteGame, canvas.transform);
            Instantiate(loadToMenu, canvas.transform);

           // CreateInputField();
        }
    }
}
