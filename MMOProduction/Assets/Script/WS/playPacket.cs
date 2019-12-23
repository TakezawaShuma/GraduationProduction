using System.Collections.Generic;

/// <summary>
/// パケットデータ　200番台(プレイシーン用)
/// </summary>
namespace Packes
{
    // -------------------送信パケット------------------- //

    /// <summary>
    /// ユーザーの移動 command:201
    /// </summary>
    public class TranslationCtoS : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public int user_id;
        /// <summary>位置X</summary>
        public float x;
        /// <summary>位置Y</summary>
        public float y;
        /// <summary>位置Z</summary>
        public float z;
        /// <summary>方向</summary>
        public float dir;

        /// <summary>デフォルトコンストラクタ</summary>
        public TranslationCtoS()
        {
            this.command = (int)CommandData.TranslationCtoS;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        /// <param name="_x">位置X</summary>
        /// <param name="_y">位置Y</summary>
        /// <param name="_z">位置Z</summary>
        /// <param name="_dir">方向</summary>
        public TranslationCtoS(
            int _user_id,
            float _x,
            float _y,
            float _z,
            float _dir
        )
        {
            this.command = (int)CommandData.TranslationCtoS;
            this.user_id = _user_id;
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.dir = _dir;
        }
    }

    /// <summary>
    /// 敵の一覧取得 command:203
    /// </summary>
    public class GetEnemysDataCtoS : IPacketDatas
    {
        /// <summary>マップのID</summary>
        public int map_id;
        /// <summary> ユーザーID</summary>
        public int user_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public GetEnemysDataCtoS()
        {
            this.command = (int)CommandData.GetEnemysDataCtoS;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_map_id">マップのID</summary>
        public GetEnemysDataCtoS(
            int _map_id,
            int _user_id
        )
        {
            this.command = (int)CommandData.GetEnemysDataCtoS;
            this.map_id = _map_id;
            this.user_id = _user_id;
        }
    }

    /// <summary>
    /// 状態送信 command:205
    /// </summary>
    public class StatusCtoS : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public int user_id;
        /// <summary>情報が欲しい相手のID</summary>
        public int target_id;
        /// <summary>どの情報が欲しいか</summary>
        public int type;


        /// <summary>デフォルトコンストラクタ</summary>
        public StatusCtoS()
        {
            this.command = (int)CommandData.StatusCtoS;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        /// <param name="_target_id">情報が欲しい相手のID</summary>
        /// <param name="_type">どの情報が欲しいか</summary>
        public StatusCtoS(
            int _user_id,
            int _target_id,
            int _type
        )
        {
            this.command = (int)CommandData.StatusCtoS;
            this.user_id = _user_id;
            this.target_id = _target_id;
            this.type = _type;
        }
    }

    /// <summary>
    /// セーブデータの読み込み要請 command:211
    /// </summary>
    public class SaveLoadCtoS : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public int user_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public SaveLoadCtoS()
        {
            this.command = (int)CommandData.SaveLoadCtoS;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        public SaveLoadCtoS(
            int _user_id
        )
        {
            this.command = (int)CommandData.SaveLoadCtoS;
            this.user_id = _user_id;
        }
    }

    /// <summary>
    /// セーブデータの読み込み完了 command:213
    /// </summary>
    public class LoadingOK : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public int user_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public LoadingOK() { this.command = (int)CommandData.LoadingOK; }

        /// <summary>フルコンストラクタ</summary>
        public LoadingOK(int _id)
        {
            this.command = (int)CommandData.LoadingOK;
            this.user_id = _id;
        }
    }




    /// <summary>
    /// 攻撃 P->E CtoS command:220
    /// </summary>
    public class Attack : IPacketDatas
    {
        /// <summary>敵のID</summary>
        public int enemy_id;
        /// <summary>プレイヤーID</summary>
        public int user_id;
        /// <summary>スキルID</summary>
        public int skill_id;
        /// <summary>マップのID</summary>
        public int map_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public Attack()
        {
            this.command = (int)CommandData.Attack;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_enemy_id">敵のID</summary>
        /// <param name="_user_id">プレイヤーID</summary>
        /// <param name="_skill_id">スキルID</summary>
        /// <param name="_map_id">マップのID</summary>
        public Attack(
            int _enemy_id,
            int _user_id,
            int _skill_id,
            int _map_id
        )
        {
            this.command = (int)CommandData.Attack;
            this.enemy_id = _enemy_id;
            this.user_id = _user_id;
            this.skill_id = _skill_id;
            this.map_id = _map_id;
        }
    }

