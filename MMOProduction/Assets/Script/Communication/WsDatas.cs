////////////////////////////////////////////////
// パケットデータ等通信で使うデータをまとめた //
////////////////////////////////////////////////
using UnityEngine;


/// <summary>
/// パケットデータ
/// </summary>
namespace Packes
{

    //public abstract class IPacketDatas
    //{
    //    public int command;

    //    public CommandData Command { get { return (CommandData)command; } }
    //}

    // ログインシーン--------------------------------------------------------------------------------------------------
    
    ///// <summary>
    ///// command 101  ユーザー作成 (client->server)
    ///// </summary>
    //public class CreateUser : IPacketDatas
    //{
    //    public CreateUser() { command = (int)CommandData.CreateUser; }
    //    public CreateUser(string name, string password) { command = 101; user_name = name; pass = password; }
    //    public string user_name;
    //    public string pass;
    //}

    ///// <summary>
    ///// command 102  ログイン (client->server)
    ///// </summary>
    //public class Login : IPacketDatas
    //{
    //    public Login() { command = (int)CommandData.Login; }
    //    public Login(string name, string password) { command = (int)CommandData.Login; user_name = name; pass = password; }
    //    public string user_name;
    //    public string pass;

    //}
    
    ///// <summary>
    ///// command 103  確認OK (server->client)
    ///// </summary>
    //public class OKConfirmation : IPacketDatas
    //{
    //    public OKConfirmation() { command = (int)CommandData.OKConfirmation; }
    //    public int user_id;
    //}

    ///// <summary>
    ///// command 104  確認取れない (server->client) エラーコマンド
    ///// </summary>
    //public class MissingConfirmation : IPacketDatas
    //{
    //    public MissingConfirmation() { command = (int)CommandData.MissingConfirmation; }
    //}
    
    ///// <summary>
    ///// command 105　新規作成完了(server->client)
    ///// </summary>
    //public class CreateReport : IPacketDatas
    //{
    //    public CreateReport() { command = (int)CommandData.CreateReport; }
    //    public int user_id;
    //}
    
    ///// <summary>
    ///// command 106　すでに存在している(server->client)    エラーコマンド
    ///// </summary>
    //public class Existing : IPacketDatas
    //{
    //    public Existing() { command = (int)CommandData.Existing; }
    //}
    

    ///// <summary>
    ///// command 107 再接続(client->server)
    ///// </summary>
    //public class ReconnectC2S:IPacketDatas
    //{
    //    public ReconnectC2S() { command = (int)CommandData.ReconnectC2S; }
    //}
    
    ///// <summary>
    ///// command 108 再接続(server->client)
    ///// </summary>
    //public class ReconnectS2C : IPacketDatas
    //{
    //    public ReconnectS2C() { command = (int)CommandData.ReconnectS2C; }
    //}


    //// プレイシーン----------------------------------------------------------------------------------------------------

        
    ///// <summary>
    ///// command 201  位置同期(client->server)
    ///// </summary>
    //public class SendPosSync : IPacketDatas
    //{
    //    public SendPosSync() { command = (int)CommandData.SendPosSync; }
    //    public SendPosSync(PlayerData _data) {
    //        command = (int)CommandData.SendPosSync; user_id = _data.id; x = _data.x; y = _data.y; z = _data.z; dir = _data.dir;
    //    }
    //    public SendPosSync(int _id, float _x, float _y, float _z, int _dir) {
    //        command = (int)CommandData.SendPosSync; user_id = _id; command = 201; x = _x; y = _y; z = _z; dir = _dir;
    //    }
    //    public int user_id;
    //    public float x;
    //    public float y;
    //    public float z;
    //    public int dir;
    //}
    
    ///// <summary>
    ///// command 202  位置同期(server->client)
    ///// </summary>
    //public class RecvPosSync : IPacketDatas
    //{


    //    public RecvPosSync() { command = (int)CommandData.RecvPosSync; }
    //    public RecvPosSync(int _id, float _x, float _y, float _z, int _dir) {
    //        command = (int)CommandData.RecvPosSync; user_id = _id; x = _x; y = _y; z = _z; dir = _dir;
    //    }
    //    public int user_id;
    //    public float x;
    //    public float y;
    //    public float z;
    //    public int dir;
    //}
    
    ///// <summary>
    ///// command 203　初期ログイン(client->server)
    ///// </summary>
    //public class SendInitialLogin : IPacketDatas
    //{
    //    public SendInitialLogin(int _id) {
    //        command = (int)CommandData.SendInitialLogin; user_id =_id;
    //    }
    //    public int user_id;
    //}


