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
	CreateUser = 101,
	/// <summary>ユーザーのログイン</summary>
	LoginUser = 102,
	/// <summary>ログイン許可</summary>
	LoginOK = 103,
	/// <summary>ログインエラー</summary>
	LoginError = 104,
	/// <summary>新規作成完了</summary>
	CreateOK = 105,
	/// <summary>再接続要請</summary>
	ReConnectionCtoS = 107,
	/// <summary>再接続確認</summary>
	ReConnectionStoC = 108,


	/// プレイ時使用コマンド
	/// <summary>移動</summary>
	TranslationCtoS = 201,
	/// <summary>移動</summary>
	TranslationStoC = 202,
	/// <summary>エネミー情報要請</summary>
	GetEnemysDataCtoS = 203,
	/// <summary>エネミー情報受信</summary>
	GetEnemyDataStoC = 204,
	/// <summary>状態送信</summary>
	StatusCtoS = 205,
	/// <summary>状態受信</summary>
	StatusStoC = 206,
	/// <summary>アクセサリー装備変更</summary>
	AccessoryChange = 207,


	/// <summary>セーブデータの読み込み要請</summary>
	SaveLoadCtoS = 211,
	/// <summary>セーブデータの読み込み</summary>
	SaveLoadStoC = 212,
	/// <summary>セーブデータの読み込み完了</summary>
	LoadingOK = 213,
	/// <summary>プレイシーンにいる他ユーザーの一覧</summary>
	OtherPlayerList = 214,
	/// <summary>他ユーザーの途中ログイン</summary>
	NewOtherUser = 215,


	/// <summary>戦闘処理送信</summary>
	Attack = 220,
	/// <summary>エネミーが生きている時</summary>
	EnemyAliveStoC = 221,
	/// <summary>エネミーが死んでいる時</summary>
	EnemyDieStoC = 222,
	/// <summary>他プレイヤーが攻撃した </summary>
	OtherPlayerUseSkill = 224,
	/// <summary>敵のスキル使用申請</summary>
	EnemyUseSkillRequest = 225,
	/// <summary>敵のスキル使用</summary>
	EnemyUseSkill = 226,
	/// <summary>敵の攻撃</summary>
	EnemyAttackResult = 227,
	/// <summary>敵の攻撃がプレイヤーにヒットした</summary>
	UserHit = 228,

	/// <summary>マップ移動コール(クエスト受注やリタイアに紐づく)</summary>
	MoveingMap = 251,
	/// <summary>マップ移動完了(クエスト受注やリタイアに紐づく)</summary>
	MoveingMapOk = 252,
	/// <summary>報酬選択</summary>
	SelectReward = 253,
	/// <summary>報酬選択完了</summary>
	SelectRewardOk = 254,
	/// <summary>永久インべの取得コール</summary>
	GetInventory = 255,
	/// <summary>永久インべの取得</summary>
	InventoryList = 256,


	/// <summary>ドロップインベの取得コール</summary>
	GetDropInventory = 291,
	/// <summary>ドロップインベの取得</summary>
	DropInventoryList = 292,

	/// その他

	/// <summary>ログアウト要請</summary>
	LogoutCtoS = 701,
	/// <summary> ログアウト完了 </summary>
	FinishComplete = 706,
	/// <summary>他ユーザーのログアウト</summary>
	LogoutStoC = 707,
	/// <summary> アイテム一覧送信 </summary>
	SendItemList = 702,
	/// <summary> スキル一覧送信 </summary>
	LoadingSkillDataSend = 703,
	/// <summary> アイテム一覧受信 </summary>
	RecvItemList = 704,
	/// <summary> スキル一覧受信 </summary>
	LoadingSkillMaster = 705,
	/// <summary> アクセサリーのマスターデータをコール </summary>
	LoadingAccessoryMasterSend = 708,
	/// <summary> アクセサリーのマスターデータを取得 </summary>
	LoadingAccessoryMaster = 709,

	/// <summary>キャラクターの詳細取得</summary>
	FindOfPlayerCtoS = 711,
	/// <summary>キャラクターの詳細</summary>
	FindOfPlayerStoC = 712,
	/// <summary>モデルの保存</summary>
	SaveModelType = 713,

	/// <summary>クエストマスターコール</summary>
	QuestMasterData = 714,
	/// <summary>クエストマスター取得</summary>
	QuestMasterDataList = 715,

    LoadingMapMasterSend = 716,
    LoadingMapMaster = 717,




	// チャット
	/// <summary> 全体チャット送信 </summary>
	SendAllChat = 801,
	/// <summary> 全体チャット受信 </summary>
	RecvAllChat = 802,

	// エラーコマンド
	/// <summary> 重複ログイン報告 </summary>
	Duplicate = 901,

}