using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCommand : MonoBehaviour
{
    [SerializeField, Header("プレイヤーセッティング")]
    private PlayerSetting playerSetting = null;

    [SerializeField, Header("速度上昇値")]
    private float upSpeedValue = 1.0f;

    [SerializeField, Header("速度上昇コマンド")]
    private Command upSpeedCommand = null;

    [SerializeField, Header("速度リセットコマンド")]
    private Command resetSpeedCommand = null;

    [SerializeField, Header("位置リセットコマンド")]
    private Command resetPosCommand = null;

    [SerializeField, Header("プレイヤー")]
    private GameObject player = null;

    public GameObject PLAYER
    {
        set { player = value; resetPosCommand.SetAction(ResetPos); }
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
}
