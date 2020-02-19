using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlock : MonoBehaviour
{
    #region グローバル変数

    /// <summary>
    /// 生成される新しいブロック
    /// </summary>
    [SerializeField] private GameObject NewBlock = null;

    /// <summary>
    /// 生成したブロックの親Obj
    /// 実体はない
    /// </summary>
    [SerializeField] private GameObject BlockParent = null;

    /// <summary>
    /// ゲームスタート判定に使用
    /// </summary>
    [SerializeField] private ReadyCheck CountdownText = null;


    /// <summary>
    /// 何秒で次のBlocksが生成されるか
    /// 経過時間とともに少しずつ短くなっていく
    /// </summary>
    private readonly float CreateTime = 0.3f;

    /// <summary>
    /// ブロックを新たに生成できるかフラグ
    /// </summary>
    private bool CanBlockCreate = false;

    /// <summary>
    /// 生成カウント
    /// </summary>
    private float CreateBlockCount = 0.0f;
    
    /// <summary>
    /// ブロック生成基準時間
    /// </summary>
    private float CreateBlockTime = 8.0f;

    /// <summary>
    /// 最大生成速度
    /// </summary>
    private readonly int MaxAdminCreateBlockTime = 7;

    /// <summary>
    /// ブロック生成基準時間を短くするための変数
    /// 3回生成されたら1加算される
    /// </summary>
    private int AdminCreateBlockTime = 0;

    /// <summary>
    /// AdminCreateBlockTimeを加算するための変数
    /// </summary>
    private int AdminWork = 0;

    /// <summary>
    /// コルーチン管理変数
    /// </summary>
    public Coroutine StartCreate = null;

    #endregion

    /// <summary>
    /// コルーチン開始可能検査
    /// </summary>
    private void Start()
    {
        // 先にコルーチンが走っていた場合はストップ
        if(StartCreate != null)
        {
            StopCoroutine(StartCreate);
        }
    }

   

    /// <summary>
    /// ブロック生成が可能なら生成する
    /// </summary>
    private void Update()
    {
        // ゲーム開始かつ
        if (CountdownText.IsGameStart())
        {
            // ゲームオーバーになっていないとき
            if (!SaveClass.GetGameOver())
            {
                CreateBlockCount += Time.deltaTime;

                // ブロック生成基準時間を生成カウントが越したとき
                if (CreateBlockCount > (CreateBlockTime - AdminCreateBlockTime))
                {
                    Debug.Log("\nCreateBlockCount_:" + CreateBlockCount + "\nCreateBlockTime_:" + CreateBlockTime + "\nAdminCreateBlockTime_:" + AdminCreateBlockTime + "\nCreateBlockTime - AdminCreateBlockTime_:" + (CreateBlockTime - AdminCreateBlockTime));
                    
                    // 移動先の決定
                    var fab = DecideMovePoint(out Vector3 ToPosition);
                   
                    // 生成移動コルーチンスタート
                    StartCreate = StartCoroutine(MoveBlocks(ToPosition, fab));

                    // 生成カウントを減少させるか否か
                    AdminCreateBlockTime = ShortenCreateBlockTime();

                    // 生成カウントリセット
                    CreateBlockCount = 0.0f;
                }
            }
        }
    }

    /// <summary>
    /// 移動先Positionの決定
    /// </summary>
    /// <param name="ToPosition">コルーチンにおける移動先ポイント</param>
    private PrefabStatus DecideMovePoint(out Vector3 ToPosition)
    {     
        // 生成可能フラグON
        CanBlockCreate = true;
        PrefabStatus fab = null;

        // PrefabのBlocksを一番上の座標に生成する
        var GetPrefab = Instantiate(NewBlock, BlockParent.transform, true);
        fab = GetPrefab.GetComponent<PrefabStatus>();//.Initialized(this);

        // 生成したブロックの移動先決定
        ToPosition = BlockParent.transform.position;
        ToPosition.y -= 0.45f;

        return fab;
    }

    /// <summary>
    /// ブロック生成コルーチン
    /// ブロック生成基準時間を生成カウントが越したときに呼び出される
    /// </summary>
    /// <param name="ToPosition">ブロック群の移動先Position</param>
    /// <returns>コルーチン</returns>
    private IEnumerator MoveBlocks(Vector3 ToPosition, PrefabStatus fab)
    {
        float IncrementTime = 0.0f; 
        float Progress = 0.0f;

        // ブロック生成可能フラグがONだったら
        // ブロック生成基準時間を生成カウントが越したときにONになる
        while (CanBlockCreate)
        {
            // 時間になったらいまある"Block"を下に移動させて
            while (Progress < 1.0f)
            {
                // ほかのブロックを下に移動させる関数               
                Vector3 NewPos = Vector3.Lerp(BlockParent.transform.position, ToPosition, Progress);
               
                // Progressの変動値代入
                // 確定した移動ポイントへ移動
                Progress = MoveToDecidedPoint(IncrementTime, NewPos, out float IncrementTimeWork);
                IncrementTime = IncrementTimeWork;

                yield return null;
            }

            // Progressのリセット
            Progress = 0.0f;
            IncrementTime = 0.0f;
            fab.SetIsActive(true);

            yield return null;
        }
        yield return null;        
    }

    /// <summary>
    /// 確定した移動先座標まで移動させる
    /// </summary>
    /// <param name="IncrementTime">Progressの基準</param>
    /// <param name="NewPos">移動先座標</param>
    /// <param name="IncrementTimeWork">インクリメントしたものをoutする</param>
    /// <returns>Progress</returns>
    private float MoveToDecidedPoint(float IncrementTime, Vector3 NewPos, out float IncrementTimeWork)
    {
        // ブロック全体を下に移動させる
        BlockParent.transform.position = NewPos;
        
        // Prgressの更新
        IncrementTime += Time.deltaTime;
        float Progress = IncrementTime / CreateTime;
        // 新しいIncrementTimeを返すためにWorkに格納する
        IncrementTimeWork = IncrementTime;

        // ブロック生成可能フラグをOFFに
        CanBlockCreate = false;

        return Progress;
    }

    /// <summary>
    /// ブロック生成カウントを管理する変数AdminCreateBlockTimeを加算させる関数
    /// 3回ブロックが生成されたらAdminCreateBlockTimeを1加算させる
    /// 最大でも7までにする
    /// </summary>
    /// <returns>AdminCreateBlockTime</returns>
    private int ShortenCreateBlockTime()
    {
        // 最大でもAdminCreateBlockTimeは7に固定
        if(AdminCreateBlockTime < MaxAdminCreateBlockTime)
        {
            // AdminWorkが3になったら
            AdminWork += 1;
            if (AdminWork > 2)
            {
                // AdminCreateBlockTimeを1加算
                AdminCreateBlockTime += 1;
                // してAdminWorkをリセット
                AdminWork = 0;
            }
        }   

        return AdminCreateBlockTime;
    }
}
