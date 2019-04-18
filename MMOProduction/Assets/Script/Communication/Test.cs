using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    struct TestBox
    {
        public float X;
        public float Y;
        public float Z;
        public int HP;
        public int MP;
        public int Direction;
    }
    TestBox testbox;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Packes.SendPosSync a=new Packes.SendPosSync();
        Set(a);

    }

    void Move()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            testbox.X += 10;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            testbox.Y += 10;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            testbox.Z += 10;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            testbox.HP += 10;
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            testbox.MP += 10;
        }

    }
    void Set(Packes.SendPosSync a)
    {
        a.MP = testbox.MP;
        a.HP = testbox.HP;
        a.X = testbox.X;
        a.Y = testbox.Y;
        a.Z = testbox.Z;
    }
}

