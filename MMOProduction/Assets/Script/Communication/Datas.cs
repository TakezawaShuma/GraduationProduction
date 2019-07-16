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

    // command 101  ユーザー作成 (client->server)
    public class CreateUser :  IPacketDatas
    {
        public CreateUser() { command = 101; }
        public CreateUser(string name,string password) { command = 101;user_name = name;pass = password; }
        public string user_name;
        public string pass;
    }
    // command 102  ログイン (client->server)
    public class Login : IPacketDatas
    {
        public Login() { command = 102; }
        public Login(string name, string password) { command = 102; user_name = name; pass = password; }
        public string user_name;
        public string pass;

    }
    // command 103  確認OK (server->client)
    public class OKConfirmation : IPacketDatas
    {
        public OKConfirmation() { command = 103; }
        public int user_id;
    }
    // command 104  確認取れない (server->client) エラーコマンド
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

    // command 106　すでに存在している(server->client)    エラーコマンド
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
        public int user_id;
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

    public class SendItemList : IPacketDatas
    {
        public SendItemList() { command = 702; }

    }
    public class SendSkillList : IPacketDatas
    {
        public SendSkillList() { command = 703; }

    }

    // command 801　チャット()
    public class Chat:IPacketDatas
    {
        public Chat() { command = 801; }
        public Chat(string _name,string _msg) {
            command = 801;
            user_name = _name;
            message = _msg;
        }
        public string user_name;
        public string message;
    }

    // command 901　重複ログイン(server->client)   エラーコマンド
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
    // 新規作成
    CmdCreateUser = (int)101,
    // ログイン
    CmdLogin = (int)102,
    // ログイン確認
    CmdOKConfirmation = (int)103,
    // ログイン失敗   エラーコマンド
    CmdMissingConfirmation = (int)104,
    // 新規作成報告
    CmdCreateReport = (int)105,
    // すでに存在している   エラーコマンド
    CmdExisting = (int)106,
    // プレイヤーデータの送信
    CmdSendPosSync = (int)201,
    // プレイヤーデータの受信
    CmdRecvPosSync = (int)202,
    // 初期ログインの送信
    CmdSendInitialLogin = (int)203,
    // 初期ログインの受信
    CmdRecvInitialLogin = (int)204,
    // ログアウトコマンド
    CmdFinished = (int)701,
    // チャット
    CmdChat = (int)801,
    // 重複ログイン報告   エラーコマンド
    CmdDuplicate = 901,
}

/// <summary>
/// シーン
/// </summary>
public enum Scenes
{
    // ログインシーン
    Login,
    // プレイシーン
    Play,
    // シーン無し
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