using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public class PlayerData
    {
        public float X;
        public float Y;
        public float Z;

        public float HP;
        public float MP;

        public float Direction;
    }

    private PlayerData playerData;

    [SerializeField, Header("カメラ")]
    private FollowingCamera Camera;

    [SerializeField, Header("通常スピード(1秒間で進む距離)")]
    private float NomalSpeed;

    [SerializeField, Header("ダッシュスピード(1秒間で進む距離)")]
    private float DashSpeed;

    [SerializeField, Header("ジャンプ力")]
    private float JumpPower;

    [SerializeField, Header("移動キー")]
    private KeyCode FrontKey;

    [SerializeField]
    private KeyCode BackKey;

    [SerializeField]
    private KeyCode LeftKey;

    [SerializeField]
    private KeyCode RightKey;

    [SerializeField, Header("ダッシュキー")]
    private KeyCode DashKey;

    [SerializeField, Header("ジャンプキー")]
    private KeyCode JumpKey;

    [SerializeField, Range(0f, 1f), Header("振り向き速度")]
    private float TurnSpeed;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        // 移動
        Move();
        
	}

    /// <summary>
    /// 移動関数
    /// </summary>
    public void Move()
    {
        // 移動速度
        Vector3 velocity = Vector3.zero;

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
        }
        else
        {
            velocity *= NomalSpeed * Time.deltaTime;
        }

        if (velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Camera.Angle * -velocity), TurnSpeed);

            transform.position += Camera.Angle * velocity;
        }

        if(Input.GetKeyDown(JumpKey))
        {
            GetComponent<Rigidbody>().AddForce(0, JumpPower, 0);
        }
    }
}
