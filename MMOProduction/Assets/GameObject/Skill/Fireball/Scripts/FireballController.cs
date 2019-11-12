//
// Fireball.cs
//
// 火球のスキルに使用
//

using UnityEngine;


public class FireballController : SkillObject
{

    private const float GRAVITY = 0.98f;


    [SerializeField]
    private FireballEffect _fireballEffect = null;  // 火球を表現するエフェクト

    [SerializeField]
    private ParticleSystem _explosionEffect = null;       // 火球の爆発に使用するエフェクト

    private Vector3 _velocity;
    private Vector3 _scale;

    private Vector3 _originPos;


    private void Start()
    {
        base.StartBase();

        _velocity = Vector3.zero;
        _scale = new Vector3(0, 0, 0);

        _originPos = transform.localPosition;

        // 関数登録
        HitAction += Explosion;
    }

    private void Update()
    {
        bool isAlive = _fireballEffect.gameObject.activeSelf;

        // アクティブ状態によってスケールを変更する
        if (!isAlive) _scale = Vector3.zero;
        else _scale = Vector3.Lerp(_scale, new Vector3(1, 1, 1), 1f * Time.deltaTime);

        // 生成後に火球を発射する
        if(isAlive)
        {
            if(_scale.x >= 0.9)
            {
                _velocity = transform.forward;
                _velocity.y = 1f;
                UpdatePlayAfter();
            }
        }
        

        // 変換したデータを反映
        this.transform.localScale = _scale;
        this.transform.position += _velocity;
    }

    /// <summary>
    /// 生成後に火球を発射する
    /// </summary>
    private void UpdatePlayAfter()
    {
        _velocity.y -= GRAVITY;
    }

    /// <summary>
    /// 火球を再生
    /// </summary>
    public override void Play()
    {
        gameObject.SetActive(true);
        _fireballEffect.Play();
    }

    /// <summary>
    /// 火球を停止
    /// </summary>
    public override void Stop()
    {
        //_explosionEffect.Stop();
        //gameObject.SetActive(false);
        _fireballEffect.Stop();
        transform.localPosition = _originPos;
        _scale = Vector3.zero;
        _velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 爆発処理
    /// </summary>
    private void Explosion(Collider other)
    {
        _explosionEffect.transform.position = gameObject.transform.position;
        _explosionEffect.Play();
        Stop();
        //Debug.Log("当たった");
    }
}
