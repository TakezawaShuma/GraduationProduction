////////////////////////////////////////////////
// パケットデータ等通信で使うデータをまとめた //
////////////////////////////////////////////////
using UnityEngine;


/// <summary>
/// パケットデータ
/// </summary>
namespace Packes
{

    public abstract class IPacketDatas
    {
        public int command;

        public CommandData Command { get { return (CommandData)command; } }
    }

    // ログインシーン--------------------------------------------------------------------------------------------------

    // command 101  ユーザー作成 (client->server)
    public class CreateUser : IPacketDatas
    {
        public CreateUser() { command = (int)CommandData.CreateUser; }
        public CreateUser(string name, string password) { command = 101; user_name = name; pass = password; }
        public string user_name;
        public string pass;
    }
    // command 102  ログイン (client->server)
    public class Login : IPacketDatas
    {
        public Login() { command = (int)CommandData.Login; }
        public Login(string name, string password) { command = (int)CommandData.Login; user_name = name; pass = password; }
        public string user_name;
        public string pass;

    }
    // command 103  確認OK (server->client)
    public class OKConfirmation : IPacketDatas
    {
        public OKConfirmation() { command = (int)CommandData.OKConfirmation; }
        public int user_id;
    }
    // command 104  確認取れない (server->client) エラーコマンド
    public class MissingConfirmation : IPacketDatas
    {
        public MissingConfirmation() { command = (int)CommandData.MissingConfirmation; }
    }

    // command 105　新規作成完了(server->client)
    public class CreateReport : IPacketDatas
    {
        public CreateReport() { command = (int)CommandData.CreateReport; }
        public int user_id;
    }

    // command 106　すでに存在している(server->client)    エラーコマンド
    public class Existing : IPacketDatas
    {
        public Existing() { command = (int)CommandData.Existing; }
    }

    // command 107 再接続(client->server)
    public class ReconnectC2S:IPacketDatas
    {
        public ReconnectC2S() { command = (int)CommandData.ReconnectC2S; }
    }

    // command 108 再接続(server->client)
    public class ReconnectS2C : IPacketDatas
    {
        public ReconnectS2C() { command = (int)CommandData.ReconnectS2C; }
    }


    // プレイシーン----------------------------------------------------------------------------------------------------


    // command 201  位置同期(client->server)
    public class SendPosSync : IPacketDatas
    {
        public SendPosSync() { command = (int)CommandData.SendPosSync; }
        public SendPosSync(PlayerData _data) {
            command = (int)CommandData.SendPosSync; user_id = _data.id; x = _data.x; y = _data.y; z = _data.z; dir = _data.dir;
        }
        public SendPosSync(int _id, float _x, float _y, float _z, int _dir) {
            command = (int)CommandData.SendPosSync; user_id = _id; command = 201; x = _x; y = _y; z = _z; dir = _dir;
        }
        public int user_id;
        public float x;
        public float y;
        public float z;
        public int dir;
    }

    // command 202  位置同期(server->client)
    public class RecvPosSync : IPacketDatas
    {


        public RecvPosSync() { command = (int)CommandData.RecvPosSync; }
        public RecvPosSync(int _id, float _x, float _y, float _z, int _dir) {
            command = (int)CommandData.RecvPosSync; user_id = _id; x = _x; y = _y; z = _z; dir = _dir;
        }
        public int user_id;
        public float x;
        public float y;
        public float z;
        public int dir;
    }

    // command 203　初期ログイン(client->server)
    public class SendInitialLogin : IPacketDatas
    {
        public SendInitialLogin(int _id) {
            command = (int)CommandData.SendInitialLogin; user_id =_id;
        }
        public int user_id;
    }

    // command 204　初期ログイン(server->client)
    public class RecvInitialLogin : IPacketDatas
    {
        public RecvInitialLogin() { command = (int)CommandData.RecvInitialLogin; }
        public int user_id;
        public float x;
        public float y;
        public float z;
    }

    // command 205 ステータス共有(client->server)
    public class SendStatus : IPacketDatas{
        public SendStatus() { command = (int)CommandData.SendStatus; user_id = Retention.ID; }
        public SendStatus(int _hp,int _mp,int _status) {
            command = (int)CommandData.SendStatus; user_id = Retention.ID; hp = _hp; mp = _mp; status = _status;
        }
        public int user_id;
        public int hp;
        public int mp;
        public int status;
    }

    // command 206 ステータス共有(server->client)
    public class RecvStatus : IPacketDatas
    {
        public RecvStatus() { command = (int)CommandData.RecvStatus; }
    }

    // command 209 セーブデータの読み込み要請(client->server)
    public class SaveDataRequ:IPacketDatas
    {
        public SaveDataRequ(int _id) { command = (int)CommandData.SaveDataRequ; user_id = _id;}
        public int user_id;
    }
    
