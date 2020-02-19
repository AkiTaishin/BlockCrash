using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabStatus : MonoBehaviour
{
    #region グローバル変数

    private bool IsActive = false;

    #endregion

    private void Update()
    {
        DestroyParents();
    }

    /// <summary>
    /// 子供が０になったら親を削除
    /// </summary>
    public void DestroyParents()
    {
        int ObjCount = transform.childCount;

        if (ObjCount == 0)
        {
            SetIsActive(false);
            Destroy(gameObject);
        }
    }

    public bool GetIsActive()
    {
        return IsActive;
    }

    public void SetIsActive(bool Pram)
    {
        IsActive = Pram;
    }

}