    /// <summary>
    /// 敵の攻撃がプレイヤーにヒットした command:228
    /// </summary>
    public class UserHit : IPacketDatas
    {
        /// <summary>ユーザーのID</summary>
        public int user_id;
        /// <summary>敵のID</summary>
        public int enemy_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public UserHit()
        {
            this.command = (int)CommandData.UserHit;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_user_id">ユーザーのID</summary>
        /// <param name="_enemy_id">敵のID</summary>
        public UserHit(
            int _user_id,
            int _enemy_id
        )
        {
            this.command = (int)CommandData.UserHit;
            this.user_id = _user_id;
            this.enemy_id = _enemy_id;
        }
    }


    // -------------------受信パケット------------------- //

    /// <summary>
    /// プレイヤーの移動 command:202
    /// </summary>
    public class TranslationStoC : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public int user_id;
        /// <summary>位置X</summary>
        public float x;
        /// <summary>位置Y</summary>
        public float y;
        /// <summary>位置Z</summary>
        public float z;
        /// <summary>方向</summary>
        public float dir;
        /// <summary>名前</summary>
        public string name;

        /// <summary>デフォルトコンストラクタ</summary>
        public TranslationStoC()
        {
            this.command = (int)CommandData.TranslationStoC;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        /// <param name="_x">位置X</summary>
        /// <param name="_y">位置Y</summary>
        /// <param name="_z">位置Z</summary>
        /// <param name="_dir">方向</summary>
        public TranslationStoC(
            int _user_id,
            float _x,
            float _y,
            float _z,
            float _dir
        )
        {
            this.command = (int)CommandData.TranslationStoC;
            this.user_id = _user_id;
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.dir = _dir;
        }
    }

    /// <summary>
    /// 敵の一覧取得 command:204
    /// </summary>
    public class GetEnemyDataStoC : IPacketDatas
    {
        public List<EnemyReceiveData> enemys;

        public GetEnemyDataStoC()
        {
            enemys = new List<EnemyReceiveData>();
            this.command = (int)CommandData.GetEnemyDataStoC;
        }
    }

    /// <summary>
    /// 状態受信 command:206
    /// </summary>
    public class StatusStoC : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public List<CharctorSatusData> status;

        /// <summary>デフォルトコンストラクタ</summary>
        public StatusStoC()
        {
            this.command = (int)CommandData.StatusStoC;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        /// <param name="_max_hp">最大ヒットポイント</summary>
        /// <param name="_hp">今のヒットポイント</summary>
        /// <param name="_max_mp">最大マジックポイント</summary>
        /// <param name="_mp">マジックポイント</summary>
        /// <param name="_status">状態</summary>
        public StatusStoC(int _i
        )
        {
            this.command = (int)CommandData.StatusStoC;
        }
    }

    /// <summary>
    /// セーブデータの読み込み command:212
    /// </summary>
    public class SaveLoadStoC : IPacketDatas
    {
        /// <summary>位置X</summary>
        public float x;
        /// <summary>位置Y</summary>
        public float y;
        /// <summary>位置Z</summary>
        public float z;
        /// <summary>モデルID</summary>
        public int model_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public SaveLoadStoC()
        {
            this.command = (int)CommandData.SaveLoadStoC;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_x">位置X</summary>
        /// <param name="_y">位置Y</summary>
        /// <param name="_z">位置Z</summary>
        /// <param name="_model_id">モデルID</summary>
        public SaveLoadStoC(
            float _x,
            float _y,
            float _z,
            int _model_id
        )
        {
            this.command = (int)CommandData.SaveLoadStoC;
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.model_id = _model_id;
        }
    }

    /// <summary>
    /// プレイシーンにいる他ユーザーの一覧 command:214
    /// </summary>
    public class OtherPlayerList : IPacketDatas
    {
        public List<OtherPlayersData> players = new List<OtherPlayersData>();
        /// <summary>デフォルトコンストラクタ</summary>
        public OtherPlayerList()
        {
            this.command = (int)CommandData.OtherPlayerList;
        }
    }

