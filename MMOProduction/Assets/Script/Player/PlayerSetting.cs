//////////////////////////////////
// Playerの初期設定用のクラス？ //
//////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    [SerializeField, Header("通常スピード(1秒間で進む距離)")]
    private float NomalSpeed = 5f;
    public float NS { get { return NomalSpeed; } }

    [SerializeField, Header("ダッシュスピード(1秒間で進む距離)")]
    private float DashSpeed = 12.5f;
    public float DS { get { return DashSpeed; } }

    [SerializeField, Header("ジャンプ力")]
    private float JumpPower = 300f;
    public float JP { get { return JumpPower; } }

    [SerializeField, Range(0f, 1f), Header("振り向き速度")]
    private float TurnSpeed = 0.15f;
    public float TS { get { return TurnSpeed; } }

    [SerializeField, Header("アニメーションをするか")]
    private bool IsAnimator = false;
    public bool IA { get { return IsAnimator; } }

    [SerializeField, Range(0f,100f), Header("ロックオンの距離")]
    private float LockOnDirection;

    public float LOD { get { return LockOnDirection; } }

    [SerializeField, Header("移動キー")]
    private KeyCode FrontKey = KeyCode.W;
    public KeyCode FKey { get { return FrontKey; } }

    [SerializeField]
    private KeyCode BackKey = KeyCode.S;
    public KeyCode BKey { get { return BackKey; } }

    [SerializeField]
    private KeyCode LeftKey = KeyCode.A;
    public KeyCode LKey { get { return LeftKey; } }

    [SerializeField]
    private KeyCode RightKey = KeyCode.D;
    public KeyCode RKey { get { return RightKey; } }

    [SerializeField, Header("ダッシュキー")]
    private KeyCode DashKey = KeyCode.LeftShift;
    public KeyCode DKey { get { return DashKey; } }

    [SerializeField, Header("ジャンプキー")]
    private KeyCode JumpKey = KeyCode.Space;
    public KeyCode JKey { get { return JumpKey; } }

    [SerializeField, Header("オートランキー")]
    private KeyCode AutoRunKey = KeyCode.T;
    public KeyCode AKey { get { return AutoRunKey; } }
}
