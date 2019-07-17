namespace Packes
{
    public abstract class IPacketDatas
    {
        public int command;

        public CommandData Command { get { return (CommandData)command; } }
    }

    // command 101  ユーザー作成 (client->server)
    public class CreateUser : IPacketDatas
    {
        public CreateUser() { command = 101; }
        public CreateUser(string name, string password) { command = 101; user_name = name; pass = password; }
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
    public class CreateReport : IPacketDatas
    {
        public CreateReport() { command = 105; }
        public int user_id;
    }

    // command 106　すでに存在している(server->client)    エラーコマンド
    public class Existing : IPacketDatas
    {
        public Existing() { command = 106; }
    }


    // command 201  位置同期(client->server)
    public class SendPosSync : IPacketDatas
    {
        public SendPosSync() { command = 201; }
        public SendPosSync(PlayerData _data) {
            command = 201; user_id = _data.id; x = _data.x; y = _data.y; z = _data.z; dir = _data.dir;
        }
        public SendPosSync(int _id, float _x, float _y, float _z, int _dir) {
            user_id = _id; command = 201; x = _x; y = _y; z = _z; dir = _dir;
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


        public RecvPosSync() { command = 202; }
        public RecvPosSync(int _id, float _x, float _y, float _z, int _dir) {
            command = 202; user_id = _id; x = _x; y = _y; z = _z; dir = _dir;
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
        public SendInitialLogin() {
            command = 203; user_id = Retention.ID;
        }
        public int user_id;
    }

    // command 204　初期ログイン(server->client)
    public class RecvInitialLogin : IPacketDatas
    {
        public RecvInitialLogin() { command = 204; }
        public int user_id;
        public float x;
        public float y;
        public float z;
    }

    public class SendStatus : IPacketDatas{
        public SendStatus() { command = 205; user_id = Retention.ID; }
        public SendStatus(int _hp,int _mp,int _status) {
            command = 205; user_id = Retention.ID; hp = _hp; mp = _mp; status = _status;
        }
        int user_id;
        int hp;
        int mp;
        int status;
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
    // プレイヤーデータの送信
    SendPosSync = (int)201,
    // プレイヤーデータの受信
    RecvPosSync = (int)202,
    // 初期ログインの送信
    SendInitialLogin = (int)203,
    // 初期ログインの受信
    RecvInitialLogin = (int)204,
    // ログアウトコマンド
    Finished = (int)701,
    // チャット
    Chat = (int)801,
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
    public static int ID { get { return id; }set { id = value; } }
}