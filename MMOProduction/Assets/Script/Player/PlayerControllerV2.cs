using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    [SerializeField, Header("プレイヤー")]
    private Player PlayerData;

    [SerializeField, Header("カメラ")]
    private FollowingCamera FollowingCamera;

    [SerializeField, Header("プレイヤーの設定ファイル")]
    private PlayerSetting playerSetting;

    // 現在のステート
    BaseState currentState;

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
    float dir;
    public float Direction
    {
        get { return dir; }
    }


    // Use this for initialization
    void Start()
    {
        IdleState.Instance.Initialized(this, playerSetting);
        KeyMoveState.Instance.Initialized(this, playerSetting);
        MouseMoveState.Instance.Initialized(this, playerSetting);
        AutoRunState.Instance.Initialized(this, playerSetting);

        currentState = IdleState.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Execute();
    }

    public void Move(Vector3 velocity)
    {
        // 移動方向に回転
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(FollowingCamera.Angle * velocity), playerSetting.TS);
        
        // 移動
        Vector3 pos = transform.position + FollowingCamera.Angle * velocity;

        SendMoveDeta(pos, rot.eulerAngles.y);
    }

    public void MouseMove(Vector3 velocity)
    {
        // 移動方向に回転
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), playerSetting.TS);

        // 移動
        Vector3 pos = transform.position + velocity;

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
}
