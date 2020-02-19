using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    #region グローバル変数

    [SerializeField] private Text top3 = null;
    [SerializeField] private Text fourAndFive = null;
    [SerializeField] private Image backGround = null;
    [SerializeField] private NetworkManager network = null;

    private bool ServerIsBesy = false;

    #endregion

    public void ActiveRanking()
    {
        var playerData = SaveClass.GetAllPlayerStatus();

        var value = playerData.Count;


        if (value > 0)
        {
            top3.text = (playerData[0].HighScore.ToString("0") + "\n\n0\n\n0");
        }
        if (value > 1)
        {
            top3.text = (playerData[0].HighScore.ToString("0") + "\n\n" + playerData[1].HighScore.ToString("0") + "\n\n");
        }
        if (value > 2)
        {
            top3.text = (playerData[0].HighScore.ToString("0") + "\n\n" + playerData[1].HighScore.ToString("0") + "\n\n" + playerData[2].HighScore.ToString("0"));
        }
        if (value > 3)
        {
            fourAndFive.text = (playerData[3].HighScore.ToString("0") + "\n\n\n0");
        }
        if (value > 4)
        {
            fourAndFive.text = (playerData[3].HighScore.ToString("0") + "\n\n\n" + playerData[4].HighScore.ToString("0"));
        }

        //top3.text = (playerData[0].HighScore.ToString("0") + "\n\n" + playerData[1].HighScore.ToString("0") + "\n\n" + playerData[2].HighScore.ToString("0"));

    }

    public void ActiveBackGround()
    {
        if (!ServerIsBesy)
        {
            StartCoroutine(WaitServerProcess());
        }
    }

    public void NonActiveBackGround()
    {
        backGround.gameObject.SetActive(false);
    }

    private IEnumerator WaitServerProcess()
    {
        ServerIsBesy = true;

        yield return network.RequestGetAllUserInfo();

        backGround.gameObject.SetActive(true);
        ActiveRanking();

        ServerIsBesy = false;
    }

}
