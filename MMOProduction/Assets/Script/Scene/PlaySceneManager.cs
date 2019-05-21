using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    Player player;
    PlayerController con;

    Dictionary<int, OtherPlayers> other_players = new Dictionary<int, OtherPlayers>();
    PlayersGenerator generator = new PlayersGenerator();
    Connect.ConnectPlay connect_play = new Connect.ConnectPlay();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        con = GameObject.Find("PlaySceneManager").GetComponent<PlayerController>();
        connect_play.ConnectionStart(Receive);
    }

    // Update is called once per frame
    void Update()
    {
        connect_play.SendPosData(0, 0, con.X, con.Y, con.Z, con.Dir);
        player.UpdatePosition(con.X, con.Y, con.Z);
        player.UpdateDirection(con.Dir);
    }

    private void OnDestroy()
    {
        connect_play.Destroy();
    }

    void Receive(int id, int hp, int mp, float x, float y, float z, float dir)
    {
        // 自分のIDだったら
        if (id == Retention.ID)
        {
            player.UpdatePosition(x, y, z);
            player.UpdateDirection(dir);

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