    /// <summary>
    /// 他ユーザーの途中ログイン command:215
    /// </summary>
    public class NewOtherUser : IPacketDatas
    {
        /// <summary>ユーザーのID</summary>
        public int user_id;
        /// <summary>位置X</summary>
        public float x;
        /// <summary>位置Y</summary>
        public float y;
        /// <summary>位置Z</summary>
        public float z;
        /// <summary>キャラクター名</summary>
        public string name;
        /// <summary>モデルID</summary>
        public int model_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public NewOtherUser()
        {
            this.command = (int)CommandData.NewOtherUser;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_user_id">ユーザーのID</summary>
        /// <param name="_x">位置X</summary>
        /// <param name="_y">位置Y</summary>
        /// <param name="_z">位置Z</summary>
        /// <param name="_name">キャラクター名</summary>
        public NewOtherUser(
            int _user_id,
            float _x,
            float _y,
            float _z,
            string _name,
            int _model_id
        )
        {
            this.command = (int)CommandData.NewOtherUser;
            this.user_id = _user_id;
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.name = _name;
            this.model_id = _model_id;
        }
    }
    

    /// <summary>
    /// 判定後生きている command:221
    /// </summary>
    public class EnemyAliveStoC : IPacketDatas
    {
        /// <summary>敵のID</summary>
        public int unique_id;
        /// <summary>ヒットポイント</summary>
        public int hp;
        /// <summary>状態</summary>
        public int status;

        /// <summary>デフォルトコンストラクタ</summary>
        public EnemyAliveStoC()
        {
            this.command = (int)CommandData.EnemyAliveStoC;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_unique_id">敵のID</summary>
        /// <param name="_hp">ヒットポイント</summary>
        /// <param name="_status">状態</summary>
        public EnemyAliveStoC(
            int _unique_id,
            int _hp,
            int _status
        )
        {
            this.command = (int)CommandData.EnemyAliveStoC;
            this.unique_id = _unique_id;
            this.hp = _hp;
            this.status = _status;
        }
    }

    /// <summary>
    /// 判定後死亡 command:222
    /// </summary>
    public class EnemyDieStoC : IPacketDatas
    {
        /// <summary>ドロップ品のID</summary>
        public int drop;
        /// <summary>敵のID</summary>
        public int unique_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public EnemyDieStoC()
        {
            this.command = (int)CommandData.EnemyDieStoC;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_drop">ドロップ品のID</summary>
        public EnemyDieStoC(
            int _drop,
            int _unique_id
        )
        {
            this.command = (int)CommandData.EnemyDieStoC;
            this.drop = _drop;
            this.unique_id = _unique_id;
        }
    }


    /// <summary>
    /// 他のプレイヤーがスキルを使用したとき command = 224
    /// </summary>
    public class OtherPlayerUseSkill : IPacketDatas
    {
        /// <summary>使用者ID</summary>
        public int user_id;
        /// <summary>スキルID</summary>
        public int skill_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public OtherPlayerUseSkill()
        {
            this.command = (int)CommandData.OtherPlayerUseSkill;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_user_id">使用者ID</summary>
        /// <param name="_skill_id">スキルID</summary>
        public OtherPlayerUseSkill(
            int _user_id,
            int _skill_id
        )
        {
            this.command = (int)CommandData.OtherPlayerUseSkill;
            this.user_id = _user_id;
            this.skill_id = _skill_id;
        }
    }


    /// <summary>
    /// 敵のスキル使用申請 command:225
    /// </summary>
    public class EnemyUseSkillRequest : IPacketDatas
    {
        /// <summary>スキルのID</summary>
        public int skill_id;
        /// <summary>敵のID</summary>
        public int enemy_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public EnemyUseSkillRequest()
        {
            this.command = (int)CommandData.EnemyUseSkillRequest;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_skill_id">スキルのID</summary>
        /// <param name="_enemy_id">敵のID</summary>
        public EnemyUseSkillRequest(
            int _skill_id,
            int _enemy_id
        )
        {
            this.command = (int)CommandData.EnemyUseSkillRequest;
            this.skill_id = _skill_id;
            this.enemy_id = _enemy_id;
        }
    }

