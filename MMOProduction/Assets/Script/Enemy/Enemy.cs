using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const float UPDATE_SPEED = 1.0f / 3.0f;

    private int maxHp;
    private int maxMp;
    private int hp = 0;
    private int mp = 0;
    private Vector3 lastPos;
    private Vector3 nextPos;
    private Quaternion lastDir = new Quaternion();
    private Quaternion nextDir = new Quaternion();


    public int HP { get { return hp; } set { hp = value; } }
    public int MP { get { return mp; } set { mp = value; } }




    private float nowFlame = 0;


    private const int MAX_FLAME = 3;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        lastDir = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // debug
        //LerpMove();

    }


    public void UpdataData(int _hp, int _mp, float _x, float _y, float _z, float _dir)
    {
        HP = _hp; MP = _mp;
        // 向きを決める
        lastDir = transform.rotation;
        nextDir = Quaternion.Euler(0, _dir, 0);

        // 位置を決める
        lastPos = transform.position;
        nextPos = new Vector3(_x, _y, _z);

        nowFlame = 0;
    }


    private void LerpMove()
    {
        nowFlame += UPDATE_SPEED;
        transform.rotation = Quaternion.Lerp(lastDir, nextDir, nowFlame);
        transform.position = Vector3.Lerp(lastPos, nextPos, nowFlame);
    }


    // アニメーション関係

    public void DeiAnimetion()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Die");
    }

    public void AttackAnimetion()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Attack");
    }


    /// <summary>
    /// アニメーションの終了を判定するコルーチン
    /// </summary>
    /// <param name="_animationName"></param>
    /// <returns></returns>
    //public IEnumerator WaitAnimationEnd(string _animationName,float _animationTime)
    //{
    //    bool fin = false;
    //    while(!fin)
    //    {
    //        AnimatorStateInfo nowState = gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
    //        if (nowState.IsName(_animationName)) { fin = true; }
    //        else { yield return new WaitForSeconds(_animationTime); }
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Attack 01" && collision.gameObject.tag == "Player")
        {

            Debug.Log("攻撃がヒットしたよ");

        }
    }

    void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
