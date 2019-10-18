////////////////////////////////////////////////
// パケットデータ等通信で使うデータをまとめた //
////////////////////////////////////////////////

/// <summary>
/// パケットデータ
/// </summary>
namespace Packes
{
    /// <summary>
    /// ユーザーの新規作成 command:101
    /// </summary>
    public class CreateUser : IPacketDatas{
        /// <summary>ユーザー名</summary>
        public string user_name;
        /// <summary>パスワード</summary>
        public string pass;

        /// <summary>デフォルトコンストラクタ</summary>
        public CreateUser(){
            this.command = (int)CommandData.CreateUser;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_name">ユーザー名</summary>
        /// <param name="_pass">パスワード</summary>
        public CreateUser(
            string _user_name,
            string _pass
        ){
            this.command = (int)CommandData.CreateUser;
            this.user_name = _user_name;
            this.pass = _pass;
        }
    }

    /// <summary>
    /// ユーザーのログイン command:102
    /// </summary>
    public class LoginUser : IPacketDatas{
        /// <summary>ユーザー名</summary>
        public string user_name;
        /// <summary>パスワード</summary>
        public string name;

        /// <summary>デフォルトコンストラクタ</summary>
        public LoginUser(){
            this.command = (int)CommandData.LoginUser;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_name">ユーザー名</summary>
        /// <param name="_name">パスワード</summary>
        public LoginUser(
            string _user_name,
            string _name
        ){
            this.command = (int)CommandData.LoginUser;
            this.user_name = _user_name;
            this.name = _name;
        }
    }

    /// <summary>
    /// 再接続 command:107
    /// </summary>
    public class ReConnectionCtoS : IPacketDatas{

        /// <summary>デフォルトコンストラクタ</summary>
        public ReConnectionCtoS(){
            this.command = (int)CommandData.ReConnectionCtoS;
        }
        /// <summary>フルコンストラクタ</summary>
        public ReConnectionCtoS(int _i){
            this.command = (int)CommandData.ReConnectionCtoS;
        }
    }

    // -------------------受信パケット------------------- //

    /// <summary>
    /// ログイン許可 command:103
    /// </summary>
    public class LoginOK : IPacketDatas{
        /// <summary>ユーザーID</summary>
        public int user_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public LoginOK(){
            this.command = (int)CommandData.LoginOK;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        public LoginOK(
            int _user_id
        ){
            this.command = (int)CommandData.LoginOK;
            this.user_id = _user_id;
        }
    }

    /// <summary>
    /// ログインエラー command:104
    /// </summary>
    public class LoginError : IPacketDatas{

        /// <summary>デフォルトコンストラクタ</summary>
        public LoginError(){
            this.command = (int)CommandData.LoginError;
        }
        /// <summary>フルコンストラクタ</summary>
        public LoginError(int _i)
        {
            this.command = (int)CommandData.LoginError;
        }
    }

    /// <summary>
    /// 新規作成完了 command:105
    /// </summary>
    public class CreateOK : IPacketDatas{
        /// <summary>ユーザーID</summary>
        public int user_id;

        /// <summary>デフォルトコンストラクタ</summary>
        public CreateOK(){
            this.command = (int)CommandData.CreateOK;
        }
        /// <summary>フルコンストラクタ</summary>
        /// <param name="_user_id">ユーザーID</summary>
        public CreateOK(
            int _user_id
        ){
            this.command = (int)CommandData.CreateOK;
            this.user_id = _user_id;
        }
    }

    /// <summary>
    /// 再接続 command:108
    /// </summary>
    public class ReConnectionStoC : IPacketDatas{

        /// <summary>デフォルトコンストラクタ</summary>
        public ReConnectionStoC(){
            this.command = (int)CommandData.ReConnectionStoC;
        }
        /// <summary>フルコンストラクタ</summary>
        public ReConnectionStoC(int _i){
            this.command = (int)CommandData.ReConnectionStoC;
        }
    }
}