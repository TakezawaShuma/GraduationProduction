using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Packes
{
    public abstract class IPacketDatas
    {
        public int command;

        public CommandData Command { get { return (CommandData)command; } }
    }
    // command 101  ユーザー作成
    public class CreateUser :  IPacketDatas
    {
        public string user_name;
        public string password;
    }
    // command 102  ログイン
    public class Login : IPacketDatas
    {
        public string username;
        public string pass;

    }
    // command 103  確認OK
    public class OKConfirmation : IPacketDatas
    {
        public int user_id;
    }
    // command 104  確認取れない
    public class MissingConfirmation : IPacketDatas
    {
    }

    // command 201  位置同期(client->server)
    public class SendPosSync : IPacketDatas
    {
        public int user_id;
        public float X;
        public float Y;
        public float Z;
        public int HP;
        public int MP;
        public int Direction;
    }

    // command 202  位置同期(server->client)
    public class RecvPosSync : IPacketDatas
    {
        public float x;
        public float y;
        public float z;
        public int hp;
        public int mp;
        public int dir;
    }
}

/// <summary>
/// コマンド
/// </summary>
public enum CommandData
{
    CmdCreateUser = (int)101,
    CmdLogin = (int)102,
    CmdOKConfirmation = (int)103,
    CmdMissingConfirmation = (int)104,
    CmdSendPosSync = (int)201,
    CmdRecvPosSync = (int)202,
}
public enum Scenes
{
    Login,
    Play,
    Non,
}

/// <summary>
/// ゲーム中保持する自分のID
/// </summary>
public static class Retention
{
    private static int id;
    public static int ID { get { return id; }set { id = value; } }
}