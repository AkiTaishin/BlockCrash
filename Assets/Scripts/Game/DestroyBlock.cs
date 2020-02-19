using UnityEngine;

/// <summary>
/// ブロックの削除
/// モード共通
/// </summary>
public class DestroyBlock : MonoBehaviour
{
    public void DestroyBlocks(RaycastHit hit)
    {     
        Destroy(hit.transform.gameObject);
    }
}


