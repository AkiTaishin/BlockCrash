using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    #region グローバル変数


    #endregion


    public void LoadtoTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void LoadtoMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadtoNormalGame()
    {
        // ゲームモードをノーマルモードに
        SaveClass.SetIsClear(false);
        SaveClass.SetGameMode(true);
        SaveClass.SetGameOver(false);
       // Debug.Log("ノーマル");
        SceneManager.LoadScene("NormalGame");
    }

    public void LoadtoInfiniteGame()
    {
        // ゲームモードをInfiniteモードに
        SaveClass.SetIsClear(false);
        SaveClass.SetGameMode(false);
        SaveClass.SetGameOver(false);
        //Debug.Log("無限をなめるな");
        SceneManager.LoadScene("InfiniteGame");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_ANDROID

       UnityEngine.Application.Quit();

#endif
    }

}
