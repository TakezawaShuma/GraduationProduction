////////////////////////////////////////////////
// パケットデータ等通信で使うデータをまとめた //
////////////////////////////////////////////////
using UnityEngine;


/// <summary>
/// パケットデータ　900番台(ゲーム外でのエラー処理)
/// </summary>
namespace Packes
{     
    /// <summary>
    /// command 901　重複ログイン(server->client)   エラーコマンド
    /// </summary>
    public class Duplicate:IPacketDatas
    {
        public Duplicate() { command = (int)CommandData.Duplicate; }
    }
}