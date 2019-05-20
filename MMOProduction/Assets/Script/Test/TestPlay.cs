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

    PlayerController player = new PlayerController();

    Dictionary<int, OtherPlayers> other_players = new Dictionary<int, OtherPlayers>();
    PlayersGenerator generator = new PlayersGenerator();

    // 対応したシーンのConnect○○を作ってね！
    Connect.ConnectPlay connect_play = new Connect.ConnectPlay();

    void Start()
    {
        player = null;
        // Startに必ず入れる事！！
        // 通信関係の初期処理
        connect_play.ConnectionStart(Receive);


    }

    void Update()
    {
        if (Input.GetKeyDown("]"))
        {
            connect_play.SendPosData(500, 200, player.transform.position.x, player.transform.position.y, player.transform.position.z, (int)player.transform.rotation.y);
            Debug.Log(other_players.Count);
        }
    }

    private void OnDestroy()
    {
        connect_play.Destroy();
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

    void Receive(int id, int hp, int mp, float x, float y, float z,float dir)
    {
        // 自分のIDだったら
        if (id == Retention.ID)
        {
            Debug.Log("自分のデータの受信");
            if (player == null)
            {
                Debug.Log("プレイヤーのnull確認");
                //generator.GeneratePlayer();
            }
        }
        // 自分以外のIDなら
        else
        {
            // すでに存在している
            if (other_players.ContainsKey(id))
            {
                other_players[id].UpdataData(hp, mp, x, y, z, dir);

            }
            // 存在していないプレイヤー
            else
            {
                other_players.Add(id, generator.GenerateOtherPlayer());
                Debug.Log("ID : " + id + " Data : " + other_players[id]);
            }
        }
    }
}
