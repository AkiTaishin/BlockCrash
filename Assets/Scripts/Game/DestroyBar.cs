using UnityEngine;

/// <summary>
/// バーの削除
/// モード共通
/// </summary>
public class DestroyBar : MonoBehaviour
{
    /// <summary>
    /// ヒットしたときに削除
    /// </summary>
    /// <param name="hit">当たってるバー</param>
    public void DestoryMyself(RaycastHit hit)
    {
        Destroy(hit.transform.gameObject);
    }

    /// <summary>
    /// すでにバーがあるときに古いバーを削除
    /// </summary>
    public void DestroyOldBar()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
