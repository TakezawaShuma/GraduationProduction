////////////////////////////////////////////////
// パケットデータ等通信で使うデータをまとめた //
////////////////////////////////////////////////
/// <summary>
/// パケットデータ
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
    /// 初回ログイン command:203
    /// </summary>
    public class InitLogin : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public int user_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public InitLogin()
        {
            this.command = (int)CommandData.InitLogin;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        public InitLogin(
            int _user_id
        )
        {
            this.command = (int)CommandData.InitLogin;
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
        /// <summary>最大ヒットポイント</summary>
        public int max_hp;
        /// <summary>ヒットポイント</summary>
        public int hp;
        /// <summary>最大マジックポイント</summary>
        public int max_mp;
        /// <summary>マジックポイント</summary>
        public int mp;
        /// <summary>異常状態</summary>
        public int status;

        /// <summary>デフォルトコンストラクタ</summary>
        public StatusCtoS()
        {
            this.command = (int)CommandData.StatusCtoS;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        /// <param name="_max_hp">最大ヒットポイント</summary>
        /// <param name="_hp">ヒットポイント</summary>
        /// <param name="_max_mp">最大マジックポイント</summary>
        /// <param name="_mp">マジックポイント</summary>
        /// <param name="_status">異常状態</summary>
        public StatusCtoS(
            int _user_id,
            int _max_hp,
            int _hp,
            int _max_mp,
            int _mp,
            int _status
        )
        {
            this.command = (int)CommandData.StatusCtoS;
            this.user_id = _user_id;
            this.max_hp = _max_hp;
            this.hp = _hp;
            this.max_mp = _max_mp;
            this.mp = _mp;
            this.status = _status;
        }
    }

    /// <summary>
    /// セーブデータの読み込み要請 command:209
    /// </summary>
    public class DataLoading : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public int user_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public DataLoading()
        {
            this.command = (int)CommandData.DataLoading;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        public DataLoading(
            int _user_id
        )
        {
            this.command = (int)CommandData.DataLoading;
            this.user_id = _user_id;
        }
    }

    /// <summary>
    /// データの読み込み終了 command:211
    /// </summary>
    public class LoadingFinish : IPacketDatas
    {

        /// <summary>デフォルトコンストラクタ</summary>
        public LoadingFinish()
        {
            this.command = (int)CommandData.LoadingFinish;
        }
        /// <summary>フルコンストラクタ</summary>
        public LoadingFinish(int _i)
        {
            this.command = (int)CommandData.LoadingFinish;
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
    /// 初回ログイン command:204
    /// </summary>
    public class InitLoginStoC : IPacketDatas
    {
        /// <summary>ユーザーのID</summary>
        public int user_id;
        /// <summary>デフォルトコンストラクタ</summary>
        public InitLoginStoC()
        {
            this.command = (int)CommandData.InitLoginStoC;
        }
        public InitLoginStoC(
            int _user_id
        )
        {
            this.command = (int)CommandData.InitLoginStoC;
            this.user_id = _user_id;
        }
    }


    /// <summary>
    /// 状態送信 command:206
    /// </summary>
    public class StatusStoC : IPacketDatas
    {
        /// <summary>ユーザーID</summary>
        public int user_id;
        /// <summary>最大ヒットポイント</summary>
        public int max_hp;
        /// <summary>今のヒットポイント</summary>
        public int hp;
        /// <summary>最大マジックポイント</summary>
        public int max_mp;
        /// <summary>マジックポイント</summary>
        public int mp;
        /// <summary>状態</summary>
        public int status;

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
        public StatusStoC(
            int _user_id,
            int _max_hp,
            int _hp,
            int _max_mp,
            int _mp,
            int _status
        )
        {
            this.command = (int)CommandData.StatusStoC;
            this.user_id = _user_id;
            this.max_hp = _max_hp;
            this.hp = _hp;
            this.max_mp = _max_mp;
            this.mp = _mp;
            this.status = _status;
        }
    }

    /// <summary>
    /// セーブデータ command:210
    /// </summary>
    public class LoadSaveData : IPacketDatas
    {

        /// <summary>デフォルトコンストラクタ</summary>
        public LoadSaveData()
        {
            this.command = (int)CommandData.LoadSaveData;
        }
        /// <summary>フルコンストラクタ</summary>
        public LoadSaveData(int _i)
        {
            this.command = (int)CommandData.LoadSaveData;
        }
    }
}
