using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    #region グローバル変数

    [SerializeField] private Text score = null;
    [SerializeField] private Ball ball = null;

    #endregion

    // Update is called once per frame
    private void Update()
    {
        score.text = ("Score: " + ball.DeleteCount * 100);
    }
}
