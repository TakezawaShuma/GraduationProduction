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
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(FollowingCamera.Angle * velocity), playerSetting.TS);
        
        // 移動
        transform.position += FollowingCamera.Angle * velocity;
    }

    public void SendMoveDeta(Vector3 position, float direction)
    {
        float x, y, z, dir;

        x = position.x;
        y = position.y;
        z = position.z;
        dir = direction;

       // PlayerData.X = x;
       // PlayerData.Y = y;
       // PlayerData.Z = z;
       // PlayerData.Dir = dir;
    }

    public void ChangeState(BaseState state)
    {
        currentState = state;
    }
}
