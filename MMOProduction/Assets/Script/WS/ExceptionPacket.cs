/// <summary>
/// パケットデータ 700番台(例外処理用)
/// </summary>
namespace Packes
{
    // -------------------送信パケット------------------- //


    /// <summary>
    /// ログアウト command:701
    /// </summary>
    public class LogoutCtoS : IPacketDatas
    {
        /// <summary>ユーザーのID</summary>
        public int user_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public LogoutCtoS()
        {
            this.command = (int)CommandData.LogoutCtoS;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_id">ユーザーのID</summary>
        public LogoutCtoS(int _user_id)
        {
            this.command = (int)CommandData.LogoutCtoS;
            this.user_id = _user_id;
        }
    }


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
    /// キャラクターの詳細取得 command:711
    /// </summary>
    public class FindOfPlayerCtoS : IPacketDatas
    {
        /// <summary>自分のID</summary>
        public int user_id;
        /// <summary>調べたい人のID</summary>
        public int target_id;
        /// <summary>マップID</summary>
        public int map_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public FindOfPlayerCtoS()
        {
            this.command = (int)CommandData.FindOfPlayerCtoS;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        /// <param name="_map_id">マップID</summary>
        public FindOfPlayerCtoS(
            int _user_id,
            int _target_id,
            int _map_id
        )
        {
            this.command = (int)CommandData.FindOfPlayerCtoS;
            this.user_id = _user_id;
            this.target_id = _target_id;
            this.map_id = _map_id;
        }
    }



    /// <summary>
    /// モデルの保存
    /// </summary>
    public class SaveModelType : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public int user_id;
        /// <summary>モデルID</summary>
        public int model_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public SaveModelType()
        {
            this.command = (int)CommandData.SaveModelType;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        /// <param name="_model_id">モデルID</summary>
        public SaveModelType(
            int _user_id,
            int _model_id
        )
        {
            this.command = (int)CommandData.SaveModelType;
            this.user_id = _user_id;
            this.model_id = _model_id;
        }
    }


    // -------------------受信パケット------------------- //



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
    /// ログアウト完了(server->client) command 706
    /// </summary>
    public class FinishComplete : IPacketDatas
    {
        public FinishComplete() { command = (int)CommandData.FinishComplete; }
    }



    /// <summary>
    /// ログアウトした人の報告 command:707
    /// </summary>
    public class LogoutStoC : IPacketDatas
    {
        public int user_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public LogoutStoC()
        {
            this.command = (int)CommandData.LogoutStoC;
        }
        public LogoutStoC(
            int _user_id
        )
        {
            this.user_id = _user_id;
        }
    }


    /// <summary>
    /// キャラクターの詳細 command:712
    /// </summary>
    public class FindOfPlayerStoC : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public int user_id;
        /// <summary>位置X</summary>
        public float x;
        /// <summary>位置Y</summary>
        public float y;
        /// <summary>位置Z</summary>
        public float z;
        /// <summary>モデルID</summary>
        public int model_id;
        /// <summary>名前</summary>
        public string name;

        /// <summary>デフォルトコンストラクタ</summary>
        public FindOfPlayerStoC()
        {
            this.command = (int)CommandData.FindOfPlayerStoC;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        /// <param name="_x">位置X</summary>
        /// <param name="_y">位置Y</summary>
        /// <param name="_z">位置Z</summary>
        /// <param name="_model_id">モデルID</summary>
        /// <param name="_name">名前</summary>
        public FindOfPlayerStoC(
            int _user_id,
            float _x,
            float _y,
            float _z,
            int _model_id,
            string _name
        )
        {
            this.command = (int)CommandData.FindOfPlayerStoC;
            this.user_id = _user_id;
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.model_id = _model_id;
            this.name = _name;
        }
    }
}
