using System.Collections.Generic;

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
    /// スキルのマスターデータを取得コール command:703
    /// </summary>
    public class LoadingSkillDataSend : IPacketDatas
    {
        public int user_id;
        public LoadingSkillDataSend()
        {
            user_id = 0;
            command = (int)CommandData.LoadingSkillDataSend;
        }
    }

    /// <summary>
    /// アクセサリーのマスターデータを取得コール command:708
    /// </summary>
    public class LoadingAccessoryMasterSend : IPacketDatas
    {
        public int user_id;
        public LoadingAccessoryMasterSend()
        {
            command = (int)CommandData.LoadingAccessoryMasterSend;
        }
        public LoadingAccessoryMasterSend(int _user_id)
        {
            command = (int)CommandData.LoadingAccessoryMasterSend;
            user_id = _user_id;
        }
    }

    /// <summary>
    /// アクセサリーのマスターデータを取得コール command:708
    /// </summary>
    public class LoadingMapMasterSend : IPacketDatas
    {
        public int user_id;
        public LoadingMapMasterSend()
        {
            command = (int)CommandData.LoadingMapMasterSend;
        }
        public LoadingMapMasterSend(int _user_id)
        {
            command = (int)CommandData.LoadingMapMasterSend;
            user_id = _user_id;
        }
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
    /// モデルの保存 command:713
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


    /// <summary>
    /// クエストマスターコール command:713
    /// </summary>
    public class QuestMasterData : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public int user_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public QuestMasterData()
        {
            this.command = (int)CommandData.QuestMasterData;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        public QuestMasterData(
            int _user_id
        )
        {
            this.command = (int)CommandData.QuestMasterData;
            this.user_id = _user_id;
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
    /// スキルのマスターデータ取得 command:705
    /// </summary>
    public class LoadingSkillMaster : IPacketDatas
    {
        public int version;
        public SkillMasterData[] skills;

        LoadingSkillMaster()
        {
            command = (int)CommandData.LoadingSkillMaster;
        }

        LoadingSkillMaster(int _version, SkillMasterData[] _skills)
        {
            command = (int)CommandData.LoadingSkillMaster;
            version = _version;
            skills = _skills;
        }
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
    /// アクセサリーのマスターデータを取得 command:709
    /// </summary>
    public class LoadingAccessoryMaster : IPacketDatas
    {
        public int version;
        public List<AccessoryMasterData> accessorys;

        LoadingAccessoryMaster()
        {
            command = (int)CommandData.LoadingAccessoryMaster;
        }

        LoadingAccessoryMaster(int _version, List<AccessoryMasterData> _accessorys)
        {
            command = (int)CommandData.LoadingAccessoryMaster;
            version = _version;
            accessorys = _accessorys;
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


    /// <summary>
    /// クエストマスター取得 command:714
    /// </summary>
    public class QuestMasterDataList : IPacketDatas
    {
        /// <summary>クエストID</summary>
        public int id;
        /// <summary>難易度</summary>
        public int difficulty;
        /// <summary>アイコンID</summary>
        public int icon_id;
        /// <summary>マップID</summary>
        public int map_id;
        /// <summary>ドロップ品一覧</summary>
        public int[] drop_ids;

        /// <summary>デフォルトコンストラクタ</summary>
        public QuestMasterDataList()
        {
            this.command = (int)CommandData.QuestMasterDataList;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_id">クエストID</summary>
        /// <param name="_difficulty">難易度</summary>
        /// <param name="_icon_id">アイコンID</summary>
        /// <param name="_map_id">マップID</summary>
        /// <param name="_drop_ids">ドロップ品一覧</summary>
        public QuestMasterDataList(
            int _id,
            int _difficulty,
            int _icon_id,
            int _map_id,
            int[] _drop_ids
        )
        {
            this.command = (int)CommandData.QuestMasterDataList;
            this.id = _id;
            this.difficulty = _difficulty;
            this.icon_id = _icon_id;
            this.map_id = _map_id;
            this.drop_ids = _drop_ids;
        }
    }

    public class LoadingMapMaster : IPacketDatas {
        public int version;
        public List<MapMasterData> maps;

        LoadingMapMaster()
        {
            command = (int)CommandData.LoadingMapMaster;
        }

        LoadingMapMaster(int _version, List<MapMasterData> _maps)
        {
            command = (int)CommandData.LoadingMapMaster;
            version = _version;
            maps = _maps;
        }
    }



    /// <summary>
    /// スキルのマスターデータ
    /// </summary>
    [System.Serializable]
    public struct SkillMasterData
    {
        public int id;                       //スキルID
        public int icon_id;              //アイコンID
        public int animation_id;             //アニメーションID
        public int effect_id;                //エフェクトID
        public string comment;               //効果説明文
        public int parent_id;                //親スキルID
        public int max_level;                //最大レベル
        public int recast_time;          //リキャスト
        public int consumption_hit_point;    //消費HP
        public int consumption_magic_point; //消費MP
        public int power;                    //威力
        public int target;                   //効果ターゲット
        public int range;                    //効果範囲
        public int target_type;          //効果範囲のタイプ

        public SkillMasterData(int _id, int _cast_time, int _recast_time, int _icon_id, int _animation_id,
                               int _consumption_hit_point, int _consumption_magic_point,
                               int _power, int _effect_id, int _target, int _range, int _target_type,
                               string _comment, int _parent_id, int _max_level)
        {
            id = _id;
            icon_id = _icon_id;
            animation_id = _animation_id;
            effect_id = _effect_id;
            comment = _comment;
            parent_id = _parent_id;
            max_level = _max_level;
            recast_time = _recast_time;
            consumption_hit_point = _consumption_hit_point;
            consumption_magic_point = _consumption_magic_point;
            power = _power;
            target = _target;
            target_type = _target_type;
            range = _range;
        }
    }

    /// <summary>
    /// アクセサリーのマスターデータ
    /// </summary>
    [System.Serializable]
    public struct AccessoryMasterData
    {
        public int id;
        public string name;
        public int level;
        public string comment;

        public int hp;
        public int mp;
        public int str;
        public int vit;
        public int mmd;
        public int dex;
        public int agi;

        public string image;

        AccessoryMasterData(
            int _id,
            string _name,
            int _level,
            string _comment,
            int _hp,
            int _mp,
            int _str,
            int _vit,
            int _mmd,
            int _dex,
            int _agi,
            string _image
            )
        {
            id = _id;
            name = _name;
            level = _level;
            comment = _comment;
            hp = _hp;
            mp = _mp;
            str = _str;
            vit = _vit;
            mmd = _mmd;
            dex = _dex;
            agi = _agi;
            image = _image;
        }
    }

    [System.Serializable]
    public struct MapMasterData {
        public int id;
        public int x;
        public int y;
        public int z;
        public int dir;

        MapMasterData(
            int _id,
            int _x,
            int _y,
            int _z,
            int _dir
            ) 
        {
            id = _id;
            x = _x;
            y = _y;
            z = _z;
            dir = _dir;
        }
    }
}
