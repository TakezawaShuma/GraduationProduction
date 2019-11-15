////////////////////////////////////////////////
// パケットデータ等通信で使うデータをまとめた //
////////////////////////////////////////////////

/// <summary>
/// パケットデータ
/// </summary>
namespace Packes
{
    /// <summary>
    /// command 801　全体チャット(client->server)
    /// </summary>
    public class SendAllChat : IPacketDatas
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
    public class RecvAllChat : IPacketDatas
    {
        public RecvAllChat() { command = (int)CommandData.RecvAllChat; }

        public RecvAllChat(string _name ,string _mag)
        {
            user_name = _name;
            message = _mag;
        }

        public string user_name;
        public string message;
    }
}
