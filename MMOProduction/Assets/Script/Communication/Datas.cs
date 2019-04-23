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
        public CreateUser() { command = 101; }
        public CreateUser(string name,string password) { command = 101;user_name = name;pass = password; }
        public string user_name;
        public string pass;
    }
    // command 102  ログイン
    public class Login : IPacketDatas
    {
        public Login() { command = 102; }
        public Login(string name, string password) { command = 102; user_name = name; pass = password; }
        public string user_name;
        public string pass;

    }
    // command 103  確認OK
    public class OKConfirmation : IPacketDatas
    {
        public OKConfirmation() { command = 103; }
        public int user_id;
    }
    // command 104  確認取れない
    public class MissingConfirmation : IPacketDatas
    {
        public MissingConfirmation() { command = 104; }
    }

    // command 105　新規作成完了(server->client)
    public class CreateReport:IPacketDatas
    {
        public CreateReport() { command = 105; }
        public int user_id;
    }

    // command 106　すでに存在している(server->client)
    public class Existing:IPacketDatas
    {
        public Existing() { command = 106; }
    }


    // command 201  位置同期(client->server)
    public class SendPosSync : IPacketDatas
    {
        public SendPosSync() { command = 201; }
        public int user_id;
        public float x;
        public float y;
        public float z;
        public int hp;
        public int mp;
        public int dir;
    }

    // command 202  位置同期(server->client)
    public class RecvPosSync : IPacketDatas
    {
        public RecvPosSync() { command = 202; }
        public float x;
        public float y;
        public float z;
        public int hp;
        public int mp;
        public int dir;
    }

    // command 203　初期ログイン(client->server)
    public class SendInitialLogin:IPacketDatas
    {
        public SendInitialLogin() { command = 203; }
        public int user_id;
        public float x;
        public float y;
        public float z;
    }

    // command 204　初期ログイン(server->client)
    public class RecvInitialLogin:IPacketDatas
    {
        public RecvInitialLogin() { command = 204; }
        public int user_id;
        public float x;
        public float y;
        public float z;
    }

    // command 701　ログアウト時(client->server)
    public class Finished : IPacketDatas
    {
        public Finished() { command = 701; }
        public int level;
        public int hp;
        public int mp;
        public float x;
        public float y;
        public float z;
        public float exp;
    }

    // command 801　チャット()
    public class Chat:IPacketDatas
    {
        public Chat() { command = 801; }
    }

    // command 901　重複ログイン(server->client)
    public class Duplicate:IPacketDatas
    {
        public Duplicate() { command = 901; }
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
    CmdCreateReport=(int)105,
    CmdExisting=(int)106,
    CmdSendPosSync = (int)201,
    CmdRecvPosSync = (int)202,
    CmdFinished=(int)701,
    CmdChat=(int)801,
    CmdDuplicate=901,
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