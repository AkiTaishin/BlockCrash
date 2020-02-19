using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInputField : MonoBehaviour
{
    #region グローバル変数

    
    #endregion


    public void EnableInputField()
    {
        if (SaveClass.GetGameOver())
        {
            gameObject.SetActive(true);
        }
    }
}
