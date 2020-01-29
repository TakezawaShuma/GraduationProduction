//////////////////////////////////
// プレイヤーコントロールクラス //
//////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("プレイヤー")]
    private Player player = null;

    [SerializeField, Header("カメラ")]
    private FollowingCamera followingCamera = null;

    [SerializeField, Header("プレイヤーの設定ファイル")]
    private PlayerSetting playerSetting = null;

    [SerializeField, Header("アニメーターコントローラー")]
    private Animator animator = null;

    [SerializeField, Header("チャットコントローラー")]
    private ChatController chatController = null;

    private GameObject weapon = null;
    private WeaponList weaponList = null;


    // プレイヤーの武器プロパティ
    public Sword Sword { get; private set; }

    // 現在のステート
    private BaseState currentState;

    private AnimatorManager animatorManager;

    private bool lockState = false;
    public bool Lock { set { lockState = value; } }

    private GameObject target;
    public GameObject Target { get { return target; } set { target = value; } }
    
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

    public void Init(Player _player, FollowingCamera _camera, PlayerSetting _setting, ChatController _chat, Animator _animator)
    {
        player = _player;
        followingCamera = _camera;
        playerSetting = _setting;
        chatController = _chat;
        animator = _animator;
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
        animatorManager = new AnimatorManager();
        animatorManager.ANIMATOR = animator;
        IdleState.Instance.Initialized(this, playerSetting, animatorManager, player);
        KeyMoveState.Instance.Initialized(this, playerSetting, animatorManager, player);
        AutoRunState.Instance.Initialized(this, playerSetting, animatorManager, player);
        NormalAttackState.Instance.Initialized(this, playerSetting, animatorManager, player);
        SkillUsingState.Instance.Initialized(this, playerSetting, animatorManager, player);

        // 初期武器を取得
        weaponList = GetComponent<WeaponList>();
        weapon = weaponList.GetWeapons(0);
        // 武器リストから各種武器の参照を取得
        Sword = weaponList.GetWeapons(0).GetComponent<Sword>();

        currentState = IdleState.Instance;
        currentState.Start();
        

        sound_ = GetComponent<PlayerSound>();
    }



    // Update is called once per frame
    void Update()
    {
        if (chatController == null || !chatController.IsChatActive())
        {
            if (target != null)
            {
                Vector3 v = target.transform.position - player.PositionV3;

                if (v.magnitude > playerSetting.LOD - 7)
                {
                    RemoveTarget();
                }
            }

            if (InputManager.InputMouseCheckDown(1) == INPUT_MODE.PLAY && target != null)
            {
                RemoveTarget();
            }

            currentState.Execute();
        }

        // デバッグ スキル使用
        // ファイア・ボール
        if (InputManager.InputKeyCheckDown(KeyCode.Alpha1))
        {
            Vector3 pos = player.PositionV3;
            pos.y += 2;
            pos += player.transform.forward * 1;
            Quaternion rot = player.Rotation;
            SkillHandler.Instance.RequestToUseSkill(SkillID.MiniIce, gameObject, pos, rot);
        }
    }

    public void NoMove()
    {
        Quaternion rot = player.Rotation;

        if (lockState)
        {
            Vector3 dir = target.transform.position - player.PositionV3;
            rot = Quaternion.Slerp(player.Rotation, Quaternion.LookRotation(dir), playerSetting.TS);
        }

        Vector3 vel = player.Rigid.velocity;
        vel.x = 0;
        vel.z = 0;

        player.Rigid.velocity = vel;
        player.Rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);

        SendMoveDeta(player.PositionV3, player.Rotation.eulerAngles.y);
    }

    public void Move(Vector3 velocity)
    {
        Quaternion rot = new Quaternion();

        Vector3 v = followingCamera.Angle * velocity;
        if (lockState)
        {
            Vector3 dir = target.transform.position - player.PositionV3;
            rot = Quaternion.Slerp(player.Rotation, Quaternion.LookRotation(dir), playerSetting.TS);
        }
        else
        {
            // 移動方向に回転
            rot = Quaternion.Slerp(player.Rotation, Quaternion.LookRotation(v), playerSetting.TS);
        }


        // 移動
        player.Rigid.velocity=new Vector3(v.x, player.Rigid.velocity.y, v.z);
        player.Rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);

        SendMoveDeta(player.PositionV3, rot.eulerAngles.y);
    }

    public void SendMoveDeta(Vector3 position, float direction)
    {
        player.PositionV4 = new Vector4(position.x, position.y, position.z, direction);
    }

    public void ChangeState(BaseState state)
    {
        currentState.End();
        currentState = state;
        currentState.Start();
    }

    public void LockOn()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // かぶさってるので処理キャンセル
            return;
        }

        bool noLock = false;

        Ray ray = followingCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        int layerNo = LayerMask.NameToLayer("Marker");
        int layerMask = 1 << layerNo;

        if (Physics.Raycast(ray, out hit, playerSetting.LOD, layerMask))
        {
            Vector3 v = hit.transform.position - player.PositionV3;

            if (v.magnitude <= playerSetting.LOD)
            {
                if (target != null)
                {
                    if (target != hit.collider.gameObject)
                    {
                        RemoveTarget();
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
                    if (target.GetComponent<Marker>().TYPE == Marker.Type.Enemy)
                    {
                        target.GetComponentInParent<Enemy>().UI_HP.On();
                    }
                }
                else
                {
                    target.GetComponent<Marker>().Execute(player.PositionV3);
                }

                if (target.GetComponent<Marker>().TYPE == Marker.Type.Enemy)
                {
                    mode = Mode.Battle;
                    Sword.gameObject.SetActive(true);
                }

                followingCamera.LOCK = target;
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
            RemoveTarget();
        }
    }


    private void OnCollisionStay(Collision _coll)
    {
        if (currentState is KeyMoveState && InputManager.InputKeyCheck(playerSetting.DKey)) { sound_.RunPlay(_coll.gameObject.tag); }
        else if(currentState is KeyMoveState) { sound_.WalkPlay(_coll.gameObject.tag); }
    }

    public float Distance(GameObject _target)
    {
        if (_target != null)
        {
            return Vector3.Distance(_target.transform.position, player.PositionV3);
        }
        else { return -1; }
    }

    public void RemoveTarget()
    {
        mode = Mode.Normal;
        if (target)
        {
            target.GetComponent<Marker>().STATE = Marker.State.None;
            if (target.GetComponent<Marker>().TYPE == Marker.Type.Enemy)
            {
                target.GetComponentInParent<Enemy>().UI_HP.Off();
            }
        }
        target = null;
        lockState = false;
        followingCamera.LOCK = null;
        ReleaseWeapon();
    }

    public void ReleaseWeapon()
    {
        if (target == null)
        {
            //weapon.gameObject.SetActive(false);
            Sword.gameObject.SetActive(false);
        }
    }
}
