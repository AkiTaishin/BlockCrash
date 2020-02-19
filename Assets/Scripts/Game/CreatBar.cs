using UnityEngine;

/// <summary>
/// バーの生成
/// </summary>
public class CreatBar : MonoBehaviour
{
    #region グローバル変数

    /// <summary>
    /// カメラ取得用
    /// </summary>
    [SerializeField] private Camera mainCamera = null;

    /// <summary>
    /// バー生成用
    /// </summary>
    [SerializeField] private GameObject Bar = null;

    /// <summary>
    /// バー削除用
    /// </summary>
    [SerializeField] private DestroyBar destroyBar = null;

    /// <summary>
    /// パワーゲージ変更に使用
    /// </summary>
    [SerializeField] private PowerGauge powerGauge = null;


    /// <summary>
    /// クリック時の座標
    /// </summary>
    private Vector3 MemoryStartPoint = Vector3.zero;

    /// <summary>
    /// クリックが離された時の座標
    /// </summary>
    private Vector3 MemoryEndPoint = Vector3.zero;

    #endregion


    /// <summary>
    /// モード共通
    /// </summary>
    private void Update()
    {
        // バー生成開始ポイント
        KeyDownMoment();

        if (SaveClass.GetIsDraw())
        {
            CreateReflectionBar();
        }
    }

    /// <summary>
    /// 反射バーの生成
    /// </summary>
    private void CreateReflectionBar()
    {
        // ノーマル時のみパワーゲージ制限がある
        if (SaveClass.GetGameMode())
        {
            // まだパワーゲージが残っていた場合
            if (powerGauge.IsRemain)
            {
                // バー生成終了ポイント
                KeyUpMoment();
                // バーがあるときの再生成時処理
                KeyPushMoment();
            }
        }
        // Infiniteはパワーゲージ制限なし
        else
        {
            // バー生成終了ポイント
            KeyUpMoment();
            // バーがあるときの再生成時処理
            KeyPushMoment();
        }               
    }

    /// <summary>
    /// クリックされた瞬間の処理
    /// モード共通
    /// </summary>
    private void KeyDownMoment()
    {
        // バー生成開始ポイント
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // カメラ座標を入れる
            MemoryStartPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // zを0にそろえる
            MemoryStartPoint.z = 0.0f;
        }
    }

    /// <summary>
    /// クリックが離された瞬間の処理
    /// Normalのときだけ最後にパワーゲージが減少する
    /// </summary>
    /// <returns>Lengs(PowerGauge処理に使用)</returns>
    public void KeyUpMoment()
    {
        // バー生成終了ポイント
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // カメラ座標を入れる
            MemoryEndPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // zを0にそろえる
            MemoryEndPoint.z = 0.0f;
            // Debug.Log("MemoryEndPoint:_" + MemoryEndPoint);

            // バーの生成
            var NewChildren = Instantiate(Bar, new Vector3((MemoryEndPoint.x + MemoryStartPoint.x) * 0.5f, (MemoryStartPoint.y + MemoryEndPoint.y) * 0.5f, 0.0f),Quaternion.identity);
            // EnptyObjectを親にする
            NewChildren.transform.parent = gameObject.transform;
            NewChildren.name = "Child";

            // ポジションの決定
            //NewChildren.transform.position = new Vector3((MemoryEndPoint.x + MemoryStartPoint.x) * 0.5f, (MemoryStartPoint.y + MemoryEndPoint.y) * 0.5f, 0.0f);

            // 角度の決定
            float AngleX = MemoryEndPoint.x - MemoryStartPoint.x;
            float AngleY = MemoryEndPoint.y - MemoryStartPoint.y;
            float Angle = Mathf.Atan2(AngleY, AngleX);
            
            NewChildren.transform.Rotate(Vector3.forward, Angle * Mathf.Rad2Deg);

            // スケールの決定
            var Lengs = Vector3.Magnitude(MemoryEndPoint - MemoryStartPoint);
            NewChildren.transform.localScale = new Vector3(Lengs, 0.3f, 1.0f);

            // ノーマルモード時のみの処理
            if (SaveClass.GetGameMode())
            {
                // パワーゲージ減少
                powerGauge.ReduceGauge(Lengs);
            }           
        }
    }

    /// <summary>
    /// スワイプされた瞬間の処理
    /// ゲームモード共通処理
    /// </summary>
    private void KeyPushMoment()
    {
        // 新たにスワイプを始めたときにすでにバーが存在していたら削除
        if (Input.GetKey(KeyCode.Mouse0))
        {
            destroyBar.DestroyOldBar();
        }
    }
}
