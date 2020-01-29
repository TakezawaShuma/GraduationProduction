//////////////////////////////////
// 自分以外のプレイヤーのクラス //
//////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
MAXHP　　　　　　変
   MP　　　　　　変
nowHP　常(変)
   MP　常(変)
Sutas　常(変)
ロール(ジョブ)　変
パーティ番号　　変
レベル　　　　　変
名前　　　　　　変
ヘイト順　常(変)

 */

public class OtherPlayers: NonPlayer
{
    private AnimatorManager am;

    private PlayerAnim.PARAMETER_ID lastAnimation;

    public int HP { get { return hp; } set { hp = value; } }
    public int MP { get { return mp; } set { mp = value; } }

    //public int id = 0;
    //public void Init(int _i) { id = _i; }

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        lastDir = nextDir = transform.rotation;
        am = new AnimatorManager();
        am.ANIMATOR = animator_;
        lastAnimation = animationType;
    }


    // Update is called once per frame
    void Update()
    {
        LerpMove();
        Animation();
    }

    /// <summary>
    /// アニメーション
    /// </summary>
    void Animation()
    {
        if (lastAnimation != animationType) {
            am.AnimChange((int)animationType);
            lastAnimation = animationType;
        }
    }
}
