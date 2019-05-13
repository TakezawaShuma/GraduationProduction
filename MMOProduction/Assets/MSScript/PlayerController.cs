using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Player PlayerData;

    [SerializeField, Header("カメラ")]
    private FollowingCamera FollowingCamera;

    [SerializeField, Header("通常スピード(1秒間で進む距離)")]
    private float NomalSpeed = 5f;

    [SerializeField, Header("ダッシュスピード(1秒間で進む距離)")]
    private float DashSpeed = 12.5f;

    [SerializeField, Header("ジャンプ力")]
    private float JumpPower = 300f;

    [SerializeField, Header("移動キー")]
    private KeyCode FrontKey = KeyCode.W;

    [SerializeField]
    private KeyCode BackKey = KeyCode.S;

    [SerializeField]
    private KeyCode LeftKey = KeyCode.A;

    [SerializeField]
    private KeyCode RightKey = KeyCode.D;

    [SerializeField, Header("ダッシュキー")]
    private KeyCode DashKey = KeyCode.LeftShift;

    [SerializeField, Header("ジャンプキー")]
    private KeyCode JumpKey = KeyCode.Space;

    [SerializeField, Header("オートランキー")]
    private KeyCode AutoRunKey = KeyCode.T;

    [SerializeField, Range(0f,1f), Header("振り向き速度")]
    private float TurnSpeed = 0.15f;

    [SerializeField, Header("アニメーションをするか")]
    private bool IsAnimator = false;

    [SerializeField, Header("アニメーター")]
    private Animator Animator;

    [SerializeField]
    private bool IsNetwork = true;

    // 速度
    private Vector3 velocity;

    private Vector3 clickPosition;

    private RaycastHit hit;

    private Ray ray;

    private bool runState;

    private bool jumpState;

    private string jsonData;

    private enum MoveMode
    {
        Key,
        Auto,
        Click,
    }

    private MoveMode moveMode;

	// Use this for initialization
	void Start ()
    {
        runState = false;
        jumpState = false;

        moveMode = MoveMode.Key;
        
        //UpdatePlayerData();
        //Debug.Log(JsonUtility.ToJson(PlayerData));
    }
	
	// Update is called once per frame
	void Update ()
    {
        // キーを押したら切り替える
        if(Input.GetKeyDown(AutoRunKey))
        {
            if (moveMode != MoveMode.Auto)
            {
                moveMode = MoveMode.Auto;
            }
            else
            {
                moveMode = MoveMode.Key;
            }
        }

        if (Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                clickPosition = hit.point;
                moveMode = MoveMode.Click;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            moveMode = MoveMode.Key;
        }

        // 移動
        Move();
    }

    /// <summary>
    /// 移動関数
    /// </summary>
    public void Move()
    {
        velocity = Vector3.zero;

        switch(moveMode)
        {
            case MoveMode.Key:
                MoveModeKey();
                break;
            case MoveMode.Auto:
                MoveModeAuto();
                break;
            case MoveMode.Click:
                MoveModeClick();
                break;
        }

        // 移動量が0より上であれば
        if (velocity.magnitude > 0)
        {
            if (moveMode != MoveMode.Click)
            {
                if (IsNetwork)
                {
                    Vector3 afterMoving = transform.position + FollowingCamera.Angle * velocity;
                    float afterDirection = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(FollowingCamera.Angle * velocity), TurnSpeed).eulerAngles.y;

                    SendMoveDeta(afterMoving, afterDirection);
                }
                else
                {
                    // 移動方向に回転
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(FollowingCamera.Angle * velocity), TurnSpeed);

                    // 移動
                    transform.position += FollowingCamera.Angle * velocity;
                }
            }
            else
            {
                if (IsNetwork)
                {
                    Vector3 afterMoving = transform.position + velocity;
                    float afterDirection = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), TurnSpeed).eulerAngles.y;

                    SendMoveDeta(afterMoving, afterDirection);
                }
                else
                {
                    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), TurnSpeed);
                    Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), TurnSpeed);
                    transform.rotation = rot;
                    transform.position += velocity;
                }
            }

            if(runState)
            {
                SetRun();
            }
            else
            {
                SetWalk();
            }
        }
        else
        {
            SetIdle();
        }

        // ジャンプ処理
        if (Input.GetKeyDown(JumpKey))
        {
            // ジャンプ状態でなければジャンプをする
            if (!jumpState)
            {
                GetComponent<Rigidbody>().AddForce(0, JumpPower, 0);
                jumpState = true;
            }
        }
    }

    private void SendMoveDeta(Vector3 position, float direction)
    {
        float x, y, z, dir;

        x = position.x;
        y = position.y;
        z = position.z;
        dir = direction;

        PlayerData.X = x;
        PlayerData.Y = y;
        PlayerData.Z = z;
        PlayerData.Dir = dir;
    }

    /// <summary>
    /// 移動モードがキー状態の時に呼ぶ関数
    /// </summary>
    private void MoveModeKey()
    {
        // キー判定
        if (Input.GetKey(FrontKey))
        {
            velocity.x -= 1;
        }
        else if (Input.GetKey(BackKey))
        {
            velocity.x += 1;
        }

        if (Input.GetKey(LeftKey))
        {
            velocity.z -= 1;
        }
        else if (Input.GetKey(RightKey))
        {
            velocity.z += 1;
        }

        // 正規化
        velocity = velocity.normalized;

        // 押していたらダッシュ
        if (Input.GetKey(DashKey))
        {
            velocity *= DashSpeed * Time.deltaTime;
            runState = true;
        }
        else
        {
            velocity *= NomalSpeed * Time.deltaTime;
            runState = false;
        }
    }

    /// <summary>
    /// 移動モードがオート状態の時に呼ぶ関数
    /// </summary>
    private void MoveModeAuto()
    {
        velocity = new Vector3(-1, 0, 0) * DashSpeed * Time.deltaTime;
        runState = true;
        
        if (Input.GetKeyDown(FrontKey) || Input.GetKeyDown(BackKey) || Input.GetKeyDown(LeftKey) || Input.GetKeyDown(RightKey))
        {
            moveMode = MoveMode.Key;
        }
    }

    /// <summary>
    /// 移動モードがクリックの時に呼ぶ関数
    /// </summary>
    private void MoveModeClick()
    {
        float dis = (new Vector2(transform.position.x, transform.position.z) - new Vector2(clickPosition.x, clickPosition.z)).magnitude;
        
        //if (dis <= 0.1f)
        //{
        //    moveMode = MoveMode.Key;
        //    runState = false;
        //}
        //else
        {
            Vector2 v = new Vector2(clickPosition.x, clickPosition.z) - new Vector2(transform.position.x, transform.position.z);

            float rad = Mathf.Atan2(v.x, v.y);

            velocity.x = Mathf.Sin(rad);
            velocity.z = Mathf.Cos(rad);

            velocity = velocity.normalized;

            velocity *= DashSpeed * Time.deltaTime;
            runState = true;
        }
    }
    
    private void SetIdle()
    {
        if (IsAnimator)
        {
            Animator.SetBool("Idle", true);
            Animator.SetBool("Walk", false);
            Animator.SetBool("Run", false);
        }
    }

    private void SetWalk()
    {
        if (IsAnimator)
        {
            Animator.SetBool("Idle", false);
            Animator.SetBool("Walk", true);
            Animator.SetBool("Run", false);
        }
    }

    private void SetRun()
    {
        if (IsAnimator)
        {
            Animator.SetBool("Idle", false);
            Animator.SetBool("Walk", false);
            Animator.SetBool("Run", true);
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpState = false;
        }
    }
}