    ///// <summary>
    ///// command 204　初期ログイン(server->client)
    ///// </summary>
    //public class RecvInitialLogin : IPacketDatas
    //{
    //    public RecvInitialLogin() { command = (int)CommandData.RecvInitialLogin; }
    //    public int user_id;
    //    public float x;
    //    public float y;
    //    public float z;
    //}

    ///// <summary>
    ///// command 205 ステータス共有(client->server)
    ///// </summary>
    //public class SendStatus : IPacketDatas{
    //    public SendStatus() { command = (int)CommandData.SendStatus; user_id = UserRecord.ID; }
    //    public SendStatus(int _hp,int _mp,int _status) {
    //        command = (int)CommandData.SendStatus; user_id = UserRecord.ID; hp = _hp; mp = _mp; status = _status;
    //    }
    //    public int user_id;
    //    public int hp;
    //    public int mp;
    //    public int status;
    //}
    
    ///// <summary>
    ///// command 206 ステータス共有(server->client)
    ///// </summary>
    //public class RecvStatus : IPacketDatas
    //{
    //    public RecvStatus() { command = (int)CommandData.RecvStatus; }
    //}
    
    ///// <summary>
    ///// command 209 セーブデータの読み込み要請(client->server)
    ///// </summary>
    //public class SaveDataRequ:IPacketDatas
    //{
    //    public SaveDataRequ() { command = (int)CommandData.SaveDataRequ; }
    //    public int user_id;
    //}
    
    ///// <summary>
    ///// command 210 セーブデータ(server->client) 
    ///// </summary>
    //public class RecvSaveData : IPacketDatas
    //{
    //    public RecvSaveData() { command = (int)CommandData.RecvSaveData; }
    //    public Weapon weapon;
    //    public Vector3 position;
    //    public int lv;
    //    public int exp;
    //}
    
    ///// <summary>
    ///// command 211 データの読み込み完了(client->server)
    ///// </summary>
    //public class DataLoadComplete:IPacketDatas
    //{
    //    public DataLoadComplete() { command = (int)CommandData.DataLoadComplete; }

    //}


    // その他データ----------------------------------------------------------------------------------------------------

        
    ///// <summary>
    ///// command 701　ログアウト時(client->server)
    ///// </summary>
    //public class Finished : IPacketDatas
    //{
    //    public Finished() { command = (int)CommandData.Finished; }
    //    public int level;
    //    public int hp;
    //    public int mp;
    //    public float x;
    //    public float y;
    //    public float z;
    //    public float exp;
    //}
    
    /// <summary>
    /// command 702 アイテム一覧(client->server)
    /// </summary>
    public class SendItemList : IPacketDatas
    {
        public SendItemList() { command = (int)CommandData.SendItemList; }

    }
    
    /// <summary>
    /// command 703 スキル一覧(client->server)
    /// </summary>
    public class SendSkillList : IPacketDatas
    {
        public SendSkillList() { command = (int)CommandData.SendSkillList; }

    }
    
    /// <summary>
    /// command 704 アイテム一覧(server->client)
    /// </summary>
    public class RecvItemList : IPacketDatas
    {
        public RecvItemList() { command = (int)CommandData.RecvItemList; }

    }

    /// <summary>
    /// command 705 スキル一覧(server->client)
    /// </summary>
    public class RecvSkillList : IPacketDatas
    {
        public RecvSkillList() { command = (int)CommandData.RecvSkillList; }

    }

    /// <summary>
    /// command 706 ログアウト完了(server->client)
    /// </summary>
    public class FinishComplete : IPacketDatas
    {
        public FinishComplete() { command = (int)CommandData.FinishComplete; }
    }


    // チャット--------------------------------------------------------------------------------------------------------
    
    /// <summary>
    /// command 801　全体チャット(client->server)
    /// </summary>
    public class SendAllChat:IPacketDatas
    {
        public SendAllChat() { command = (int)CommandData.SendAllChat; }
        public SendAllChat(string _name, string _msg)
        { command = (int)CommandData.SendAllChat; user_name = _name; message = _msg; }
        public string user_name;
        public string message;
    }
    

    /// <summary>
    /// command 802 全体チャット(server->client)
    /// </summary>
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
    
    /// <summary>
    /// command 901　重複ログイン(server->client)   エラーコマンド
    /// </summary>
    public class Duplicate:IPacketDatas
    {
        public Duplicate() { command = (int)CommandData.Duplicate; }
    }
}