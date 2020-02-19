using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ボール
/// </summary>
public class Ball : MonoBehaviour
{
    #region グローバル変数

    /// <summary>
    /// 開始フラグ受け取り用
    /// </summary>
    [SerializeField] private ReadyCheck CountdownText = null;

    /// <summary>
    /// Ballが物体に当たった時の処理に使用
    /// </summary>
    [SerializeField] private ReflectObject Obj = null;

    /// <summary>
    /// Resultへの遷移用
    /// </summary>
    [SerializeField] private GameToResult ToResult = null;

    /// <summary>
    /// 貫通玉になるかに使用
    /// 壁やブロックに当たる前にバーに一度でも触れているか
    /// </summary>
    private bool IsBound = false;

    /// <summary>
    /// 貫通玉になる
    /// </summary>
    private bool IsThrough = false;

    /// <summary>
    /// ボールの動くスピード
    /// </summary>
    private readonly float BallSpeed = 0.1f;

    /// <summary>
    /// ブロックの総数
    /// </summary>
    private readonly int BlockCount = 15;

    /// <summary>
    /// ブロックが消えた数
    /// Normalモードではクリア条件に使用
    /// Infiniteモードではスコアの計算に使用
    /// </summary>
    public int DeleteCount = 0;

    /// <summary>
    /// ボールの動く向き
    /// </summary>
    public Vector3 BallVector = Vector3.zero;

    /// <summary>
    /// ボールの実際の位置
    /// </summary>
    public Transform BallTrans = null;

    /// <summary>
    /// マテリアルの色の一時保管に使用
    /// </summary>
    private Color32 MaterialColor = new Color32(0, 0, 0, 0);

    #endregion


    /// <summary>
    /// ゲーム開始時のボールの動き
    /// </summary>
    private void Start()
    {
        // 開始時にランダムに下方向に降ってくるようにする
        BallTrans = gameObject.transform;
        BallVector = new Vector3(/*Random.Range(-1.0f, 1.0f)*/0.0f, -1.0f, 0.0f).normalized * BallSpeed;        
        
        // マテリアルカラーの保管
        MaterialColor = transform.GetComponent<Renderer>().material.color;
    }

    private void LateUpdate()
    {
        // カウントダウンが終わったら
        if (CountdownText.IsGameStart())
        {

            // モード共通失敗処理
            ToResult.GotoResultScene();

            // 条件を満たしたらResultへ遷移
            // ノーマルモードの場合
            if (SaveClass.GetGameMode())
            {
                if (BlockCount != DeleteCount)
                {
                    //埋まったレイの長さ
                    float Check;
                    RaycastHit hit;

                    var BallVectorWork = BallVector;

                    // 物体に衝突したかどうか
                    // 衝突していた場合新しいBallVectorを取得
                    BallVector = Obj.Reflection(BallTrans, BallVector, out Check, out hit);

                    if (BallVector != BallVectorWork)
                    {
                        while (true)
                        {
                            //BallTrans.position = hit.point- BallVector;

                            var work = BallVector.normalized;
                            BallVectorWork = BallVector;
                            work *= Check;
                            //Debug.Log("Check_:" + Check);

                            BallVector = Obj.Reflection(BallTrans, BallVector, out Check, out hit);

                            if (BallVector == BallVectorWork)
                            {
                                BallTrans.Translate(work);
                                break;
                            }

                        }
                    }
                    else
                    {
                        BallTrans.Translate(BallVector);
                    }            
                }
                else
                {
                    // クリアフラグを立てて遷移
                    SaveClass.SetIsClear(true);
                    DeleteCount = 0;
                    ToResult.GotoResultScene();
                }
            }
            // Infiniteモードの場合
            // クリアはないのでひたすらに壁打ち
            // @todo 関数にできそう
            else
            {
                //埋まったレイの長さ
                float Check;
                RaycastHit hit;

                var BallVectorWork = BallVector;

                // 物体に衝突したかどうか
                // 衝突していた場合新しいBallVectorを取得
                BallVector = Obj.Reflection(BallTrans, BallVector, out Check, out hit);

                if (BallVector != BallVectorWork) 
                {
                    while(true)
                    {
                        //BallTrans.position = hit.point- BallVector;

                        var work = BallVector.normalized;
                        BallVectorWork = BallVector;
                        work *= Check;
                       // Debug.Log("Check_:" + Check);
                
                        BallVector = Obj.Reflection(BallTrans, BallVector, out Check, out hit);
                
                        if (BallVector == BallVectorWork)
                        {
                            BallTrans.Translate(work);
                            break;
                        }
                
                    }
                }
                else
                {
                    BallTrans.Translate(BallVector);
                }



                //while (hit.point > BallVector.magnitude)
                //{
                //    var work = BallVector.normalized;
                //    work * Check
                //    BallVector = Obj.Reflection(BallTrans, BallVector, out Check);
                //}

                //// BallVectorの方向へ移動
                //BallTrans.Translate(BallVector);
            }         
        }        
    }

    /// <summary>
    /// バーに当たった時のフラグ管理処理
    /// モード共通
    /// </summary>
    public void BarHitProcess()
    {
        // 貫通玉ではないとき
        if (!IsThrough)
        {
            // バーにすでに当たっていた時の処理
            // 貫通玉にする
            if (IsBound)
            {
                // 貫通フラグON
                IsThrough = true;
                // バーとの衝突済みフラグを戻す
                IsBound = false;
                // マテリアルカラーを赤に
                transform.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
            }
            // バーにまだ衝突していなかった場合の処理
            else
            {
                // 衝突フラグON
                IsBound = true;
                SetMaterialColor();
            }
        }
        // 貫通玉だったのにさらにバーに衝突したときの処理
        else
        {
            // 貫通フラグを戻す
            IsThrough = false;

            // バウンドはした
            IsBound = true;

            // 色も戻そうね
            SetMaterialColor();
        }
    }

    /// <summary>
    /// 衝突済みフラグの獲得
    /// ReflectObject.csにて使用
    /// </summary>
    /// <returns>IsBound</returns>
    public bool GetIsBound()
    {
        return IsBound;
    }

    /// <summary>
    /// 貫通玉フラグの獲得
    /// ReflectObject.csにて使用
    /// </summary>
    /// <returns>IsThrough</returns>
    public bool GetIsThrough()
    {
        return IsThrough;
    }

    /// <summary>
    /// IsBoundのフラグ初期化
    /// ReflectObject.csにて使用
    /// </summary>
    public void SetIsBound()
    {
        IsBound = false;
    }

    /// <summary>
    /// IsThroughフラグの初期化
    /// ReflectObject.csにて使用
    /// </summary>
    public void SetIsThrough()
    {
        IsThrough = false;
    }

    /// <summary>
    /// マテリルカラーの初期化
    /// </summary>
    public void SetMaterialColor()
    {
        // Start関数で保管していた変数の使用
        transform.GetComponent<Renderer>().material.color = MaterialColor;
    }
}
