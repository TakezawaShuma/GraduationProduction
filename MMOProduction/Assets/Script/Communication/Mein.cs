using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mein : MonoBehaviour
{
    ClientSocket clSoc;

    // Start is called before the first frame update
    void Start()
    {
        clSoc = new ClientSocket();
        string ip = "172.24.52.41";
        int port = 8002;
        clSoc.Connect(ip,port);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
