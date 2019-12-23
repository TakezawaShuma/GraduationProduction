using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCommand : MonoBehaviour
{
    [SerializeField, Header("プレイヤーセッティング")]
    private PlayerSetting playerSetting = null;

    [SerializeField, Header("速度上昇値")]
    private float upSpeedValue = 1.0f;

    [SerializeField, Header("上昇下降速度")]
    private float upDownSpeed = 1.0f;

    [SerializeField, Header("速度上昇コマンド")]
    private Command upSpeedCommand = null;

    [SerializeField, Header("速度リセットコマンド")]
    private Command resetSpeedCommand = null;

    [SerializeField, Header("位置リセットコマンド")]
    private Command resetPosCommand = null;

    [SerializeField, Header("空中歩行コマンド")]
    private Command airWalkCommand = null;

    [SerializeField, Header("プレイヤー")]
    private GameObject player = null;

    private bool airFlag = false;

    public GameObject PLAYER
    {
        set { player = value; }
    }

    [SerializeField, Header("初期位置")]
    private Vector3 startPos = Vector3.zero;

    private float startDashSpeedValue = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        startDashSpeedValue = playerSetting.DS;

        upSpeedCommand.SetAction(UpDashSpeed);
        resetSpeedCommand.SetAction(ResetDashSpeed);
        resetPosCommand.SetAction(ResetPos);
        airWalkCommand.SetAction(AirWalk);
    }

    void Update()
    {
        if(airFlag)
        {
            Vector3 vel = player.GetComponent<Rigidbody>().velocity;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                vel.y = upDownSpeed;
            }
            else if(Input.GetKey(KeyCode.DownArrow))
            {
                vel.y = -upDownSpeed;
            }
            else
            {
                vel.y = 0;
            }

            player.GetComponent<Rigidbody>().velocity = vel;
        }
    }

    private void UpDashSpeed()
    {
        playerSetting.DS += upSpeedValue;
    }

    private void ResetDashSpeed()
    {
        playerSetting.DS = startDashSpeedValue;
    }

    private void ResetPos()
    {
        player.transform.position = startPos;
    }

    private void AirWalk()
    {
        airFlag = !airFlag;

        player.GetComponent<Rigidbody>().useGravity = !player.GetComponent<Rigidbody>().useGravity;
    }
}
