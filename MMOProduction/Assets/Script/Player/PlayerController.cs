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

    // 現在のステート
    private BaseState currentState;

    private AnimatorManager animatorManager;

    private bool lockState = false;

    private GameObject target;

    public void Init(Player _playerData,FollowingCamera _camera,PlayerSetting _setting) {
        PlayerData = _playerData;
        FollowingCamera = _camera;
        playerSetting = _setting; 
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
        pos = transform.position;
        animatorManager = new AnimatorManager(animator);
        IdleState.Instance.Initialized(this, playerSetting, animatorManager);
        KeyMoveState.Instance.Initialized(this, playerSetting, animatorManager);
        //MouseMoveState.Instance.Initialized(this, playerSetting, animatorManager);
        AutoRunState.Instance.Initialized(this, playerSetting, animatorManager);

        currentState = IdleState.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Execute();
    }

    public void NoMove()
    {
        Quaternion rot = transform.rotation;

        if (lockState)
        {
            Vector3 dir = target.transform.position - transform.position;
            rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), playerSetting.TS);
        }

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
            rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Camera.main.GetComponent<FollowingCamera>().Angle * velocity), playerSetting.TS);
        }
        
        // 移動
        pos = transform.position + Camera.main.GetComponent<FollowingCamera>().Angle * velocity;

        SendMoveDeta(pos, rot.eulerAngles.y);
    }

    public void MouseMove(Vector3 velocity)
    {
        // 移動方向に回転
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), playerSetting.TS);

        // 移動
        pos = transform.position + velocity;

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
        if(target != null)
        {
            target.GetComponent<Marker>().FLAG = false;
        }
        target = null;
        lockState = false;

        Debug.Log(Camera.main);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5);

            Debug.Log(hit.collider.gameObject);

            if (hit.collider.gameObject.tag == "Enemy")
            {
                target = hit.collider.gameObject;

                lockState = true;

                target.GetComponent<Marker>().FLAG = true;
            }
        }
    }
}
