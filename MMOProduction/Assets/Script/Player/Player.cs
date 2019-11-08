//////////////////////
// プレイヤークラス //
//////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    [SerializeField, Header("プレイヤーの設定")]
    private PlayerSetting playerSetting;

    private float x;
    private float y;
    private float z;

    private Status status;
    public int hp { get; set; }
    public int mp { get; set; }
    public int maxHp { get; set; }
    public int maxMp { get; set; }
    private int strength;
    private int vitality;
    private int Intelligence;
    private int mind;
    private int dexterity;
    private int agility;

    private float dir;

    private Rigidbody rb;

    // プレイヤー用コマンド
    private CommandAttack _attackCommand;
    public CommandAttack AttackCommand { get { return _attackCommand; } }

    // プレイヤーID
    public int ID { get; set; }


    private void Start()
    {
        x = 0;
        y = 0;
        z = 0;

        dir = transform.rotation.eulerAngles.y;

        rb = GetComponent<Rigidbody>();

        // プレイヤーが使用するコマンドを登録
        _attackCommand = new CommandAttack(this);
        _attackCommand.Kill = (() => {
            return Input.GetKeyDown(KeyCode.Alpha1);
        });
        _attackCommand.Punch = (() => {
            return Input.GetKeyDown(KeyCode.Alpha2);
        });
    }

    private void Update()
    {
        //rb.AddForce(new Vector3(x, y, z) * 100);
        //transform.rotation = Quaternion.Euler(0, dir, 0);
    }

    public void UpdatePosition(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public void UpdateDirection(float _dir)
    {
        dir = _dir;
    }

    public Vector4 GetPosition()
    {
        return new Vector4(x, y, z, dir);
    }
}
