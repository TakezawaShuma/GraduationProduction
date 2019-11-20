//////////////////////////////////
// プレイヤーコントロールクラス //
//////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("プレイヤー")]
    private Player PlayerData = null;

    [SerializeField, Header("カメラ")]
    private FollowingCamera FollowingCamera = null;

    [SerializeField, Header("プレイヤーの設定ファイル")]
    private PlayerSetting playerSetting = null;

    [SerializeField, Header("アニメーターコントローラー")]
    private Animator animator = null;

    [SerializeField, Header("チャットコントローラー")]
    private ChatController chatController = null;


    //[SerializeField, Header("攻撃判定用当たり判定")]
    private GameObject weapon = null;

    public GameObject AttackCollider
    {
        get { return weapon; }
        set { weapon = value; }
    }

    // 現在のステート
    private BaseState currentState;

    private AnimatorManager animatorManager;

    private bool lockState = false;
    public bool Lock { set { lockState = value; } }

    private GameObject target;
    public GameObject Target { get { return target; } }

    private Rigidbody rigidbody1;

    private PlayerSound sound_ = null;

    public enum Mode
    {
        Normal,
        Battle,
    }

    private Mode mode = Mode.Normal;

    public Mode MODE
    {
        get { return mode; }
    }

    private int skilId = 0;

    public int SKIL
    {
        set { skilId = value; }
    }

    // スキル再生（使用？）クラス
    [SerializeField]
    private SkillPlayer _skillPlayer = null;

    public void Init(Player _playerData, FollowingCamera _camera, PlayerSetting _setting, ChatController _chat, Animator _animator)
    {
        PlayerData = _playerData;
        FollowingCamera = _camera;
        playerSetting = _setting;
        chatController = _chat;
        animator = _animator;
    }

    // 位置
    Vector3 pos;
    public Vector3 Position
    {
        get { return pos; }
    }

    // 向き
    float dir = 0;
    public float Direction
    {
        get { return dir; }
    }


    // Use this for initialization
    void Start()
    {
        pos = Vector3.zero;
        animatorManager = new AnimatorManager(animator);
        IdleState.Instance.Initialized(this, playerSetting, animatorManager);
        KeyMoveState.Instance.Initialized(this, playerSetting, animatorManager);
        AutoRunState.Instance.Initialized(this, playerSetting, animatorManager);
        NormalAttackState.Instance.Initialized(this, playerSetting, animatorManager);

        AttackCollider = GetComponent<WeaponList>().GetWeapons(0);

        currentState = IdleState.Instance;
        currentState.Start();

        rigidbody1 = GetComponent<Rigidbody>();

        _skillPlayer = GetComponent<SkillPlayer>();

        sound_ = GetComponent<PlayerSound>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!chatController.GetChatActiveFlag())
        {
            if (target != null)
            {
                Vector3 v = target.transform.position - transform.position;

                if (v.magnitude > playerSetting.LOD)
                {
                    target.GetComponent<Marker>().STATE = Marker.State.None;
                    target = null;
                    lockState = false;
                    FollowingCamera.LOCK = null;
                }
            }

            if (Input.GetMouseButtonDown(1) && target != null)
            {
                mode = Mode.Normal;
                target.GetComponent<Marker>().STATE = Marker.State.None;
                target = null;
                lockState = false;
                weapon.SetActive(false);
                FollowingCamera.LOCK = null;
            }

            currentState.Execute();
        }

        // デバッグ スキル使用
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _skillPlayer.GetSkill(0).Play();
        }
    }

    public void NoMove()
    {
        rigidbody1.velocity = new Vector3(0, rigidbody1.velocity.y, 0);
        Quaternion rot = transform.rotation;

        if (lockState)
        {
            Vector3 dir = target.transform.position - transform.position;
            rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), playerSetting.TS);
        }


        pos = rigidbody1.position;

        transform.rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);

        SendMoveDeta(pos, rot.eulerAngles.y);
    }

    public void Move(Vector3 velocity)
    {
        Quaternion rot = new Quaternion();

        if (lockState)
        {
            Vector3 dir = target.transform.position - transform.position;
            rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), playerSetting.TS);
        }
        else
        {
            // 移動方向に回転
            rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(FollowingCamera.Angle * velocity), playerSetting.TS);
        }

        Vector3 v = FollowingCamera.Angle * velocity;

        // 移動
        rigidbody1.velocity = new Vector3(v.x, rigidbody1.velocity.y, v.z);

        pos = rigidbody1.position;

        transform.rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);

        SendMoveDeta(pos, rot.eulerAngles.y);
    }

    public void SendMoveDeta(Vector3 position, float direction)
    {
        float x, y, z, dir;

        x = position.x;
        y = position.y;
        z = position.z;
        dir = direction;

        PlayerData.Position = new Vector4(x, y, z, dir);

        //PlayerData.UpdatePosition(x, y, z);
        //PlayerData.UpdateDirection(dir);
    }

    public void ChangeState(BaseState state)
    {
        currentState.End();
        currentState = state;
        currentState.Start();
    }

    public void LockOn()
    {
        bool noLock = false;

        Ray ray = FollowingCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        int layerNo = LayerMask.NameToLayer("Marker");
        int layerMask = 1 << layerNo;

        if (Physics.Raycast(ray, out hit, playerSetting.LOD, layerMask))
        {
            Vector3 v = hit.transform.position - transform.position;

            if (v.magnitude <= playerSetting.LOD)
            {
                if (target != null)
                {
                    if (target != hit.collider.gameObject)
                    {
                        target.GetComponent<Marker>().STATE = Marker.State.None;
                    }
                }

                target = hit.collider.gameObject;

                if (target.GetComponent<Marker>().LOCK_OBSERVE)
                {
                    lockState = true;
                }

                if (target.GetComponent<Marker>().STATE != Marker.State.Choice)
                {
                    target.GetComponent<Marker>().STATE = Marker.State.Choice;
                }
                else
                {
                    target.GetComponent<Marker>().Execute(transform.position);
                }

                if (target.GetComponent<Marker>().TYPE == Marker.Type.Enemy)
                {
                    mode = Mode.Battle;
                    weapon.SetActive(true);
                }

                FollowingCamera.LOCK = target;
            }
            else
            {
                noLock = true;
            }
        }
        else
        {
            noLock = true;
        }

        if (noLock && mode == Mode.Normal)
        {
            if (target != null)
            {
                target.GetComponent<Marker>().STATE = Marker.State.None;
            }
            target = null;
            lockState = false;
            weapon.SetActive(false);

            FollowingCamera.LOCK = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // エネミーかどうか判定
        if (other.tag == "Enemy")
        {
            int enemyId = 0;
            int userId = UserRecord.ID;
            int mapId = 0;
            // ここでデータを送る
            WS.WsPlay.Instance.Send(new Packes.Attack(enemyId, userId, skilId, mapId).ToJson());
        }
    }

    private void OnCollisionStay(Collision _coll)
    {
        if (currentState is KeyMoveState) sound_.WalkPlay(_coll.gameObject.tag);
    }

    public float Distance(GameObject _target)
    {
        if (_target != null)
        {
            return Vector3.Distance(_target.transform.position, transform.position);
        }
        else { return -1; }
    }
}