    // command 210 セーブデータ(server->client)
    public class RecvSaveData : IPacketDatas
    {
        public RecvSaveData() { command = (int)CommandData.RecvSaveData; }
        public Weapon weapon;
        public Vector3 position;
        public int lv;
        public int exp;
    }

    // command 211 データの読み込み完了(client->server)
    public class DataLoadComplete:IPacketDatas
    {
        public DataLoadComplete() { command = (int)CommandData.DataLoadComplete; }

    }


    // その他データ----------------------------------------------------------------------------------------------------


    // command 701　ログアウト時(client->server)
    public class Finished : IPacketDatas
    {
        public Finished() { command = (int)CommandData.Finished; }
        public int level;
        public int hp;
        public int mp;
        public float x;
        public float y;
        public float z;
        public float exp;
    }

    // command 702 アイテム一覧(client->server)
    public class SendItemList : IPacketDatas
    {
        public SendItemList() { command = (int)CommandData.SendItemList; }

    }

    // command 703 スキル一覧(client->server)
    public class SendSkillList : IPacketDatas
    {
        public SendSkillList() { command = (int)CommandData.SendSkillList; }

    }
    // command 704 アイテム一覧(server->client)
    public class RecvItemList : IPacketDatas
    {
        public RecvItemList() { command = (int)CommandData.RecvItemList; }

    }

    // command 705 スキル一覧(server->client)
    public class RecvSkillList : IPacketDatas
    {
        public RecvSkillList() { command = (int)CommandData.RecvSkillList; }

    }

    // command 706 ログアウト完了(server->client)
    public class FinishComplete : IPacketDatas
    {
        public FinishComplete() { command = (int)CommandData.FinishComplete; }
    }


    // チャット--------------------------------------------------------------------------------------------------------

    // command 801　全体チャット(client->server)
    public class SendAllChat:IPacketDatas
    {
        public SendAllChat() { command = (int)CommandData.SendAllChat; }
        public SendAllChat(string _name, string _msg)
        { command = (int)CommandData.SendAllChat; user_name = _name; message = _msg; }
        public string user_name;
        public string message;
    }

    // command 802 全体チャット(server->client)
    public class RecvAllChat:IPacketDatas
    {
        public RecvAllChat() { command = (int)CommandData.RecvAllChat; }

        
        public string user_name;
        public string message;
    }

    //// command 803 全体チャット受信(client->server)
    //public class RecvAllChat:IPacketDatas
    //{
    //    RecvAllChat() { command = 803; }
    //}


    // エラーコマンド--------------------------------------------------------------------------------------------------

    // command 901　重複ログイン(server->client)   エラーコマンド
    public class Duplicate:IPacketDatas
    {
        public Duplicate() { command = (int)CommandData.Duplicate; }
    }
}

/// <summary>
/// コマンド
/// </summary>
public enum CommandData
{
    // 新規作成
    CreateUser = (int)101,
    // ログイン
    Login = (int)102,
    // ログイン確認
    OKConfirmation = (int)103,
    // ログイン失敗   エラーコマンド
    MissingConfirmation = (int)104,
    // 新規作成報告
    CreateReport = (int)105,
    // すでに存在している   エラーコマンド
    Existing = (int)106,
    // 再接続
    ReconnectC2S = (int)107,
　　// 再接続確認
    ReconnectS2C = (int)108,


    // プレイヤーデータの送信
    SendPosSync = (int)201,
    // プレイヤーデータの受信
    RecvPosSync = (int)202,
    // 初期ログインの送信
    SendInitialLogin = (int)203,
    // 初期ログインの受信
    RecvInitialLogin = (int)204,
    // ステータスの送信
    SendStatus = (int)205,
    // ステータスの受信
    RecvStatus = (int)206,
    // セーブデータ要請
    SaveDataRequ = (int)209,
    // セーブデータ
    RecvSaveData = (int)210,
    // データ読み込み完了
    DataLoadComplete = (int)211,


    // ログアウト要請
    Finished = (int)701,
    // アイテム一覧送信
    SendItemList = (int)702,
    // スキル一覧送信
    SendSkillList = (int)703,
    // アイテム一覧受信
    RecvItemList = (int)704,
    // スキル一覧受信
    RecvSkillList = (int)705,
    // ログアウト完了
    FinishComplete = (int)706,


    // チャット
    // 全体チャット送信
    SendAllChat = (int)801,
    // 全体チャット受信
    RecvAllChat = (int)802,
    // 重複ログイン報告   エラーコマンド
    Duplicate = 901,
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
    private static string name;
    public static int ID { get { return id; }set { if(id==0)id = value; } }
    public static string Name { get { return name; } set { if (name=="") name = value; } }
}