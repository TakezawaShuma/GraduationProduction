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
            connect_play.SendPlayData(testbox.hp, testbox.mp, testbox.x, testbox.y, testbox.z);
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
        if(Input.GetKey(KeyCode.X)){ testbox.x++; }
        if (Input.GetKey(KeyCode.Y)) { testbox.y++; }
        if (Input.GetKey(KeyCode.Z)) { testbox.z++; }
        if (Input.GetKey(KeyCode.M)) { testbox.mp--; }
        if (Input.GetKey(KeyCode.H)) { testbox.hp--; }
        if (Input.GetKey(KeyCode.D)) { testbox.dir--; }

    }
}
