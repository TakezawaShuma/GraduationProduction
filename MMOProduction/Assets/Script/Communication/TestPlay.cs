using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlay : MonoBehaviour
{
    class TestBox
    {
        public float X;
        public float Y;
        public float Z;
        public int HP;
        public int MP;
        public int Direction;
        public TestBox() { }
        public TestBox(int hp,int mp) { HP = hp;MP = mp; }
    }
    TestBox testbox=new TestBox(500,300);

    // 対応したシーンのConnect○○を作ってね！
    Connect.ConnectPlay connect_play = new Connect.ConnectPlay();

    void Start()
    {
        // Startに必ず入れる事！！
        // 通信関係の初期処理
        connect_play.ConnectionStart();
    }

    void Update()
    {
        Move();
        if (Input.GetKeyDown("s"))
        {
            connect_play.SendPlayData(testbox.HP, testbox.MP, testbox.X, testbox.Y, testbox.Z);
        }
    }

    // OnDestroy関数はシーンマネージャーに必ず入れてください。
    private void OnDestroy()
    {
        // 必ずDestroyはしてください。
        connect_play.Destroy();
    }

    private void Move()
    {
        if(Input.GetKey(KeyCode.X)){ testbox.X++; }
        if (Input.GetKey(KeyCode.Y)) { testbox.Y++; }
        if (Input.GetKey(KeyCode.Z)) { testbox.Z++; }
        if (Input.GetKey(KeyCode.M)) { testbox.MP--; }
        if (Input.GetKey(KeyCode.H)) { testbox.HP--; }
        if (Input.GetKey(KeyCode.D)) { testbox.Direction--; }

    }
}
