using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private PlayerData playerData;

    private string jsonData;

    [SerializeField, Header("カメラ")]
    private FollowingCamera FollowingCamera;

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

    [SerializeField, Header("オートラン")]
    private KeyCode AutoRunKey;


    [SerializeField, Range(0f, 1f), Header("振り向き速度")]
    private float TurnSpeed;

    private bool autoRunState;

	// Use this for initialization
	void Start ()
    {
        autoRunState = false;

        playerData = new PlayerData();

        playerData.X = 10;
        playerData.Y = 5;
        playerData.Z = 2;
        playerData.HP = 100;
        playerData.MP = 50;
        playerData.Direction = 180;
        Debug.Log(JsonUtility.ToJson(playerData));
    }

	
	
	// Update is called once per frame
	void Update ()
    {
        // キーを押したら切り替える
        if(Input.GetKeyDown(AutoRunKey))
        {
            autoRunState = !autoRunState;
        }

        // 移動
        Move();

        //UpdatePlayerData();

        //UpdateJsonData();
	}

    /// <summary>
    /// 移動関数
    /// </summary>
    public void Move()
    {
        // 移動速度
        Vector3 velocity = Vector3.zero;

        if (!autoRunState)
        {
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
        }
        else
        {
            velocity = new Vector3(-1, 0, 0) * DashSpeed * Time.deltaTime;
        }

        if (velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(FollowingCamera.Angle * -velocity), TurnSpeed);

            transform.position += FollowingCamera.Angle * velocity;
        }
    }

    private void UpdatePlayerData()
    {
        playerData.X = transform.position.x;
        playerData.Y = transform.position.y;
        playerData.Z = transform.position.z;
        playerData.HP = 100;
        playerData.MP = 100;
        playerData.Direction = transform.rotation.y;
    }

    private void UpdateJsonData()
    {
        jsonData = JsonUtility.ToJson(playerData);

        // Debug.Log(jsonData);
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Ground")
    //    {
    //        if (Input.GetKeyDown(JumpKey))
    //        {
    //            GetComponent<Rigidbody>().AddForce(0, JumpPower, 0);
    //        }
    //    }
    //}
}
