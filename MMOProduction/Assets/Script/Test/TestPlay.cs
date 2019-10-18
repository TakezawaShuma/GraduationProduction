using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlay : MonoBehaviour
{ 
    class TestBox
    {
        public float x;
        public float y;
        public float z;
        public int hp;
        public int mp;
        public int dir;
        public TestBox() { }
        public TestBox(int hp,int mp) { this.hp = hp; this.mp = mp; }
    }
    TestBox testbox=new TestBox(500,500);

    Player player = new Player();

    Dictionary<int, OtherPlayers> other_players = new Dictionary<int, OtherPlayers>();
    PlayersGenerator generator = new PlayersGenerator();

    // 対応したシーンのConnect○○を作ってね！
    //WS.WsPlay ws_play = new WS.WsPlay();

    void Start()
    {
        player = null;
        // Startに必ず入れる事！！
        // 通信関係の初期処理
        //ws_play.ConnectionStart(Receive);
    }

    void Update()
    {
        if (Input.GetKeyDown("]"))
        {
            //ws_play.SendPosData( player.transform.position.x, player.transform.position.y, player.transform.position.z, (int)player.transform.rotation.y);
            Debug.Log(other_players.Count);
        }
    }

    private void OnDestroy()
    {
        //ws_play.Destroy();
    }

    private void Move()
    {
        if(Input.GetKey(KeyCode.X)){ testbox.x++; }
        if (Input.GetKey(KeyCode.Y)) { testbox.y++; }
        if (Input.GetKey(KeyCode.Z)) { testbox.z++; }
        if (Input.GetKey(KeyCode.M)) { testbox.mp--; }
        if (Input.GetKey(KeyCode.H)) { testbox.hp--; }
        if (Input.GetKey(KeyCode.D)) { testbox.dir--; }
    }

    void Receive(PlayerData _data)
    {
        // 自分のIDだったら
        if (_data.id == Retention.ID)
        {
            Debug.Log("自分のデータの受信");
            if (player == null)
            {
                Debug.Log("プレイヤーのnull確認");
                player = generator.GenetatePlayer();
            }
        }
        // 自分以外のIDなら
        else
        {
            // すでに存在している
            if (other_players.ContainsKey(_data.id))
            {
                //other_players[_data.id].UpdataData(_data.HP, _data.MP, _data.X, _data.Y, _data.Z, _data.Direction);

            }
            // 存在していないプレイヤー
            else
            {
                other_players.Add(_data.id, generator.GenerateOtherPlayer());
                Debug.Log("ID : " + _data.id + " Data : " + other_players[_data.id]);
            }
        }
    }
}
