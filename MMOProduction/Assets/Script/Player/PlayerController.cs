//////////////////////////////////
// プレイヤーコントロールクラス //
//////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    [SerializeField, Header("プレイヤー")]
    private Player PlayerData;

    [SerializeField, Header("カメラ")]
    private FollowingCamera FollowingCamera;

    [SerializeField, Header("プレイヤーの設定ファイル")]
    private PlayerSetting playerSetting;

    [SerializeField, Header("アニメーターコントローラー")]
    private Animator animator;

    [SerializeField, Header("チャットコントローラー")]
    private ChatController chatController;

    [SerializeField, Header("攻撃判定用当たり判定")]
    private CapsuleCollider attackCollider;

    public CapsuleCollider AttackCollider
    {
        get { return attackCollider; }
    }

    // 現在のステート
    private BaseState currentState;
    
    private AnimatorManager animatorManager;

    private bool lockState = false;

    private GameObject target;

    // Tama: プレイヤーアニメーションデータ
    private PlayerAnimData _playerAnim;

    private Rigidbody rigidbody;
    

    public void Init(Player _playerData,FollowingCamera _camera,PlayerSetting _setting, ChatController chat) {
        PlayerData = _playerData;
        FollowingCamera = _camera;
        playerSetting = _setting;
        chatController = chat;

        _playerAnim = new PlayerAnimData(this.gameObject);
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
        TestAttackState.Instance.Initialized(this, playerSetting, animatorManager);

        currentState = IdleState.Instance;

        rigidbody = GetComponent<Rigidbody>();
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

            currentState.Execute();
        }
    }

    public void NoMove()
    {
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
        Quaternion rot = transform.rotation;

        if (lockState)
        {
            Vector3 dir = target.transform.position - transform.position;
            rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), playerSetting.TS);
        }


        pos = rigidbody.position;

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
        rigidbody.velocity = new Vector3(v.x, rigidbody.velocity.y, v.z);

        pos = rigidbody.position;

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

        PlayerData.UpdatePosition(x, y, z);
        PlayerData.UpdateDirection(dir);
    }

    public void ChangeState(BaseState state)
    {
        currentState = state;
    }

    public void LockOn()
    {
        bool noLock = false;

        Ray ray = FollowingCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        int layerNo = LayerMask.NameToLayer("Marker");
        int layerMask = 1 << layerNo;

        if (Physics.Raycast(ray, out hit, playerSetting.LOD,layerMask))
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
                
                lockState = true;
                
                if (target.GetComponent<Marker>().STATE != Marker.State.Choice)
                {
                    target.GetComponent<Marker>().STATE = Marker.State.Choice;
                }
                else
                {
                    target.GetComponent<Marker>().Execute(transform.position);
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
        
        if(noLock)
        {
            if (target != null)
            {
                target.GetComponent<Marker>().STATE = Marker.State.None;
            }
            target = null;
            lockState = false;

            FollowingCamera.LOCK = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            // ここでデータを送る
            Debug.Log("当たってるYO");
        }
    }
}
