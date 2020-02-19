using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerGauge : MonoBehaviour
{
    #region グローバル変数

    /// <summary>
    /// パワーゲージ減少のために使用
    /// </summary>
    [SerializeField] private Image powerGauge = null;

    private readonly float MaxValue = 60.0f;
    private float NowGaugeValue = 0.0f;
    public bool IsRemain = true;

    #endregion

    /// <summary>
    /// パワーゲージ減少処理
    /// ノーマルモード時にしか使用しない
    /// 呼び出し先でノーマル時のみ処理が通過するようにしている
    /// </summary>
    /// <param name="Lengs"></param>
    public void ReduceGauge(float Lengs)
    {
        NowGaugeValue += Lengs;

        // まだパワーがあるとき
        // これいる？
        if(powerGauge.fillAmount > 0)
        {
            var work = 1 - (NowGaugeValue / MaxValue);
            // 0下回り処理
            if (work <= 0)
            {
                powerGauge.fillAmount = 0;
                IsRemain = false;
            }
            // workが0よりも大きかった場合
            else
            {
                powerGauge.fillAmount = work;
            }
        }
    }
}
