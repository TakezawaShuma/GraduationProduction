//////////////////////////////////////////
// パケットデータの基底とコマンドデータ //
//////////////////////////////////////////
/// <summary>
/// パケットデータ
/// </summary>
namespace Packes
{
    /// <summary>
    /// パケットデータインターフェース
    /// </summary>
    public abstract class IPacketDatas
    {
        /// <summary>コマンド</summary>
        public int command;
        /// <summary>コマンド</summary>
        public CommandData Command { get { return (CommandData)(this.command); } }
    }
}

/// <summary>
/// コマンド一覧
/// </summary>
public enum CommandData
{
    /// ログイン時使用コマンド
    /// <summary>ユーザーの新規作成</summary>
    CreateUser = (int)101,
    /// <summary>ユーザーのログイン</summary>
    LoginUser = (int)102,
    /// <summary>ログイン許可</summary>
    LoginOK = (int)103,
    /// <summary>ログインエラー</summary>
    LoginError = (int)104,
    /// <summary>新規作成完了</summary>
    CreateOK = (int)105,
    /// <summary>再接続要請</summary>
    ReConnectionCtoS = (int)107,
    /// <summary>再接続確認</summary>
    ReConnectionStoC = (int)108,


    /// プレイ時使用コマンド
    /// <summary>移動</summary>
    TranslationCtoS = (int)201,
    /// <summary>移動</summary>
    TranslationStoC = (int)202,
    /// <summary>初回ログイン送信</summary>
    GetEnemysDataCtoS = (int)203,
    /// <summary>初回ログイン受信</summary>
    GetEnemyDataStoC = (int)204,
    /// <summary>状態送信</summary>
    StatusCtoS = (int)205,
    /// <summary>状態受信</summary>
    StatusStoC = (int)206,
    /// <summary>セーブデータの読み込み</summary>
    DataLoading = (int)209,
    /// <summary>セーブデータ</summary>
    LoadSaveData = (int)210,
    /// <summary>データの読み込み終了報告</summary>
    LoadingFinishCtoS = (int)211,
    /// <summary>データの読み込み終了報告</summary>
    LoadingFinishStoC = (int)212,

    /// <summary>戦闘処理送信</summary>
    Attack = (int)220,
    /// <summary>エネミーが生きている時</summary>
    EnemyAliveStoC = (int)221,
    /// <summary>エネミーが死んでいる時</summary>
    EnemyDieStoC = (int)222,



    /// その他

    /// <summary>ログアウト要請</summary>
    LogoutCtoS = (int)701,
    /// <summary> アイテム一覧送信 </summary>
    SendItemList = (int)702,
    /// <summary> スキル一覧送信 </summary>
    SendSkillList = (int)703,
    /// <summary> アイテム一覧受信 </summary>
    RecvItemList = (int)704,
    /// <summary> スキル一覧受信 </summary>
    RecvSkillList = (int)705,
    /// <summary> ログアウト完了 </summary>
    FinishComplete = (int)706,

    /// <summary>他ユーザーのログアウト</summary>
    LogoutStoC = (int)707,

    // チャット
    /// <summary> 全体チャット送信 </summary>
    SendAllChat = (int)801,
    /// <summary> 全体チャット受信 </summary>
    RecvAllChat = (int)802,

    // エラーコマンド
    /// <summary> 重複ログイン報告 </summary>
    Duplicate = 901,
}