    /// <summary>
    /// 敵のスキル使用 command:226
    /// </summary>
    public class EnemyUseSkill : IPacketDatas
    {
        /// <summary>スキルのID</summary>
        public int skill_id;
        /// <summary>敵のID</summary>
        public int enemy_id;
        /// <summary>ターゲットのID</summary>
        public int target_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public EnemyUseSkill()
        {
            this.command = (int)CommandData.EnemyUseSkill;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_skill_id">スキルのID</summary>
        /// <param name="_enemy_id">敵のID</summary>
        public EnemyUseSkill(
            int _skill_id,
            int _enemy_id,
            int _target_id
        )
        {
            this.command = (int)CommandData.EnemyUseSkill;
            this.skill_id = _skill_id;
            this.enemy_id = _enemy_id;
            this.target_id = _target_id;
        }
    }

    /// <summary>
    /// 敵の攻撃の結果 command:227
    /// </summary>
    public class EnemyAttackResult : IPacketDatas
    {
        /// <summary>ユーザーのID</summary>
        public int user_id;
        /// <summary>ヒットポイント</summary>
        public int hp;

        /// <summary>デフォルトコンストラクタ</summary>
        public EnemyAttackResult()
        {
            this.command = (int)CommandData.EnemyAttackResult;
        }
        /// <summary>コンストラクタ</summary>
        /// <param name="_user_id">ユーザーのID</summary>
        /// <param name="_hp">ヒットポイント</summary>
        public EnemyAttackResult(
            int _user_id,
            int _hp
        )
        {
            this.command = (int)CommandData.EnemyAttackResult;
            this.user_id = _user_id;
            this.hp = _hp;
        }
    }


    /// <summary> 他プレイヤーの作成用データ </summary>
    [System.Serializable]
    public struct OtherPlayersData
    {  
        /// <summary>ユーザーID</summary>
        public int user_id;
        /// <summary>位置X</summary>
        public float x;
        /// <summary>位置Y</summary>
        public float y;
        /// <summary>位置Z</summary>
        public float z;
        /// <summary>モデルのID</summary>
        public int model_id;
        /// <summary>名前</summary>
        public string name;

        public OtherPlayersData(
            int _user_id,
            float _x,
            float _y,
            float _z,
            int _model_id,
        string _name)
        {
            this.user_id = _user_id;
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.model_id = _model_id;
            this.name = _name;
        }
    }
    


    enum OBJECT_TYPE
    {
        PLAYER,
        OTHER_PLAYER,
        ENEMY,
    }

    /// <summary> 敵の受信用データ</summary>
    [System.Serializable]
    public struct EnemyReceiveData
    {
        public int unique_id;
        public int master_id;
        public float x;
        public float y;
        public float z;
        public float dir;
        public int anime_id;
        public int hp;

        /// <summary>
        /// 敵のデータ
        /// </summary>
        /// <param name="_unique_id">敵の個人ID</param>
        /// <param name="_master_id">マスターID</param>
        /// <param name="_x">位置X</param>
        /// <param name="_y">位置Y</param>
        /// <param name="_z">位置Z</param>
        /// <param name="_dir">方向</param>
        /// <param name="_anime_id">現在のアニメーションID</param>
        /// <param name="_hp">ヒットポイント</param>
        public EnemyReceiveData(
            int _unique_id,
            int _master_id,
            float _x,
            float _y,
            float _z,
            float _dir,
            int _anime_id,
            int _hp
        )
        {
            this.unique_id = _unique_id;
            this.master_id = _master_id;
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.dir = _dir;
            this.anime_id = _anime_id;
            this.hp = _hp;
        }
    }


    /// <summary>
    /// キャラクターのステータス共有データ
    /// </summary>
    [System.Serializable]
    public struct CharctorSatusData
    {
        public int charctor_id;
        public int max_hp;
        public int hp;
        public int max_mp;
        public int mp;
        public int status;
        public CharctorSatusData(int _charctor_id, int _max_hp, int _hp, int _max_mp, int _mp, int _status)
        {
            this.charctor_id = _charctor_id;
            this.max_hp = _max_hp;
            this.hp = _hp;
            this.max_mp = _max_mp;
            this.mp = _mp;
            this.status = _status;
        }
    }

}
