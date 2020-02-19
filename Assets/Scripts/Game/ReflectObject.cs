using UnityEngine;

/// <summary>
/// 物体の反射処理云々
/// モード共通
/// </summary>
public class ReflectObject : MonoBehaviour
{
    #region グローバル変数

    /// <summary>
    /// 反射ベクトルの計算に使用
    /// </summary>
    [SerializeField] private Ball ball = null;

    /// <summary>
    /// 削除に使用
    /// </summary>
    [SerializeField] private DestroyBar destroyBar = null;
    [SerializeField] private DestroyBlock destroyBlock = null;

    #endregion

    /// <summary>
    /// 毎フレボールが物体に衝突したか求める
    /// </summary>
    /// <param name="ballTrans">BallTrans</param>
    /// <param name="ballVector">BallVector</param>
    /// <returns>反射後のBallVector</returns>
    public Vector3 Reflection(Transform ballTrans, Vector3 ballVector, out float Check, out RaycastHit hit)
    {
        Check = 0.0f;
        //RaycastHit hit;
        // ボールの位置から向いている方向にレイを飛ばす
        Ray ray = new Ray(ballTrans.position, ballVector);

        //// 当たってるか計算   
        if (Physics.SphereCast(ray, 0.25f, out hit, Vector3.Magnitude(ballVector)))
        {
           // Debug.Log("ballVector.magnitude_" + ballVector.magnitude + "\nVector3.Magnitude(hit.point - ballTrans.position)" + (Vector3.Magnitude(hit.point - ballTrans.position) - 0.25f));
            //埋まったレイの長さを計算
            Check = ballVector.magnitude - (Vector3.Distance(hit.point , ballTrans.position)-0.25f);
            // ぶつかった物体に応じて処理変更
            ballVector = HitCase(hit, ballVector);

            //Debug.Log("hitしたもの_" + hit.transform.name + "\nhitした座標_" + hit.point);
        }

        //if(Physics.Raycast(ray,  out hit, Vector3.Magnitude(ballVector)))
        //{
        //    Debug.Log("ballVector.magnitude_" + ballVector.magnitude + "\nVector3.Magnitude(hit.point - ballTrans.position)" + Vector3.Magnitude(hit.point - ballTrans.position) + "_" + Vector3.Distance(hit.point, ballTrans.position));
        //    //埋まったレイの長さを計算
        //    Check = ballVector.magnitude - Vector3.Distance(hit.point, ballTrans.position);
        //    // ぶつかった物体に応じて処理変更
        //    ballVector = HitCase(hit, ballVector);
        //
        //    Debug.Log("hitしたもの_" + hit.transform.name + "\nhitした座標_" + hit.point);
        //}
        Debug.DrawRay(ballTrans.position, ballVector * 10);

        return ballVector;
    }

    /// <summary>
    /// 反射の処理はここ
    /// </summary>
    /// <param name="hit">ぶつかった物体</param>
    /// <param name="ballVector">今の向き</param>
    /// <returns>新しい向き</returns>
    private Vector3 ReflectionProcess(RaycastHit hit, Vector3 ballVector)
    {
        // ぶつかった物体の法線取得
        Vector3 Normal = hit.normal;
        // 反射Vec計算
        ballVector = ball.BallVector - (2 * Vector3.Dot(ball.BallVector, Normal) * Normal);

        return ballVector;
    }

    /// <summary>
    /// それぞれの物体に衝突したときの処理
    /// </summary>
    /// <param name="hit">ぶつかった物体</param>
    /// <param name="ballVector">今の向き</param>
    /// <returns>向き(ReflectionProcess)</returns>
    private Vector3 HitCase(RaycastHit hit, Vector3 ballVector)
    {
        // 衝突した物体の特定 
        switch (hit.transform.tag)
        {
            case "Wall":
                {
                    // Ballのフラグ、色をすべて元に戻す
                    ball.SetIsBound();
                    ball.SetIsThrough();
                    ball.SetMaterialColor();

                    // 向き変更
                    ballVector = ReflectionProcess(hit, ballVector);
                    break;
                }

            case "Bar":
                {
                    // 貫通玉になるかどうか
                    ball.BarHitProcess();
                    // 向き変更
                    ballVector = ReflectionProcess(hit, ballVector);
                    // ヒットしたバーだけ削除
                    destroyBar.DestoryMyself(hit);

                    break;
                }

            case "Block":
                {
                    // 貫通がOFFの時
                    if (!ball.GetIsThrough())
                    {
                        // Ballのフラグをすべて元に戻す
                        ball.SetIsBound();
                        ball.SetIsThrough();

                        // Blockを削除して向き変更
                        destroyBlock.DestroyBlocks(hit);
                        hit.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                        ballVector = ReflectionProcess(hit, ballVector);

                        // DeleteCountを増やす
                        ball.DeleteCount += 1;
                    }
                    else
                    // 貫通がONになっていたらブロックには跳ね返らない
                    {
                        // 向きの変更はなし
                        destroyBlock.DestroyBlocks(hit);

                        var collider = destroyBlock.GetComponent<BoxCollider>();
                        if(collider != null)
                        {
                            collider.enabled = false;
                        }

                        // DeleteCountを増やす
                        ball.DeleteCount += 1;
                    }
                    break;
                }

        }

        return ballVector;
    }
}
#region Ray復習
//Ray(①Vector3 origin, ②Vector3 direction)

//①・・・Rayの発生地点

//②・・・Rayの進む方向


//Debug.DrawRay(①Vector3 start, ②Vector3 dir, ③Color color, ④float duration, ⑤bool depthTest);

//①・・・開始地点。Ray.originでRayの開始地点を得られる

//②・・・方向と長さ。Ray.directionで得られる

//③・・・可視化したときの色。(何も指定しなければ、Color.white)

//④・・・表示時間(何も指定しなければ、0.0fになる(ワンフレームだけ描写))

//⑤・・・ラインがオブジェクトで隠れた場合、ラインを隠すかどうか(何も指定しなければ、true)
#endregion